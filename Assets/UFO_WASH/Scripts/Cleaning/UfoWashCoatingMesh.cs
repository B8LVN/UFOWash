using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UFOWASH
{
    // Experimental UV-independent coating for the existing static hull meshes.
    // One bounded overlay draw per panel, no per-spray GameObjects/materials.
    internal sealed class UfoWashCoatingMesh : IDisposable
    {
        const float CellEdge = 0.32f;
        const int MaxCells = 32768;
        readonly Transform owner;
        readonly List<Vector3> centers = new List<Vector3>();
        readonly List<Vector3> normals = new List<Vector3>();
        readonly Mesh mesh;
        readonly Material material;
        readonly GameObject overlay;
        readonly Color[] colors;
        public UfoWashCoatingState State { get; }

        public UfoWashCoatingMesh(UfoWashDirtSurface surface, Color tint)
        {
            owner = surface.transform;
            var filter = surface.GetComponent<MeshFilter>();
            if (filter == null || filter.sharedMesh == null || !filter.sharedMesh.isReadable)
                throw new InvalidOperationException("Local coating requires a readable static hull mesh: " + surface.name);
            var shader = Shader.Find("UFO WASH/Local Coating");
            if (shader == null) throw new InvalidOperationException("Missing UFO WASH/Local Coating shader.");

            var vertices = new List<Vector3>();
            var weights = new List<float>();
            Mesh source = filter.sharedMesh;
            var points = source.vertices;
            var triangles = source.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
                Subdivide(points[triangles[i]], points[triangles[i + 1]], points[triangles[i + 2]],
                    0, vertices, weights);
            if (weights.Count == 0)
                throw new InvalidOperationException("Mesh contains no non-degenerate triangles: " +
                    surface.name + " (vertices=" + points.Length + ", triangles=" + triangles.Length + ")");
            State = new UfoWashCoatingState(weights.ToArray(), surface.dirt, surface.foam);
            mesh = new Mesh { name = surface.name + " // local coating", indexFormat = IndexFormat.UInt32 };
            mesh.MarkDynamic();
            mesh.SetVertices(vertices);
            var indices = new int[vertices.Count];
            for (int i = 0; i < indices.Length; i++) indices[i] = i;
            mesh.SetTriangles(indices, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            colors = new Color[vertices.Count];
            material = new Material(shader) { name = "Local grime + persistent foam" };
            material.SetColor("_DirtColor", tint);
            overlay = new GameObject("COATING // LOCAL DIRT + FOAM") { layer = 2 };
            overlay.transform.SetParent(owner, false);
            overlay.AddComponent<MeshFilter>().sharedMesh = mesh;
            var renderer = overlay.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = true;
            Refresh();
        }

        void Subdivide(Vector3 a, Vector3 b, Vector3 c, int depth, List<Vector3> vertices,
            List<float> weights)
        {
            Vector3 wa = owner.TransformPoint(a), wb = owner.TransformPoint(b), wc = owner.TransformPoint(c);
            Vector3 cross = Vector3.Cross(wb - wa, wc - wa);
            float area = cross.magnitude * 0.5f;
            if (area < 0.00001f) return;
            float ab = (wa - wb).sqrMagnitude, bc = (wb - wc).sqrMagnitude, ca = (wc - wa).sqrMagnitude;
            if (depth < 10 && Mathf.Max(ab, Mathf.Max(bc, ca)) > CellEdge * CellEdge)
            {
                // Bisect the longest edge; avoid exponential four-way subdivision.
                if (ab >= bc && ab >= ca)
                {
                    Vector3 mid = (a + b) * 0.5f;
                    Subdivide(a, mid, c, depth + 1, vertices, weights);
                    Subdivide(mid, b, c, depth + 1, vertices, weights);
                }
                else if (bc >= ca)
                {
                    Vector3 mid = (b + c) * 0.5f;
                    Subdivide(a, b, mid, depth + 1, vertices, weights);
                    Subdivide(a, mid, c, depth + 1, vertices, weights);
                }
                else
                {
                    Vector3 mid = (c + a) * 0.5f;
                    Subdivide(a, b, mid, depth + 1, vertices, weights);
                    Subdivide(mid, b, c, depth + 1, vertices, weights);
                }
                return;
            }
            Vector3 center = (a + b + c) / 3f;
            Vector3 normal = cross.normalized;
            // Do not use physics occlusion tests here. The saucer contains overlapping
            // hull/cockpit colliders; raycasting those against every subdivided face
            // incorrectly classified the entire upper shell as buried. The coating is
            // generated from this renderer's own triangles. During spraying, the shared
            // target query and per-cell normals still restrict work to the visible side.
            if (centers.Count >= MaxCells)
                throw new InvalidOperationException("Hull coating cell budget exceeded: " + owner.name);
            Vector3 offset = owner.InverseTransformVector(normal * 0.006f);
            vertices.Add(a + offset); vertices.Add(b + offset); vertices.Add(c + offset);
            centers.Add(center);
            normals.Add(owner.localToWorldMatrix.transpose.MultiplyVector(normal).normalized);
            weights.Add(area);
        }

        public bool Process(UfoWashToolMode mode, float seconds, Vector3 point, Vector3 normal, float radius)
        {
            float radiusSquared = radius * radius;
            Matrix4x4 toWorld = owner.localToWorldMatrix;
            Matrix4x4 normalMatrix = owner.worldToLocalMatrix.transpose;
            bool changed = false;
            for (int i = 0; i < centers.Count; i++)
            {
                Vector3 worldCenter = toWorld.MultiplyPoint3x4(centers[i]);
                Vector3 cellNormal = normalMatrix.MultiplyVector(normals[i]).normalized;
                if (Vector3.Dot(cellNormal, normal) < 0.25f || (worldCenter - point).sqrMagnitude > radiusSquared) continue;
                // Do not wrap a brush through thin hulls or around sharp corners.
                if (Mathf.Abs(Vector3.Dot(worldCenter - point, normal)) > 0.16f) continue;
                changed |= State.Treat(i, (int)mode, seconds);
            }
            if (!changed) return false;
            Refresh();
            return true;
        }

        public void Reset(Color tint, float dirt, float foam = 0f)
        {
            material.SetColor("_DirtColor", tint);
            State.Reset(dirt, foam);
            Refresh();
        }

        void Refresh()
        {
            for (int i = 0; i < State.Count; i++)
            {
                var value = new Color(State.DirtAt(i), State.FoamAt(i), 0f, 1f);
                colors[i * 3] = value; colors[i * 3 + 1] = value; colors[i * 3 + 2] = value;
            }
            mesh.colors = colors;
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(overlay);
            UnityEngine.Object.Destroy(mesh);
            UnityEngine.Object.Destroy(material);
        }
    }
}
