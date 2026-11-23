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
        void Update()
        {
            spraying = Mouse.current != null && Mouse.current.leftButton.isPressed && Cursor.lockState == CursorLockMode.Locked;
            if (!spraying || viewCamera == null) return;
            if (Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward, out RaycastHit hit, range))
            {
                var dirt = hit.collider.GetComponentInParent<UfoWashDirtSurface>();
                if (dirt != null) dirt.Clean(cleanPerSecond * Time.deltaTime);
            }
        }
    }
}