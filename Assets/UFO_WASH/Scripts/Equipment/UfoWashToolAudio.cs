using UnityEngine;

namespace UFOWASH
{
    // Original synthesized pressure/noise layer, not a licensed recording or final audio pass.
    [RequireComponent(typeof(AudioSource))]
    public sealed class UfoWashToolAudio : MonoBehaviour
    {
        AudioSource source;
        AudioClip pressure;

        void Awake()
        {
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = true;
            source.spatialBlend = 0f;
            source.volume = 0f;
            const int rate = 22050;
            var samples = new float[rate * 2];
            var random = new System.Random(2189);
            float filtered = 0f;
            for (int i = 0; i < samples.Length; i++)
            {
                float noise = (float)random.NextDouble() * 2f - 1f;
                filtered = Mathf.Lerp(filtered, noise, 0.2f);
                float phase = i / (float)rate;
                // Integral cycles and tapered ends keep the loop boundary quiet.
                float edge = Mathf.Min(1f, Mathf.Min(i, samples.Length - i - 1) / 220f);
                samples[i] = (filtered * 0.65f + Mathf.Sin(phase * 2f * Mathf.PI * 90f) * 0.055f) * edge;
            }
            pressure = AudioClip.Create("UFO WASH // original pressure loop", samples.Length, 1, rate, false);
            pressure.SetData(samples, 0);
            source.clip = pressure;
        }

        public void SetFlow(bool flowing, UfoWashToolMode mode)
        {
            if (source == null) return;
            source.pitch = mode == UfoWashToolMode.Foam ? 0.68f : mode == UfoWashToolMode.FinalRinse ? 1.12f : 0.94f;
            source.volume = Mathf.MoveTowards(source.volume, flowing ? 0.24f : 0f, Time.unscaledDeltaTime * 2f);
            if (flowing && !source.isPlaying) source.Play();
            else if (!flowing && source.volume <= 0f) source.Stop();
        }

        void OnDisable() { if (source != null) { source.Stop(); source.volume = 0f; } }
        void OnDestroy() { if (pressure != null) Destroy(pressure); }
    }
}
