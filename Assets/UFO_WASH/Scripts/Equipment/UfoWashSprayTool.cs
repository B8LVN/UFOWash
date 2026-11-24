using UnityEngine;
using UnityEngine.InputSystem;

namespace UFOWASH
{
    [DefaultExecutionOrder(-50)]
    public class UfoWashSprayTool : MonoBehaviour
    {
        public Camera viewCamera;
        public float range = 9f;
        public float cleanPerSecond = 0.38f;
        public float sprayRadius = 0.85f;
        public bool spraying;
        public UfoWashToolMode selectedMode = UfoWashToolMode.PreRinse;

        ParticleSystem sprayParticles;
        ParticleSystem impactParticles;
        float nextImpactTime;
        Material particleMaterial;
        UfoWashFirstPersonController controller;
        UfoWashToolAudio toolAudio;
        readonly RaycastHit[] targetHits = new RaycastHit[64];
        public bool FanNozzleInstalled { get; private set; }
        public bool FanNozzleActive { get; private set; }
        public float ActiveRadius => FanNozzleActive ? Mathf.Max(sprayRadius, 1.25f) : Mathf.Max(sprayRadius, 0.85f);
        public string Feedback { get; private set; } = "AIM AT A CONTRACTED HULL PANEL";

        public void InstallFanNozzle()
        {
            FanNozzleInstalled = true;
            FanNozzleActive = true;
        }

