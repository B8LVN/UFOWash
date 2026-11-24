using System;

namespace UFOWASH
{
    // Engine-independent state: visual cells and stage summaries use the same data.
    public sealed class UfoWashCoatingState
    {
        readonly float[] areas;
        readonly float[] dirt;
        readonly float[] foam;
        readonly float totalArea;

        public int Count => dirt.Length;
        public float Dirt => Average(dirt);
        public float Foam => Average(foam);
        public float DirtAt(int index) => dirt[index];
        public float FoamAt(int index) => foam[index];

        public UfoWashCoatingState(float[] cellAreas, float initialDirt, float initialFoam = 0f)
        {
            if (cellAreas == null || cellAreas.Length == 0)
                throw new ArgumentException("A coating requires exposed surface cells.", nameof(cellAreas));
            areas = (float[])cellAreas.Clone();
            dirt = new float[areas.Length];
            foam = new float[areas.Length];
            foreach (float area in areas)
            {
                if (!Finite(area) || area <= 0f) throw new ArgumentException("Cell areas must be positive.");
                totalArea += area;
            }
            Reset(initialDirt, initialFoam);
        }

        public void Reset(float initialDirt, float initialFoam = 0f)
        {
            if (!Finite(initialDirt) || !Finite(initialFoam)) throw new ArgumentException("Invalid coating amount.");
            for (int i = 0; i < Count; i++)
            {
                dirt[i] = Clamp01(initialDirt);
                foam[i] = Clamp01(initialFoam);
            }
        }

        // Only the caller's actual brush footprint is modified; repeating one spot
        // cannot clear or coat the rest of a panel.
        public void Treat(int index, int mode, float seconds)
        {
            if (!Finite(seconds) || seconds <= 0f) return;
            switch (mode)
            {
                case 0:
                    dirt[index] = Math.Min(dirt[index], Math.Max(0.50f, dirt[index] - seconds * 0.80f));
                    break;
                case 1:
                    foam[index] = Clamp01(foam[index] + seconds * 1.6f);
                    break;
                case 2:
                    // Rinse remains effective after the last visible suds disappear,
                    // avoiding a stranded final-rinse stage.
                    dirt[index] = Math.Max(0f, dirt[index] - seconds * (foam[index] > 0.01f ? 0.95f : 0.25f));
                    foam[index] = Math.Max(0f, foam[index] - seconds * 0.85f);
                    break;
            }
        }

        float Average(float[] values)
        {
            float sum = 0f;
            for (int i = 0; i < Count; i++) sum += values[i] * areas[i];
            return sum / totalArea;
        }

        static float Clamp01(float value) => Math.Max(0f, Math.Min(1f, value));
        static bool Finite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    }
}
