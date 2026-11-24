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
        Color contaminationTint = new Color(0.48f, 0.30f, 0.16f);
        public bool CoatingReady => coating != null;

        void OnEnable() { if (!activeSurfaces.Contains(this)) activeSurfaces.Add(this); }
        void OnDisable() { activeSurfaces.Remove(this); }
        void OnDestroy() { coating?.Dispose(); }

        public bool PrepareCoating()
        {
            if (coating != null) return true;
            try { coating = new UfoWashCoatingMesh(this, contaminationTint); }
            catch (System.Exception exception)
            {
                Debug.LogError("UFO WASH // Local coating unavailable on " + name + ": " + exception.Message, this);
                return false;
            }
            return true;
        }

        public void Process(UfoWashToolMode mode, float seconds)
            => Process(mode, seconds, transform.position, transform.up, 0.65f);

        public void Process(UfoWashToolMode mode, float seconds, Vector3 hitPoint, Vector3 hitNormal, float sprayRadius)
        {
            if (seconds <= 0f || !float.IsFinite(seconds) || !PrepareCoating()) return;
            coating.Process(mode, seconds, hitPoint, hitNormal.normalized, Mathf.Clamp(sprayRadius, 0.15f, 1.5f));
            dirt = coating.State.Dirt;
            foam = coating.State.Foam;
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
