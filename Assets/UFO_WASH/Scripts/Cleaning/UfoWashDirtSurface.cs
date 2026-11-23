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
        static readonly Color cleanColor = new Color(0.18f, 0.78f, 0.9f);
        static readonly Color dirtyColor = new Color(0.32f, 0.19f, 0.12f);
        static readonly Color foamColor = new Color(0.72f, 0.94f, 1f);

        void Awake()
        {
            rend = GetComponent<Renderer>();
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
            if (TotalDirt() <= 0.015f)
            {
                var career = UfoWashCareerLoop.Instance;
                if (career != null) career.CompleteJob();
                else
                {
                    UfoWashHUD.Credits += RewardCredits;
                    UfoWashHUD.JobComplete = true;
                }
            }
        }

        public void Clean(float amount)
        {
            Process(UfoWashToolMode.PreRinse, amount);
        }

        void ApplyColor()
        {
            if (!rend) rend = GetComponent<Renderer>();
            if (!rend) return;

            Color surfaceColor = Color.Lerp(cleanColor, dirtyColor, dirt);
            surfaceColor = Color.Lerp(surfaceColor, foamColor, foam * 0.9f);
            Material material = rend.material;
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", surfaceColor);
            else if (material.HasProperty("_Color")) material.color = surfaceColor;
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
