using UnityEngine;
using UnityEngine.InputSystem;

namespace UFOWASH
{
    [RequireComponent(typeof(CharacterController))]
    public class UfoWashFirstPersonController : MonoBehaviour
    {
        [SerializeField] Transform cameraPivot;
        [SerializeField] Camera playerCamera;
        [SerializeField] float moveSpeed = 5.5f;
        [SerializeField] float mouseSensitivity = 0.12f;
        [SerializeField] float gravity = -20f;
        [SerializeField] float explorationCameraDistance = 3.1f;
        CharacterController controller;
        Renderer[] bodyRenderers;
        Renderer[] viewModelRenderers;
        UfoWashSprayTool sprayTool;
        float pitch, verticalVelocity;
        bool inputSuspended;
        bool isCleaningView;
        bool discardMouseDelta;

        public bool IsCleaningView => isCleaningView;

        public void SetInputSuspended(bool suspended)
        {
            inputSuspended = suspended;
            Cursor.lockState = suspended ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = suspended;
            if (suspended) ApplyView(false);
            else discardMouseDelta = true;
        }

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            sprayTool = GetComponent<UfoWashSprayTool>();
            if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
            var renderers = GetComponentsInChildren<Renderer>(true);
            var body = new System.Collections.Generic.List<Renderer>();
            var tool = new System.Collections.Generic.List<Renderer>();
            foreach (var renderer in renderers)
            {
                if (renderer.GetComponentInParent<Camera>() != null) tool.Add(renderer);
                else body.Add(renderer);
            }
            bodyRenderers = body.ToArray();
            viewModelRenderers = tool.ToArray();
            if (cameraPivot != null)
            {
                pitch = cameraPivot.localEulerAngles.x;
                if (pitch > 180f) pitch -= 360f;
            }
            ApplyView(false);
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (inputSuspended || !isActiveAndEnabled || !gameObject.activeInHierarchy ||
                controller == null || !controller.enabled || cameraPivot == null ||
                Keyboard.current == null || Mouse.current == null) return;

            bool wantsCleaningView = IsActivelyAimingAtWashableShip();
            if (wantsCleaningView != isCleaningView) ApplyView(wantsCleaningView);

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (Cursor.lockState != CursorLockMode.Locked) return;

            Vector2 mouse = discardMouseDelta ? Vector2.zero : Mouse.current.delta.ReadValue() * mouseSensitivity;
            discardMouseDelta = false;
            pitch = Mathf.Clamp(pitch - mouse.y, -72f, 72f);
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            transform.Rotate(Vector3.up * mouse.x);

            Vector2 move = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) move.y += 1;
            if (Keyboard.current.sKey.isPressed) move.y -= 1;
            if (Keyboard.current.dKey.isPressed) move.x += 1;
            if (Keyboard.current.aKey.isPressed) move.x -= 1;
            Vector3 motion = (transform.forward * move.y + transform.right * move.x).normalized * moveSpeed;
            if (controller.isGrounded && verticalVelocity < 0) verticalVelocity = -2f;
            verticalVelocity += gravity * Time.deltaTime;
            motion.y = verticalVelocity;
            controller.Move(motion * Time.deltaTime);
        }

        bool IsActivelyAimingAtWashableShip()
        {
            if (Mouse.current == null || !Mouse.current.leftButton.isPressed ||
                Cursor.lockState != CursorLockMode.Locked || playerCamera == null) return false;
            var career = UfoWashCareerLoop.Instance;
            if (career == null || !career.JobAccepted) return false;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                out var hit, 14f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                return hit.collider.GetComponentInParent<UfoWashDirtSurface>() != null;
            return false;
        }

        void ApplyView(bool cleaning)
        {
            isCleaningView = cleaning;
            if (playerCamera != null)
            {
                Transform t = playerCamera.transform;
                t.localPosition = cleaning ? Vector3.zero : new Vector3(0f, 0.12f, -explorationCameraDistance);
                t.localRotation = Quaternion.identity;
                playerCamera.fieldOfView = cleaning ? 72f : 66f;
            }
            SetRenderers(bodyRenderers, !cleaning);
            SetRenderers(viewModelRenderers, cleaning);
        }

        static void SetRenderers(Renderer[] renderers, bool visible)
        {
            if (renderers == null) return;
            foreach (var renderer in renderers)
                if (renderer != null) renderer.enabled = visible;
        }
    }
}




