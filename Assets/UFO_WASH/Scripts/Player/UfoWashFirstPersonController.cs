using UnityEngine;
using UnityEngine.InputSystem;

namespace UFOWASH
{
    [RequireComponent(typeof(CharacterController))]
    public class UfoWashFirstPersonController : MonoBehaviour
    {
        [SerializeField] Transform cameraPivot;
        [SerializeField] float moveSpeed = 5.5f;
        [SerializeField] float mouseSensitivity = 0.12f;
        [SerializeField] float gravity = -20f;
        CharacterController controller;
        float pitch, verticalVelocity;

        void Awake() { controller = GetComponent<CharacterController>(); }
        void Start() { if (cameraPivot != null) { pitch = cameraPivot.localEulerAngles.x; if (pitch > 180f) pitch -= 360f; } Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        void Update()
        {
            if (Keyboard.current == null || Mouse.current == null) return;
            if (Keyboard.current.escapeKey.wasPressedThisFrame) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
            if (Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
            if (Cursor.lockState != CursorLockMode.Locked) return;
            Vector2 mouse = Mouse.current.delta.ReadValue() * mouseSensitivity;
            pitch = Mathf.Clamp(pitch - mouse.y, -85f, 85f);
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
    }
}