using UnityEngine;

namespace UFOWASH
{
    public class UfoWashDirtSurface : MonoBehaviour
    {
        [Range(0f,1f)] public float dirt = 1f;
        public static int RewardCredits = 250;
        Renderer rend;
        Color cleanColor = new Color(0.18f, 0.78f, 0.9f);
        Color dirtyColor = new Color(0.32f, 0.19f, 0.12f);
        static bool jobPaid;
        void Awake() { rend = GetComponent<Renderer>(); ApplyColor(); }
        public void Clean(float amount)
        {
            if (jobPaid) return;
            dirt = Mathf.Clamp01(dirt - amount);
            ApplyColor();
            if (TotalDirt() <= 0.015f && !jobPaid) { jobPaid = true; UfoWashHUD.Credits += RewardCredits; UfoWashHUD.JobComplete = true; }
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
            float sum=0; foreach(var s in surfaces) sum += s.dirt;
            return sum / surfaces.Length;
        }
        public static void ResetJob()
        {
            jobPaid=false; UfoWashHUD.JobComplete=false;
            var surfaces=FindObjectsByType<UfoWashDirtSurface>(FindObjectsSortMode.None);
            foreach(var s in surfaces){s.dirt=1f;s.ApplyColor();}
        }
    }
}