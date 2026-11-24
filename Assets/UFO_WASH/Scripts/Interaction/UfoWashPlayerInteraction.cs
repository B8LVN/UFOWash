using UnityEngine;
using UnityEngine.InputSystem;

namespace UFOWASH
{
    public class UfoWashPlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Camera viewCamera;
        [SerializeField] private float interactionRange = 3.2f;
        private UfoWashInteractable currentTarget;
        UfoWashFirstPersonController controller;
        readonly RaycastHit[] interactionHits = new RaycastHit[48];

        private void Awake()
        {
            if (viewCamera == null) viewCamera = GetComponentInChildren<Camera>();
            controller = GetComponent<UfoWashFirstPersonController>();
        }

        private void Update()
        {
            if (UfoWashContractTerminalUI.IsOpen || controller == null || !controller.CanControl ||
                controller.IsCleaningView) { currentTarget = null; return; }
            currentTarget = null;
            if (viewCamera == null) return;

            // Interaction distance belongs to the character, not the camera four
            // metres behind their shoulder. Keep occlusion from the visible reticle.
            int count = Physics.RaycastNonAlloc(viewCamera.transform.position, viewCamera.transform.forward,
                interactionHits, interactionRange + 5f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            if (count == interactionHits.Length) return;
            float nearest = float.PositiveInfinity;
            RaycastHit targetHit = default;
            for (int i = 0; i < count; i++)
            {
                var hit = interactionHits[i];
                if (hit.collider.transform == transform || hit.collider.transform.IsChildOf(transform) || hit.distance >= nearest) continue;
                nearest = hit.distance;
                targetHit = hit;
            }
            if (targetHit.collider != null && Vector3.Distance(transform.position + Vector3.up, targetHit.point) <= interactionRange)
                currentTarget = targetHit.collider.GetComponentInParent<UfoWashInteractable>();

            if (currentTarget != null && Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                currentTarget.Interact(gameObject);
            }
        }

        private void OnGUI()
        {
            if (currentTarget == null) return;
            GUI.color = Color.white;
            var style = new GUIStyle(GUI.skin.box)
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                richText = true
            };
            GUI.Box(new Rect(Screen.width * 0.5f - 310, Screen.height * 0.72f, 620, 48),
                currentTarget.GetPrompt(), style);
        }
    }

    public class UfoWashInteractable : MonoBehaviour
    {
        [SerializeField] private string prompt = "E  INTERACT";
        public virtual string GetPrompt() => prompt;
        public virtual void Interact(GameObject player) { }
    }
}
