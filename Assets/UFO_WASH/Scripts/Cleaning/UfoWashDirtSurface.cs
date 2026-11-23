using UnityEngine;

namespace UFOWASH
{
    public enum UfoWashToolMode { PreRinse, Foam, FinalRinse }

    public class UfoWashDirtSurface : MonoBehaviour
    {
        [Range(0f,1f)] public float dirt = 1f;
        [Range(0f,1f)] public float foam;
        public static int RewardCredits = 250;

        Renderer rend;
        MaterialPropertyBlock propertyBlock;
        Color originalColor = Color.white;
        bool usesBaseColor;
        static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        static readonly int ColorId = Shader.PropertyToID("_Color");
        static readonly Color dirtTint = new Color(0.48f, 0.30f, 0.16f, 1f);
        static readonly Color foamTint = new Color(0.82f, 0.96f, 1f, 1f);

        void Awake()
        {
            rend = GetComponent<Renderer>();
            propertyBlock = new MaterialPropertyBlock();
            if (rend != null && rend.sharedMaterial != null)
            {
                var material = rend.sharedMaterial;
                if (material.HasProperty(BaseColorId))
                {
                    originalColor = material.GetColor(BaseColorId);
                    usesBaseColor = true;
                }
                else if (material.HasProperty(ColorId))
                {
                    originalColor = material.GetColor(ColorId);
                    usesBaseColor = false;
                }
            }
            ApplyColor();
        }

        public void Process(UfoWashToolMode mode, float seconds)
        {
            if (seconds <= 0f) return;

            switch (mode)
            {
                case UfoWashToolMode.PreRinse:
                    dirt = Mathf.Clamp01(dirt - seconds * 0.19f);
                    break;
                case UfoWashToolMode.Foam:
                    foam = Mathf.Clamp01(foam + seconds * 0.28f);
                    break;
                case UfoWashToolMode.FinalRinse:
                    if (foam > 0.08f)
                    {
                        dirt = Mathf.Clamp01(dirt - seconds * 0.25f);
                        foam = Mathf.Clamp01(foam - seconds * 0.2f);
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
            if (!rend || !usesBaseColor) return;
            if (propertyBlock == null) propertyBlock = new MaterialPropertyBlock();

            // Tint the authored hull material instead of replacing it. This keeps
            // the ship's metal/glass/accent identity visible throughout cleaning.
            Color dirtLayer = Color.Lerp(Color.white, dirtTint, dirt * 0.72f);
            Color finalTint = Color.Lerp(dirtLayer, foamTint, foam * 0.82f);
            Color result = originalColor * finalTint;

            rend.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(rend.sharedMaterial.HasProperty(BaseColorId) ? BaseColorId : ColorId, result);
            rend.SetPropertyBlock(propertyBlock);
        }

        public static float TotalDirt()
        {
            var surfaces = FindObjectsByType<UfoWashDirtSurface>(FindObjectsSortMode.None);
            if (surfaces.Length == 0) return 0f;
            float sum = 0f;
            foreach (var surface in surfaces) sum += surface.dirt;
            return sum / surfaces.Length;
        }

        public static float TotalFoam()
        {
            var surfaces = FindObjectsByType<UfoWashDirtSurface>(FindObjectsSortMode.None);
            if (surfaces.Length == 0) return 0f;
            float sum = 0f;
            foreach (var surface in surfaces) sum += surface.foam;
            return sum / surfaces.Length;
        }

        public static void ResetJob()
        {
            UfoWashHUD.JobComplete = false;
            var surfaces = FindObjectsByType<UfoWashDirtSurface>(FindObjectsSortMode.None);
            foreach (var surface in surfaces)
            {
                surface.dirt = 1f;
                surface.foam = 0f;
                surface.ApplyColor();
            }
        }
    }
}
