using UnityEngine;
using UnityEngine.InputSystem;

namespace UFOWASH
{
    public class UfoWashSprayTool : MonoBehaviour
    {
        public Camera viewCamera;
        public float range = 9f;
        public float cleanPerSecond = 0.38f;
        public float sprayRadius = 0.35f;
        public bool spraying;
        public UfoWashToolMode selectedMode = UfoWashToolMode.PreRinse;

        ParticleSystem sprayParticles;
        ParticleSystem impactParticles;
        float nextImpactTime;
        Material particleMaterial;

        void Awake()
        {
            if (viewCamera == null) viewCamera = GetComponentInChildren<Camera>();
            CreateEffects();
        }

        void Update()
        {
            if (Keyboard.current != null)
            {
                if (Keyboard.current.digit1Key.wasPressedThisFrame) selectedMode = UfoWashToolMode.PreRinse;
                if (Keyboard.current.digit2Key.wasPressedThisFrame) selectedMode = UfoWashToolMode.Foam;
                if (Keyboard.current.digit3Key.wasPressedThisFrame) selectedMode = UfoWashToolMode.FinalRinse;
            }

            spraying = Mouse.current != null &&
                Mouse.current.leftButton.isPressed &&
                Cursor.lockState == CursorLockMode.Locked;

            var career = UfoWashCareerLoop.Instance;
            bool canWash = career == null || career.JobAccepted;
            bool active = spraying && canWash && !UfoWashHUD.JobComplete && viewCamera != null;

            if (sprayParticles != null)
            {
                if (active && !sprayParticles.isPlaying) sprayParticles.Play();
                else if (!active && sprayParticles.isPlaying) sprayParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (sprayParticles != null)
            {
                var main = sprayParticles.main;
                main.startColor = selectedMode == UfoWashToolMode.Foam
                    ? new Color(0.88f, 0.97f, 1f, 0.9f)
                    : new Color(0.38f, 0.84f, 1f, 0.84f);
            }

            if (!active) return;

            // A small sphere cast gives the spray a usable footprint instead of
            // requiring pixel-perfect aiming at one triangle.
            RaycastHit[] hits = Physics.SphereCastAll(viewCamera.transform.position,
                sprayRadius, viewCamera.transform.forward, range);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (hit.collider == null || hit.collider.transform == transform ||
                    hit.collider.transform.IsChildOf(transform)) continue;

                var dirt = hit.collider.GetComponentInParent<UfoWashDirtSurface>();
                if (dirt == null && hit.collider.GetComponentInParent<UfoWashCustomerShip>() != null)
                    continue; // Decorative ship hardware must not block the washable hull behind it.

                if (dirt != null)
                {
                    var loop = UfoWashCareerLoop.Instance;
                    if (loop != null) loop.ApplyTool(dirt, selectedMode, Time.deltaTime);
                    else dirt.Process(selectedMode, Time.deltaTime);
                }

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
                break;
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
    }
}