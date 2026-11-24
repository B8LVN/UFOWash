using System.Collections.Generic;
using UnityEngine;

namespace UFOWASH
{
    public enum UfoWashToolMode { PreRinse, Foam, FinalRinse }

    [RequireComponent(typeof(Renderer))]
    public class UfoWashDirtSurface : MonoBehaviour
    {
        // Preserve serialized fields and existing scene references. Runtime totals
        // are derived from the local coating, never a whole-renderer tint.
        [Range(0f, 1f)] public float dirt = 1f;
        [Range(0f, 1f)] public float foam;
        public static int RewardCredits = 250;
        static readonly List<UfoWashDirtSurface> activeSurfaces = new List<UfoWashDirtSurface>(32);
        UfoWashCoatingMesh coating;
        Mesh correctedHull;
        Mesh originalHull;
        Mesh originalColliderMesh;
        bool reportedCoatingFailure;
        Color contaminationTint = new Color(0.48f, 0.30f, 0.16f);
        public bool CoatingReady => coating != null;

        void OnEnable() { if (!activeSurfaces.Contains(this)) activeSurfaces.Add(this); }
        void OnDisable() { activeSurfaces.Remove(this); }
        void OnDestroy()
        {
            coating?.Dispose();
            if (correctedHull == null) return;
            var filter = GetComponent<MeshFilter>();
            if (filter != null && filter.sharedMesh == correctedHull) filter.sharedMesh = originalHull;
            var collider = GetComponent<MeshCollider>();
            if (collider != null && collider.sharedMesh == correctedHull) collider.sharedMesh = originalColliderMesh;
            Destroy(correctedHull);
        }

        void RepairHullWinding()
        {
            if (correctedHull != null) return;
            var filter = GetComponent<MeshFilter>();
            if (filter == null || filter.sharedMesh == null || !filter.sharedMesh.isReadable) return;
            Mesh source = filter.sharedMesh;
            var vertices = source.vertices;
            var xyz = new float[vertices.Length * 3];
            for (int i = 0; i < vertices.Length; i++)
            {
                xyz[i * 3] = vertices[i].x;
                xyz[i * 3 + 1] = vertices[i].y;
                xyz[i * 3 + 2] = vertices[i].z;
            }
            if (!UfoWashHullWinding.IsInward(xyz, source.triangles)) return;
            originalHull = source;
            correctedHull = Instantiate(source);
            correctedHull.name = source.name + " // outward runtime copy";
            // Preserve material/submesh assignments.
            for (int submesh = 0; submesh < correctedHull.subMeshCount; submesh++)
            {
                var indices = correctedHull.GetTriangles(submesh);
                for (int i = 0; i < indices.Length; i += 3)
                {
                    int swap = indices[i + 1];
                    indices[i + 1] = indices[i + 2];
                    indices[i + 2] = swap;
                }
                correctedHull.SetTriangles(indices, submesh);
            }
            correctedHull.RecalculateNormals();
            correctedHull.RecalculateBounds();
            filter.sharedMesh = correctedHull;
            var collider = GetComponent<MeshCollider>();
            if (collider != null && collider.sharedMesh == source)
            {
                originalColliderMesh = collider.sharedMesh;
                collider.sharedMesh = correctedHull;
            }
        }

        public bool PrepareCoating()
        {
            if (coating != null) return true;
            try
            {
                RepairHullWinding();
                Physics.SyncTransforms();
                coating = new UfoWashCoatingMesh(this, contaminationTint);
                reportedCoatingFailure = false;
            }
            catch (System.Exception exception)
            {
                if (!reportedCoatingFailure)
                    Debug.LogError("UFO WASH // Local coating unavailable on " + name + ": " + exception.Message, this);
                reportedCoatingFailure = true;
                return false;
            }
            return true;
        }

        public bool Process(UfoWashToolMode mode, float seconds)
            => Process(mode, seconds, transform.position, transform.up, 0.65f);

        public bool Process(UfoWashToolMode mode, float seconds, Vector3 hitPoint, Vector3 hitNormal, float sprayRadius)
        {
            if (seconds <= 0f || !float.IsFinite(seconds) || !PrepareCoating()) return false;
            if (!coating.Process(mode, seconds, hitPoint, hitNormal.normalized, Mathf.Clamp(sprayRadius, 0.15f, 1.5f)))
                return false;
            dirt = coating.State.Dirt;
            foam = coating.State.Foam;
            return true;
        }

        public void Clean(float amount) => Process(UfoWashToolMode.PreRinse, amount);

        void SetProfile(Color tint, float startingDirt)
        {
            contaminationTint = tint;
            dirt = Mathf.Clamp01(startingDirt);
            foam = 0f;
            if (PrepareCoating()) coating.Reset(tint, dirt);
        }

        public static float TotalDirt() => AverageDirt(activeSurfaces);
        public static float TotalFoam() => AverageFoam(activeSurfaces);
        public static float AverageDirt(IList<UfoWashDirtSurface> surfaces) => Average(surfaces, false);
        public static float AverageFoam(IList<UfoWashDirtSurface> surfaces) => Average(surfaces, true);

        static float Average(IList<UfoWashDirtSurface> surfaces, bool useFoam)
        {
            if (surfaces == null || surfaces.Count == 0) return useFoam ? 0f : 1f;
            float sum = 0f;
            int count = 0;
            for (int i = 0; i < surfaces.Count; i++)
            {
                if (surfaces[i] == null) continue;
                sum += useFoam ? surfaces[i].foam : surfaces[i].dirt;
                count++;
            }
            return count == 0 ? (useFoam ? 0f : 1f) : sum / count;
        }

        public static void SetJobProfile(Color tint, float startingDirt) => SetJobProfile(activeSurfaces, tint, startingDirt);
        public static void SetJobProfile(IList<UfoWashDirtSurface> surfaces, Color tint, float startingDirt)
        {
            UfoWashHUD.JobComplete = false;
            if (surfaces == null) return;
            for (int i = 0; i < surfaces.Count; i++)
                if (surfaces[i] != null) surfaces[i].SetProfile(tint, startingDirt);
        }

        public static void ResetJob() => ResetJob(activeSurfaces);
        public static void ResetJob(IList<UfoWashDirtSurface> surfaces)
        {
            UfoWashHUD.JobComplete = false;
            if (surfaces == null) return;
            for (int i = 0; i < surfaces.Count; i++)
                if (surfaces[i] != null) surfaces[i].SetProfile(surfaces[i].contaminationTint, 1f);
        }
    }
}
