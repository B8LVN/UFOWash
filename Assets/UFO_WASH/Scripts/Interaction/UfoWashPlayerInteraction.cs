using UnityEngine;
using UnityEngine.InputSystem;

namespace UFOWASH
{
    public class UfoWashPlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Camera viewCamera;
        [SerializeField] private float interactionRange = 3.2f;
        private UfoWashInteractable currentTarget;

        private void Awake()
        {
            if (viewCamera == null) viewCamera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            currentTarget = null;
            if (viewCamera == null) return;

            if (Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward,
                out RaycastHit hit, interactionRange))
            {
                currentTarget = hit.collider.GetComponentInParent<UfoWashInteractable>();
            }

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
            GUI.Box(new Rect(Screen.width * 0.5f - 150, Screen.height * 0.72f, 300, 42),
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
