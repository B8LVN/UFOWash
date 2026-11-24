using UnityEngine;

namespace UFOWASH
{
    public sealed class UfoWashCustomerShip : MonoBehaviour
    {
        enum DockState { Arriving, Docked, Departing, Turnaround }
        [SerializeField] float travelDistance = 10.5f;
        [SerializeField] float travelSpeed = 6f;
        [SerializeField] float turnaroundSeconds = 1.6f;

        Vector3 dockPosition;
        Vector3 approachPosition;
        float turnaroundTimer;
        DockState state;
        UfoWashDirtSurface[] serviceSurfaces;

        public bool IsDocked { get; private set; }
        public UfoWashDirtSurface[] ServiceSurfaces => serviceSurfaces;

        void Awake()
        {
            dockPosition = transform.position;
            serviceSurfaces = GetComponentsInChildren<UfoWashDirtSurface>(true);
            approachPosition = dockPosition + Vector3.forward * travelDistance;
            transform.position = approachPosition;
            IsDocked = false;
            state = DockState.Arriving;
        }

        void Update()
        {
            if (state == DockState.Turnaround)
            {
                turnaroundTimer -= Time.deltaTime;
                if (turnaroundTimer <= 0f) state = DockState.Arriving;
                return;
            }

            Vector3 target = state == DockState.Departing ? approachPosition : dockPosition;
            transform.position = Vector3.MoveTowards(transform.position, target, travelSpeed * Time.deltaTime);

            if ((transform.position - target).sqrMagnitude > 0.0025f) return;

            if (state == DockState.Arriving)
            {
                IsDocked = true;
                state = DockState.Docked;
            }
            else if (state == DockState.Departing)
            {
                IsDocked = false;
                state = DockState.Turnaround;
                turnaroundTimer = turnaroundSeconds;
            }
        }

        public void DepartAndReturn()
        {
            if (!IsDocked || state != DockState.Docked) return;
            IsDocked = false;
            state = DockState.Departing;
        }
    }
}
