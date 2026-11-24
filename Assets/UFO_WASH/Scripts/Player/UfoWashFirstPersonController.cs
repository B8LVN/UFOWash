using UnityEngine;
using UnityEngine.InputSystem;

namespace UFOWASH
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(CharacterController))]
    public class UfoWashFirstPersonController : MonoBehaviour
    {
        [SerializeField] Transform cameraPivot;
        [SerializeField] Camera playerCamera;
        [SerializeField] float moveSpeed = 5.5f;
        [SerializeField] float mouseSensitivity = 0.12f;
        [SerializeField] float gravity = -20f;
        [SerializeField] float explorationCameraDistance = 4.2f;
        [SerializeField] float explorationCameraShoulder = 1.05f;
        [SerializeField] float explorationCameraHeight = 0.55f;
        [SerializeField] float explorationCameraPitch = 5f;
        CharacterController controller;
        Renderer[] bodyRenderers;
        Renderer[] viewModelRenderers;
        UfoWashSprayTool sprayTool;
        float pitch, verticalVelocity;
        bool inputSuspended;
        bool isCleaningView;
        bool discardMouseDelta;
        bool hasFocus = true;
        bool swallowHeldClick;

        public bool IsCleaningView => isCleaningView;
        public bool CanControl => enabled && !inputSuspended && hasFocus && !swallowHeldClick &&
            Cursor.lockState == CursorLockMode.Locked && Time.timeScale > 0f;

        public void SetCharacterVisible(bool visible) => SetRenderers(bodyRenderers, visible && !isCleaningView);

        public void SetInputSuspended(bool suspended)
        {
            inputSuspended = suspended;
            Cursor.lockState = suspended ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = suspended;
            if (suspended) ApplyView(false);
            else { discardMouseDelta = true; swallowHeldClick = true; }
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
            discardMouseDelta = true;
            swallowHeldClick = true;
        }

        void Update()
        {
            if (inputSuspended || !isActiveAndEnabled || !gameObject.activeInHierarchy ||
                controller == null || !controller.enabled || cameraPivot == null ||
                Keyboard.current == null || Mouse.current == null) return;

            if (!hasFocus || Time.timeScale <= 0f) { ApplyView(false); return; }
            if (!Mouse.current.leftButton.isPressed) swallowHeldClick = false;

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ApplyView(false);
                discardMouseDelta = true;
                return;
            }
            if (Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                discardMouseDelta = true;
                swallowHeldClick = true;
                return;
            }
            if (Cursor.lockState != CursorLockMode.Locked) { ApplyView(false); return; }

            bool wantsCleaningView = IsActivelyAimingAtWashableShip(out var aimPoint);
            if (wantsCleaningView != isCleaningView)
            {
                ApplyView(wantsCleaningView);
                if (wantsCleaningView)
                {
                    // Keep the same hull point under the reticle across the
                    // shoulder-to-eye translation, rather than toggling every frame.
                    Vector3 direction = aimPoint - playerCamera.transform.position;
                    Vector3 angles = Quaternion.LookRotation(direction).eulerAngles;
                    transform.rotation = Quaternion.Euler(0f, angles.y, 0f);
                    pitch = Mathf.DeltaAngle(0f, angles.x);
                    cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
                }
            }

            Vector2 mouse = discardMouseDelta ? Vector2.zero : Mouse.current.delta.ReadValue() * mouseSensitivity;
            discardMouseDelta = false;
            float pitchLimit = isCleaningView ? 72f : 38f;
            pitch = Mathf.Clamp(pitch - mouse.y, -pitchLimit, pitchLimit);
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
            ResolveExplorationCameraCollision();
        }

        bool IsActivelyAimingAtWashableShip(out Vector3 aimPoint)
        {
            aimPoint = Vector3.zero;
            if (Mouse.current == null || !Mouse.current.leftButton.isPressed ||
                !CanControl || playerCamera == null || sprayTool == null) return false;
            var career = UfoWashCareerLoop.Instance;
            if (career == null || !career.CanUseTool(sprayTool.selectedMode) ||
                !sprayTool.TryGetWashTarget(out var hit, out _)) return false;
            aimPoint = hit.point;
            return true;
        }

        void OnApplicationFocus(bool focused)
        {
            hasFocus = focused;
            discardMouseDelta = true;
            swallowHeldClick = true;
            if (!focused) { ApplyView(false); Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
        }

        void OnDisable() { ApplyView(false); }

        void ApplyView(bool cleaning)
        {
            isCleaningView = cleaning;
            if (playerCamera != null)
            {
                Transform t = playerCamera.transform;
                t.localPosition = cleaning ? new Vector3(0f, 0.08f, 0.42f) :
                    new Vector3(explorationCameraShoulder, explorationCameraHeight, -explorationCameraDistance);
                t.localRotation = cleaning ? Quaternion.identity : Quaternion.Euler(explorationCameraPitch, 0f, 0f);
                playerCamera.fieldOfView = cleaning ? 72f : 66f;
            }
            SetRenderers(bodyRenderers, !cleaning);
            SetRenderers(viewModelRenderers, cleaning);
            if (!cleaning) ResolveExplorationCameraCollision();
        }

        void ResolveExplorationCameraCollision()
        {
            if (isCleaningView || playerCamera == null || cameraPivot == null) return;

            Vector3 desiredOffset = new Vector3(explorationCameraShoulder, explorationCameraHeight, -explorationCameraDistance);
            Vector3 worldOffset = cameraPivot.TransformVector(desiredOffset);
            float distance = worldOffset.magnitude;
            if (distance <= 0.01f) return;

            Vector3 direction = worldOffset / distance;
            var hits = Physics.SphereCastAll(cameraPivot.position, 0.2f, direction, distance,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            for (int i = 0; i < hits.Length; i++)
            {
                Collider collider = hits[i].collider;
                if (collider == null || collider.transform == transform || collider.transform.IsChildOf(transform))
                    continue;

                float safeDistance = Mathf.Max(0.25f, hits[i].distance - 0.12f);
                playerCamera.transform.position = cameraPivot.position + direction * safeDistance;
                return;
            }

            playerCamera.transform.localPosition = desiredOffset;
        }

        static void SetRenderers(Renderer[] renderers, bool visible)
        {
            if (renderers == null) return;
            foreach (var renderer in renderers)
                if (renderer != null) renderer.enabled = visible;
        }
    }
}







