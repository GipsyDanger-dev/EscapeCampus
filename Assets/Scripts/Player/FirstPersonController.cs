using UnityEngine;

namespace EscapeCampus.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float sprintSpeed = 8f;
        [SerializeField] private float crouchSpeed = 2.5f;

        [Header("Look")]
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float lookXClamp = 85f;

        [Header("Crouch")]
        [SerializeField] private float standingHeight = 2f;
        [SerializeField] private float crouchHeight = 1.2f;
        [SerializeField] private float crouchTransitionSpeed = 8f;

        [Header("Ground Check")]
        [SerializeField] private float gravity = -19.62f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.3f;
        [SerializeField] private LayerMask groundMask;

        private CharacterController controller;
        private Camera playerCamera;
        private Vector3 velocity;
        private float xRotation;
        private float currentSpeed;
        private float targetHeight;
        private bool isGrounded;
        private bool isSprinting;
        private bool isCrouching;

        public bool IsGrounded => isGrounded;
        public bool IsSprinting => isSprinting;
        public bool IsCrouching => isCrouching;
        public float CurrentSpeed => currentSpeed;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            playerCamera = GetComponentInChildren<Camera>();

            if (playerCamera == null)
            {
                GameObject camObj = new GameObject("PlayerCamera");
                camObj.transform.SetParent(transform);
                camObj.transform.localPosition = new Vector3(0f, 0.8f, 0f);
                playerCamera = camObj.AddComponent<Camera>();
                camObj.AddComponent<AudioListener>();
            }

            targetHeight = standingHeight;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleGroundCheck();
            HandleMovement();
            HandleLook();
            HandleCrouch();
            HandleGravity();
        }

        private void HandleGroundCheck()
        {
            if (groundCheck != null)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            }
            else
            {
                isGrounded = controller.isGrounded;
            }
        }

        private void HandleMovement()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            isSprinting = Input.GetKey(KeyCode.LeftShift) && !isCrouching && moveZ > 0;

            if (isCrouching)
                currentSpeed = crouchSpeed;
            else if (isSprinting)
                currentSpeed = sprintSpeed;
            else
                currentSpeed = walkSpeed;

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            move = Vector3.ClampMagnitude(move, 1f);

            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        private void HandleLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -lookXClamp, lookXClamp);

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleCrouch()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouching = true;
                targetHeight = crouchHeight;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                if (!Physics.Raycast(transform.position, Vector3.up, standingHeight - crouchHeight + 0.1f))
                {
                    isCrouching = false;
                    targetHeight = standingHeight;
                }
            }

            float currentHeight = controller.height;
            if (!Mathf.Approximately(currentHeight, targetHeight))
            {
                float newHeight = Mathf.Lerp(currentHeight, targetHeight, crouchTransitionSpeed * Time.deltaTime);
                float heightDiff = newHeight - currentHeight;
                controller.height = newHeight;
                controller.center = new Vector3(0f, newHeight / 2f, 0f);
                transform.position += new Vector3(0f, heightDiff / 2f, 0f);
            }
        }

        private void HandleGravity()
        {
            if (isGrounded && velocity.y < 0f)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        public void SetSensitivity(float sensitivity)
        {
            mouseSensitivity = sensitivity;
        }
    }
}
