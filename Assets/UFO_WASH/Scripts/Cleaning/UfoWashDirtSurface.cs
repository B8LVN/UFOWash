using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UFOWASH
{
    public enum UfoWashToolMode { PreRinse, Foam, FinalRinse }

    [RequireComponent(typeof(Renderer))]
    public class UfoWashDirtSurface : MonoBehaviour
    {
        [Range(0f, 1f)] public float dirt = 1f;
        [Range(0f, 1f)] public float foam;
        public static int RewardCredits = 250;

        static readonly List<UfoWashDirtSurface> activeSurfaces = new List<UfoWashDirtSurface>(32);
        static Material foamMaterial;
        static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        static readonly int ColorId = Shader.PropertyToID("_Color");

        Renderer rend;
        MaterialPropertyBlock[] propertyBlocks;
        Color[] originalColors;
        int[] colorProperties;
        readonly List<Renderer> foamPatches = new List<Renderer>(32);
        static MaterialPropertyBlock foamBlock;
        float foamPatchTimer;
        Color contaminationTint = new Color(0.48f, 0.30f, 0.16f, 1f);

        void Awake()
        {
            rend = GetComponent<Renderer>();
            if (rend != null)
            {
                var materials = rend.sharedMaterials;
                propertyBlocks = new MaterialPropertyBlock[materials.Length];
                originalColors = new Color[materials.Length];
                colorProperties = new int[materials.Length];

                for (int i = 0; i < materials.Length; i++)
                {
                    propertyBlocks[i] = new MaterialPropertyBlock();
                    originalColors[i] = Color.white;
                    colorProperties[i] = -1;
                    var material = materials[i];
                    if (material == null) continue;

                    if (material.HasProperty(BaseColorId))
                    {
                        originalColors[i] = material.GetColor(BaseColorId);
                        colorProperties[i] = BaseColorId;
                    }
                    else if (material.HasProperty(ColorId))
                    {
                        originalColors[i] = material.GetColor(ColorId);
                        colorProperties[i] = ColorId;
                    }
                }
            }
            ApplyColor();
        }

        void OnEnable()
        {
            if (!activeSurfaces.Contains(this)) activeSurfaces.Add(this);
        }

        void OnDisable()
        {
            activeSurfaces.Remove(this);
            ClearFoamPatches();
        }

        void OnDestroy()
        {
            ClearFoamPatches();
        }

        public void Process(UfoWashToolMode mode, float seconds)
        {
            Process(mode, seconds, transform.position, transform.up, 0.3f);
        }

        public void Process(UfoWashToolMode mode, float seconds, Vector3 hitPoint, Vector3 hitNormal, float sprayRadius)
        {
            if (seconds <= 0f) return;

            switch (mode)
            {
                case UfoWashToolMode.PreRinse:
                    dirt = Mathf.Clamp01(dirt - seconds * 0.19f);
                    break;

                case UfoWashToolMode.Foam:
                    foam = Mathf.Clamp01(foam + seconds * 0.28f);
                    foamPatchTimer += seconds;
                    while (foamPatchTimer >= 0.075f)
                    {
                        foamPatchTimer -= 0.075f;
                        AddFoamPatch(hitPoint, hitNormal, Mathf.Clamp(sprayRadius * Random.Range(0.72f, 1.12f), 0.12f, 0.48f));
                    }
                    break;

                case UfoWashToolMode.FinalRinse:
                    if (foam > 0.01f)
                    {
                        dirt = Mathf.Clamp01(dirt - seconds * 0.25f);
                        foam = Mathf.Clamp01(foam - seconds * 0.2f);
                        RefreshFoamPatches();
                    }
                    break;
            }

            ApplyColor();
        }

        public void Clean(float amount)
        {
            Process(UfoWashToolMode.PreRinse, amount);
        }

        void ApplyColor()
        {
            if (!rend) rend = GetComponent<Renderer>();
            if (!rend || propertyBlocks == null) return;

            // Dirt is a hull-level contamination tint. Foam is rendered separately
            // as local patches exactly where the nozzle contacts the ship.
            Color dirtLayer = Color.Lerp(Color.white, contaminationTint, dirt * 0.72f);
            for (int i = 0; i < propertyBlocks.Length; i++)
            {
                if (colorProperties[i] < 0) continue;
                var block = propertyBlocks[i];
                rend.GetPropertyBlock(block, i);
                block.SetColor(colorProperties[i], originalColors[i] * dirtLayer);
                rend.SetPropertyBlock(block, i);
            }
        }

        void AddFoamPatch(Vector3 worldPoint, Vector3 worldNormal, float radius)
        {
            if (foamPatches.Count >= 72)
            {
                var oldest = foamPatches[0];
                foamPatches.RemoveAt(0);
                if (oldest != null) Destroy(oldest.gameObject);
            }

            var patch = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            patch.name = "FOAM PATCH // LIVE APPLICATION";
            patch.layer = 2; // Ignore Raycast: foam must not become a spray obstruction.
            var collider = patch.GetComponent<Collider>();
            if (collider != null) Destroy(collider);

            patch.transform.SetParent(transform, false);
            Vector3 normal = worldNormal.sqrMagnitude > 0.001f ? worldNormal.normalized : transform.up;
            Vector3 localNormal = transform.InverseTransformDirection(normal).normalized;
            patch.transform.localPosition = transform.InverseTransformPoint(worldPoint + normal * 0.018f);
            patch.transform.localRotation = Quaternion.FromToRotation(Vector3.up, localNormal) *
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            Vector3 scale = transform.lossyScale;
            scale = new Vector3(Mathf.Max(0.001f, Mathf.Abs(scale.x)),
                Mathf.Max(0.001f, Mathf.Abs(scale.y)), Mathf.Max(0.001f, Mathf.Abs(scale.z)));
            patch.transform.localScale = new Vector3(radius * 1.45f / scale.x, 0.035f / scale.y, radius / scale.z);

            var patchRenderer = patch.GetComponent<Renderer>();
            Material material = GetFoamMaterial();
            if (material == null) { Destroy(patch); return; }
            patchRenderer.sharedMaterial = material;
            patchRenderer.shadowCastingMode = ShadowCastingMode.Off;
            patchRenderer.receiveShadows = false;
            foamPatches.Add(patchRenderer);
            SetPatchOpacity(patchRenderer, foam);
        }

        void RefreshFoamPatches()
        {
            if (foam <= 0.015f)
            {
                ClearFoamPatches();
                return;
            }

            for (int i = foamPatches.Count - 1; i >= 0; i--)
            {
                var patch = foamPatches[i];
                if (patch == null) { foamPatches.RemoveAt(i); continue; }
                SetPatchOpacity(patch, foam);
            }
        }

        static void SetPatchOpacity(Renderer patch, float amount)
        {
            // UnityEngine.Object-backed rendering helpers must be constructed after
            // Unity has entered its runtime lifecycle, never in a static field initializer.
            if (foamBlock == null) foamBlock = new MaterialPropertyBlock();
            foamBlock.Clear();
            Color color = new Color(0.82f, 0.97f, 1f, Mathf.Clamp01(amount) * 0.94f);
            foamBlock.SetColor(BaseColorId, color);
            foamBlock.SetColor(ColorId, color);
            patch.SetPropertyBlock(foamBlock);
        }

        void ClearFoamPatches()
        {
            for (int i = 0; i < foamPatches.Count; i++)
                if (foamPatches[i] != null)
                {
                    if (Application.isPlaying) Destroy(foamPatches[i].gameObject);
                    else DestroyImmediate(foamPatches[i].gameObject);
                }
            foamPatches.Clear();
            foamPatchTimer = 0f;
        }

        static Material GetFoamMaterial()
        {
            if (foamMaterial != null) return foamMaterial;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Unlit/Transparent");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            if (shader == null) return null;

            foamMaterial = new Material(shader) { name = "UFO WASH // Translucent Foam" };
            Color foamColor = new Color(0.82f, 0.97f, 1f, 0.94f);
            if (foamMaterial.HasProperty(BaseColorId)) foamMaterial.SetColor(BaseColorId, foamColor);
            if (foamMaterial.HasProperty(ColorId)) foamMaterial.SetColor(ColorId, foamColor);
            if (foamMaterial.HasProperty("_Surface"))
            {
                foamMaterial.SetFloat("_Surface", 1f);
                foamMaterial.SetFloat("_Blend", 0f);
                foamMaterial.SetFloat("_ZWrite", 0f);
                foamMaterial.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
                foamMaterial.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
                foamMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
            foamMaterial.renderQueue = (int)RenderQueue.Transparent;
            return foamMaterial;
        }

        public static float TotalDirt()
        {
            if (activeSurfaces.Count == 0) return 0f;
            float sum = 0f;
            for (int i = 0; i < activeSurfaces.Count; i++) sum += activeSurfaces[i].dirt;
            return sum / activeSurfaces.Count;
        }

        public static float TotalFoam()
        {
            if (activeSurfaces.Count == 0) return 0f;
            float sum = 0f;
            for (int i = 0; i < activeSurfaces.Count; i++) sum += activeSurfaces[i].foam;
            return sum / activeSurfaces.Count;
        }

        public static float AverageDirt(IList<UfoWashDirtSurface> surfaces)
        {
            if (surfaces == null || surfaces.Count == 0) return 1f;
            float sum = 0f;
            int count = 0;
            for (int i = 0; i < surfaces.Count; i++)
            {
                if (surfaces[i] == null) continue;
                sum += surfaces[i].dirt;
                count++;
            }
            return count == 0 ? 1f : sum / count;
        }

        public static float AverageFoam(IList<UfoWashDirtSurface> surfaces)
        {
            if (surfaces == null || surfaces.Count == 0) return 0f;
            float sum = 0f;
            int count = 0;
            for (int i = 0; i < surfaces.Count; i++)
            {
                if (surfaces[i] == null) continue;
                sum += surfaces[i].foam;
                count++;
            }
            return count == 0 ? 0f : sum / count;
        }

        public static void SetJobProfile(Color tint, float startingDirt)
        {
            UfoWashHUD.JobComplete = false;
            SetJobProfile(activeSurfaces, tint, startingDirt);
        }

        public static void SetJobProfile(IList<UfoWashDirtSurface> surfaces, Color tint, float startingDirt)
        {
            UfoWashHUD.JobComplete = false;
            if (surfaces == null) return;
            for (int i = 0; i < surfaces.Count; i++)
            {
                var surface = surfaces[i];
                if (surface == null) continue;
                surface.contaminationTint = tint;
                surface.dirt = Mathf.Clamp01(startingDirt);
                surface.foam = 0f;
                surface.ClearFoamPatches();
                surface.ApplyColor();
            }
        }

        public static void ResetJob()
        {
            UfoWashHUD.JobComplete = false;
            ResetJob(activeSurfaces);
        }

        public static void ResetJob(IList<UfoWashDirtSurface> surfaces)
        {
            UfoWashHUD.JobComplete = false;
            if (surfaces == null) return;
            for (int i = 0; i < surfaces.Count; i++)
            {
                var surface = surfaces[i];
                if (surface == null) continue;
                surface.dirt = 1f;
                surface.foam = 0f;
                surface.ClearFoamPatches();
                surface.ApplyColor();
            }
        }
    }
}