        // The camera handoff and the spray use exactly the same range, obstruction
        // rules and hit point. A footprint is applied around this ray's contact.
        public bool TryGetWashTarget(out RaycastHit target, out UfoWashDirtSurface surface)
        {
            target = default;
            surface = null;
            var career = UfoWashCareerLoop.Instance;
            if (viewCamera == null || career == null || !career.JobAccepted) return false;
            int count = Physics.RaycastNonAlloc(viewCamera.transform.position, viewCamera.transform.forward,
                targetHits, range, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            // Never choose an arbitrary hit from a truncated buffer.
            if (count == targetHits.Length) { Feedback = "TARGET BUFFER FULL // MOVE CLOSER"; return false; }
            System.Array.Sort(targetHits, 0, count, HitDistanceComparer.Instance);
            for (int i = 0; i < count; i++)
            {
                var hit = targetHits[i];
                var collider = hit.collider;
                if (collider == null || collider.transform == transform || collider.transform.IsChildOf(transform)) continue;
                var panel = collider.GetComponentInParent<UfoWashDirtSurface>();
                if (panel != null)
                {
                    if (!career.IsContractSurface(panel)) return false;
                    target = hit;
                    surface = panel;
                    return true;
                }
                if (collider.GetComponentInParent<UfoWashCustomerShip>() != null) continue;
                return false; // A wall, terminal or other solid world object blocks cleaning.
            }
            return false;
        }

        sealed class HitDistanceComparer : System.Collections.Generic.IComparer<RaycastHit>
        {
            public static readonly HitDistanceComparer Instance = new HitDistanceComparer();
            public int Compare(RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance);
        }

        void Awake()
        {
            if (viewCamera == null) viewCamera = GetComponentInChildren<Camera>();
            controller = GetComponent<UfoWashFirstPersonController>();
            toolAudio = gameObject.AddComponent<UfoWashToolAudio>();
            CreateEffects();
        }

        void Update()
        {
            bool inputReady = controller != null && controller.CanControl && Time.timeScale > 0f;
            if (inputReady && Keyboard.current != null)
            {
                if (Keyboard.current.digit1Key.wasPressedThisFrame) selectedMode = UfoWashToolMode.PreRinse;
                if (Keyboard.current.digit2Key.wasPressedThisFrame) selectedMode = UfoWashToolMode.Foam;
                if (Keyboard.current.digit3Key.wasPressedThisFrame) selectedMode = UfoWashToolMode.FinalRinse;
                if (FanNozzleInstalled && Keyboard.current.qKey.wasPressedThisFrame) FanNozzleActive = !FanNozzleActive;
            }

            bool held = inputReady && Mouse.current != null &&
                Mouse.current.leftButton.isPressed &&
                Cursor.lockState == CursorLockMode.Locked;

            var career = UfoWashCareerLoop.Instance;
            spraying = false;
            Feedback = career == null || !career.JobAccepted ? "ACCEPT A CONTRACT AT DISPATCH" :
                !career.CanUseTool(selectedMode) ? career.ToolBlockReason(selectedMode) : "AIM AT A CONTRACTED HULL PANEL";
            if (held && controller.IsCleaningView && TryGetWashTarget(out var target, out var surface))
            {
                // The upgrade's footprint and power now affect actual local work.
                float power = Mathf.Max(0.1f, cleanPerSecond) / 0.38f;
                spraying = career.ApplyTool(surface, selectedMode, Time.deltaTime, target.point, target.normal,
                    ActiveRadius, power);
                if (spraying)
                {
                    Feedback = surface.name.Replace(" - contaminated", "").ToUpperInvariant() +
                        " // DIRT " + Mathf.RoundToInt(surface.dirt * 100f) + "% // FOAM " +
                        Mathf.RoundToInt(surface.foam * 100f) + "%";
                    EmitImpact(target);
                }
                else Feedback = career.LastMessage;
            }
            bool active = spraying;
            toolAudio.SetFlow(active, selectedMode);

            if (sprayParticles != null)
            {
                if (active && !sprayParticles.isPlaying) sprayParticles.Play();
                else if (!active && sprayParticles.isPlaying) sprayParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (sprayParticles != null)
            {
                var main = sprayParticles.main;
                var shape = sprayParticles.shape;
                shape.angle = FanNozzleActive ? 9f : 3f;
                main.startColor = selectedMode == UfoWashToolMode.Foam
                    ? new Color(0.88f, 0.97f, 1f, 0.9f)
                    : new Color(0.38f, 0.84f, 1f, 0.84f);
            }

        }

        void EmitImpact(RaycastHit hit)
        {
                if (impactParticles != null && Time.time >= nextImpactTime)
                {
                    impactParticles.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(hit.normal));
                    var emit = new ParticleSystem.EmitParams
                    {
                        position = hit.point,
                        velocity = hit.normal * Random.Range(.8f, 2f) + Random.insideUnitSphere * 1.2f,
                        startColor = selectedMode == UfoWashToolMode.Foam
                            ? new Color(0.88f, 0.97f, 1f, 0.95f)
                            : new Color(0.35f, 0.88f, 1f, 0.9f),
                        startSize = Random.Range(.035f,.085f)
                    };
                    impactParticles.Emit(emit, 5);
                    nextImpactTime = Time.time + .075f;
                }
        }

        void CreateEffects()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
            if (shader == null) shader = Shader.Find("Particles/Standard Unlit");
            if (shader != null)
            {
                particleMaterial = new Material(shader);
                if (particleMaterial.HasProperty("_BaseColor")) particleMaterial.SetColor("_BaseColor", new Color(.35f,.85f,1f,.85f));
                else if (particleMaterial.HasProperty("_Color")) particleMaterial.color = new Color(.35f,.85f,1f,.85f);
            }

            if (viewCamera != null)
            {
                var sprayObject = new GameObject("SPRAY // pressurized ion-water");
                sprayObject.transform.SetParent(viewCamera.transform, false);
                sprayObject.transform.localPosition = new Vector3(.36f,-.30f,1.02f);
                sprayParticles = sprayObject.AddComponent<ParticleSystem>();
                var main = sprayParticles.main;
                main.loop = true;
                main.playOnAwake = false;
                main.startLifetime = .24f;
                main.startSpeed = 13f;
                main.startSize = .065f;
                main.startColor = new Color(.48f,.9f,1f,.82f);
                main.simulationSpace = ParticleSystemSimulationSpace.Local;
                var emission = sprayParticles.emission;
                emission.rateOverTime = 150f;
                var shape = sprayParticles.shape;
                shape.shapeType = ParticleSystemShapeType.Cone;
                shape.angle = 3f;
                shape.radius = .018f;
                var renderer = sprayObject.GetComponent<ParticleSystemRenderer>();
                renderer.renderMode = ParticleSystemRenderMode.Billboard;
                if (particleMaterial != null) renderer.material = particleMaterial;
                sprayParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            var impactObject = new GameObject("SPRAY IMPACT // water scatter");
            impactObject.transform.SetParent(transform, false);
            impactParticles = impactObject.AddComponent<ParticleSystem>();
            var impactMain = impactParticles.main;
            impactMain.loop = false;
            impactMain.playOnAwake = false;
            impactMain.startLifetime = .28f;
            impactMain.startSpeed = 1.4f;
            impactMain.startSize = .06f;
            impactMain.startColor = new Color(.35f,.88f,1f,.9f);
            impactMain.simulationSpace = ParticleSystemSimulationSpace.World;
            var impactEmission = impactParticles.emission;
            impactEmission.enabled = false;
            var impactShape = impactParticles.shape;
            impactShape.shapeType = ParticleSystemShapeType.Sphere;
            impactShape.radius = .04f;
            var impactRenderer = impactObject.GetComponent<ParticleSystemRenderer>();
            impactRenderer.renderMode = ParticleSystemRenderMode.Billboard;
            if (particleMaterial != null) impactRenderer.material = particleMaterial;
        }

        void OnDestroy()
        {
            if (particleMaterial != null) Destroy(particleMaterial);
        }

        void OnDisable()
        {
            spraying = false;
            if (sprayParticles != null) sprayParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if (impactParticles != null) impactParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if (toolAudio != null) toolAudio.SetFlow(false, selectedMode);
            var audio = GetComponent<AudioSource>();
            if (audio != null) { audio.Stop(); audio.volume = 0f; }
        }
    }
}