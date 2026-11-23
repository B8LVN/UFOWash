using System.Collections.Generic;
using UnityEngine;

namespace UFOWASH
{
    public enum UfoWashToolMode { PreRinse, Foam, FinalRinse }

    public class UfoWashDirtSurface : MonoBehaviour
    {
        [Range(0f,1f)] public float dirt = 1f;
        [Range(0f,1f)] public float foam;
        public static int RewardCredits = 250;
        static readonly List<UfoWashDirtSurface> activeSurfaces = new List<UfoWashDirtSurface>(32);

        Renderer rend;
        MaterialPropertyBlock[] propertyBlocks;
        Color[] originalColors;
        int[] colorProperties;
        static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        static readonly int ColorId = Shader.PropertyToID("_Color");
        static readonly Color dirtTint = new Color(0.48f, 0.30f, 0.16f, 1f);
        static readonly Color foamTint = new Color(0.82f, 0.96f, 1f, 1f);

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
            if (!rend || propertyBlocks == null) return;

            // Tint each authored material slot independently. The original hull
            // palette remains intact while contamination and foam are layered on top.
            Color dirtLayer = Color.Lerp(Color.white, dirtTint, dirt * 0.72f);
            Color finalTint = Color.Lerp(dirtLayer, foamTint, foam * 0.82f);

            for (int i = 0; i < propertyBlocks.Length; i++)
            {
                if (colorProperties[i] < 0) continue;
                var block = propertyBlocks[i];
                rend.GetPropertyBlock(block, i);
                block.SetColor(colorProperties[i], originalColors[i] * finalTint);
                rend.SetPropertyBlock(block, i);
            }
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

        public static void ResetJob()
        {
            UfoWashHUD.JobComplete = false;
            for (int i = 0; i < activeSurfaces.Count; i++)
            {
                var surface = activeSurfaces[i];
                surface.dirt = 1f;
                surface.foam = 0f;
                surface.ApplyColor();
            }
        }
    }
}
