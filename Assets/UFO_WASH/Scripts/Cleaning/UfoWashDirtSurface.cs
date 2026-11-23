using UnityEngine;

namespace UFOWASH
{
    public class UfoWashDirtSurface : MonoBehaviour
    {
        [Range(0f,1f)] public float dirt = 1f;
        public static int RewardCredits = 250;
        Renderer rend;
        static readonly Color cleanColor = new Color(0.18f, 0.78f, 0.9f);
        static readonly Color dirtyColor = new Color(0.32f, 0.19f, 0.12f);

        void Awake()
        {
            rend = GetComponent<Renderer>();
            ApplyColor();
        }

        public void Clean(float amount)
        {
            var career = UfoWashCareerLoop.Instance;
            if (career != null && !career.JobAccepted) return;
            if (UfoWashHUD.JobComplete) return;

            dirt = Mathf.Clamp01(dirt - amount);
            ApplyColor();

            if (TotalDirt() <= 0.015f)
            {
                if (career != null) career.CompleteJob();
                else
                {
                    UfoWashHUD.Credits += RewardCredits;
                    UfoWashHUD.JobComplete = true;
                }
            }
        }

        void ApplyColor()
        {
            if (!rend) rend = GetComponent<Renderer>();
            if (!rend) return;
            Color c = Color.Lerp(cleanColor, dirtyColor, dirt);
            if (rend.material.HasProperty("_BaseColor")) rend.material.SetColor("_BaseColor", c);
            else if (rend.material.HasProperty("_Color")) rend.material.color = c;
        }

        public static float TotalDirt()
        {
            var surfaces = FindObjectsByType<UfoWashDirtSurface>(FindObjectsSortMode.None);
            if (surfaces.Length == 0) return 0;
            float sum = 0;
            foreach (var surface in surfaces) sum += surface.dirt;
            return sum / surfaces.Length;
        }

        public static void ResetJob()
        {
            UfoWashHUD.JobComplete = false;
            var surfaces = FindObjectsByType<UfoWashDirtSurface>(FindObjectsSortMode.None);
            foreach (var surface in surfaces)
            {
                surface.dirt = 1f;
                surface.ApplyColor();
            }
        }
    }
}