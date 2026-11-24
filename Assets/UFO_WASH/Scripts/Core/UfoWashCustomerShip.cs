using UnityEngine;

namespace UFOWASH
{
    public sealed class UfoWashCustomerShip : MonoBehaviour
    {
        enum DockState { Arriving, Docked, Departing, Turnaround }
        [SerializeField] float travelDistance = 10.5f;
        [SerializeField] float travelSpeed = 6f;
        [SerializeField] float turnaroundSeconds = 1.6f;
        [SerializeField] float arrivalLift = .7f;

        Vector3 dockPosition;
        Vector3 approachPosition;
        Vector3 travelStart;
        Vector3 travelTarget;
        Quaternion dockRotation;
        Quaternion approachRotation;
        Quaternion travelStartRotation;
        Quaternion travelTargetRotation;
        float turnaroundTimer;
        float travelElapsed;
        float travelDuration;
        DockState state;
        UfoWashDirtSurface[] serviceSurfaces;
        Light dockingLight;

        public bool IsDocked { get; private set; }
        public UfoWashDirtSurface[] ServiceSurfaces => serviceSurfaces;

        void Awake()
        {
            dockPosition = transform.position;
            dockRotation = transform.rotation;
            serviceSurfaces = GetComponentsInChildren<UfoWashDirtSurface>(true);
            approachPosition = dockPosition + Vector3.forward * travelDistance;
            approachRotation = dockRotation * Quaternion.Euler(0f, -12f, 0f);
            transform.position = approachPosition;
            transform.rotation = approachRotation;
            IsDocked = false;
            CreateDockingLight();
            BeginTravel(DockState.Arriving);
        }

        void Update()
        {
            if (state == DockState.Turnaround)
            {
                turnaroundTimer -= Time.deltaTime;
                if (dockingLight != null)
                    dockingLight.intensity = .12f + Mathf.PingPong(Time.time * .35f, .12f);
                if (turnaroundTimer <= 0f) BeginTravel(DockState.Arriving);
                return;
            }

            if (state == DockState.Docked)
            {
                if (dockingLight != null)
                    dockingLight.intensity = .35f + Mathf.PingPong(Time.time * .22f, .12f);
                return;
            }

            travelElapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(travelElapsed / travelDuration);
            float eased = progress * progress * (3f - 2f * progress);
            Vector3 position = Vector3.Lerp(travelStart, travelTarget, eased);
            position.y += Mathf.Sin(progress * Mathf.PI) * arrivalLift;
            transform.position = position;

            float bank = Mathf.Sin(progress * Mathf.PI) * (state == DockState.Arriving ? 4f : -4f);
            transform.rotation = Quaternion.Slerp(travelStartRotation, travelTargetRotation, eased) *
                Quaternion.Euler(0f, 0f, bank);
            if (dockingLight != null)
            {
                dockingLight.color = state == DockState.Arriving
                    ? new Color(.24f,.82f,1f)
                    : new Color(1f,.52f,.25f);
                dockingLight.intensity = .22f + Mathf.Sin(progress * Mathf.PI) * .85f;
            }

            if (progress < 1f) return;
            if (state == DockState.Arriving)
            {
                transform.SetPositionAndRotation(dockPosition, dockRotation);
                IsDocked = true;
                state = DockState.Docked;
            }
            else if (state == DockState.Departing)
            {
                transform.SetPositionAndRotation(approachPosition, approachRotation);
                IsDocked = false;
                state = DockState.Turnaround;
                turnaroundTimer = turnaroundSeconds;
            }
        }

        void BeginTravel(DockState travelState)
        {
            state = travelState;
            travelElapsed = 0f;
            travelStart = transform.position;
            travelTarget = travelState == DockState.Departing ? approachPosition : dockPosition;
            travelStartRotation = transform.rotation;
            travelTargetRotation = travelState == DockState.Departing ? approachRotation : dockRotation;
            travelDuration = Mathf.Max(
                Vector3.Distance(travelStart, travelTarget) / Mathf.Max(.1f, travelSpeed), 1.4f);
        }

        void CreateDockingLight()
        {
            var beacon = new GameObject("DOCKING BEACON // APPROACH SIGNAL");
            beacon.transform.SetParent(transform, false);
            beacon.transform.localPosition = new Vector3(0f,-.55f,0f);
            dockingLight = beacon.AddComponent<Light>();
            dockingLight.type = LightType.Point;
            dockingLight.color = new Color(.24f,.82f,1f);
            dockingLight.intensity = .22f;
            dockingLight.range = 4.5f;
            dockingLight.shadows = LightShadows.None;
        }

        public void DepartAndReturn()
        {
            if (!IsDocked || state != DockState.Docked) return;
            IsDocked = false;
            BeginTravel(DockState.Departing);
        }
    }
}
