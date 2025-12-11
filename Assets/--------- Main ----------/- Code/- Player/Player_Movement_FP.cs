using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

namespace Henmova.Vaibhav
{
    public class Player_Movement_FP : MonoBehaviour
    {
        [Header("Animators")]
        [SerializeField] private Animator _cameraAnimator;
        [Header("Movement Settings")]
        public float walkSpeed = 5f;
        public float sprintSpeed = 8f;
        public float crouchSpeed = 2.5f;
        public float jumpHeight = 1.5f;
        public float gravity = -9.81f;
        private bool isJumped = false;

        [Header("Mouse Look Settings")]
        public Transform cameraTransform;
        public float mouseSensitivity = 100f;
        public float maxLookUpAngle = 80f;
        public float maxLookDownAngle = -80f;

        [Header("Ground Check")]
        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;
        [Header("Ground Ray")]
        [SerializeField] private Transform _groundRayCheckPoint;
        [SerializeField] private float _maxGroundAnimImpactValue = 7f;
        [SerializeField] private LayerMask _groundRayMask;
        private float _oldHightY, _newHightY;

        [Header("Crouch Settings")]
        public float crouchHeight = 1f;
        public float standingHeight = 2f;
        public float crouchTransitionSpeed = 6f;

        [Space(5)]
        public float cam_CrouchHeight = 1f, cam_StandingHeight = 1.6f;

        [Header("Sprint Settings")]
        public KeyCode sprintKey = KeyCode.LeftShift;

        [Header("Cinemachine FOV Settings")]
        public CinemachineCamera virtualCamera;
        public float walkFOV = 60f;
        public float sprintFOV = 75f;
        public float fovTransitionSpeed = 4f;

        private CharacterController controller;
        private Vector3 velocity;
        private bool isGrounded;
        private float xRotation = 0f;
        private bool isCrouching = false;
        private bool isSprinting = false;

        private float targetHeight;
        [Title("Components")]
        private Player_Referrences _referrences;
        [Title("Runtime Data")]
        private bool _isTalking = false;
        [GUIColor("yellow")]
        [SerializeField] private Transform _StartPoint, _RestartPoint;


        private bool _isDead = false;


        #region ------ MONO ------

        void Awake()
        {
            _referrences = GetComponent<Player_Referrences>();

            // subscriptions
            Game_Manager.OnPlayerRestart += Handle_PlayerRestart;
        }

        void OnDisable()
        {
            // Unsubscribe from events
            Game_Manager.OnPlayerRestart -= Handle_PlayerRestart;
        }
        void Start()
        {
            // cameraRotation = Quaternion.Euler(euler);
            // cameraPosition = virtualCamera.transform.localPosition;

            controller = GetComponent<CharacterController>();
            targetHeight = standingHeight;
            controller.height = targetHeight;
            Cursor.lockState = CursorLockMode.Locked;

            if (virtualCamera != null)
            {
                virtualCamera.Lens.FieldOfView = walkFOV;
            }
        }

        void Update()
        {
            HandleMouseLook();
            HandleCrouch();
            HandleMovement();
            HandleFOV();

        }
        #endregion

        void HandleMouseLook()
        {
            if (_isTalking) return;
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, maxLookDownAngle, maxLookUpAngle);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        void HandleMovement()
        {
            if (_isDead)
            {
                _cameraAnimator.SetFloat(KeyValueNames.ANIM_PLAYER_CAM_MOVEMENT, 0f);
                return;
            }
            isGrounded = controller.isGrounded;

            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;

            isSprinting = Input.GetKey(sprintKey) && !isCrouching && moveZ > 0;

            float speed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : walkSpeed);
            if (!_isTalking && move.magnitude > 0.1f)
            {
                _cameraAnimator.SetFloat(KeyValueNames.ANIM_PLAYER_CAM_MOVEMENT, isCrouching ? .5f : (isSprinting ? 1f : .5f));
                controller.Move(move.normalized * speed * Time.deltaTime);
            }
            else if (move.magnitude == 0f)
            {
                _cameraAnimator.SetFloat(KeyValueNames.ANIM_PLAYER_CAM_MOVEMENT, 0f);
            }

            if (!isGrounded)
            {
                isJumped = true;
                float newPoint = transform.position.y;
                if (newPoint > _newHightY) _newHightY = newPoint;
            }
            else
            {
                _oldHightY = transform.position.y;
                float heightValue = (_newHightY - _oldHightY) / _maxGroundAnimImpactValue;
                float jumpImpactMultiplier = Mathf.Lerp(.2f, 1f, heightValue);
                _cameraAnimator.SetFloat(KeyValueNames.ANIM_PLAYER_CAM_JUMP_DOWN_IMPACT, jumpImpactMultiplier);
            }

            if (isJumped && isGrounded)
            {
                _cameraAnimator.SetBool(KeyValueNames.ANIM_PLAYER_CAM_JUMP_DOWN, true);
                _cameraAnimator.SetBool(KeyValueNames.ANIM_PLAYER_CAM_JUMP_UP, false);
                isJumped = false;
            }
            else if (!isJumped)
            {
                _cameraAnimator.SetBool(KeyValueNames.ANIM_PLAYER_CAM_JUMP_DOWN, false);
            }

            if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            {
                // isJumped = true;
                _cameraAnimator.SetBool(KeyValueNames.ANIM_PLAYER_CAM_JUMP_DOWN, false);
                _cameraAnimator.SetBool(KeyValueNames.ANIM_PLAYER_CAM_JUMP_UP, true);
                velocity.y = Mathf.Sqrt((isSprinting ? jumpHeight * 1.3f : jumpHeight) * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, -20f, 20f);
            // if (isGrounded) { }
            // else
            // {
            //     if (velocity.y <= -20f)
            //     {
            //         velocity.y = -20f;
            //         Debug.Log("Limiting velocity");
            //     }
            // }
            controller.Move(velocity * Time.deltaTime);
        }

        void HandleCrouch()
        {
            if (_isTalking) return;
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouching = !isCrouching;
                targetHeight = isCrouching ? crouchHeight : standingHeight;
            }
            // else if (Input.GetKeyUp(KeyCode.LeftControl))
            // {
            //     isCrouching = false;
            //     targetHeight = standingHeight;
            // }

            float previousHeight = controller.height;
            controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
            // Adjust center to keep feet on ground
            float heightDifference = controller.height - previousHeight;
            controller.center += new Vector3(0, heightDifference / 2f, 0);

            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = isCrouching ? cam_CrouchHeight : cam_StandingHeight;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, camPos, Time.deltaTime * crouchTransitionSpeed);
        }



        void HandleFOV()
        {
            if (virtualCamera == null) return;

            float targetFOV = isSprinting ? sprintFOV : walkFOV;
            virtualCamera.Lens.FieldOfView = Mathf.Lerp(
                virtualCamera.Lens.FieldOfView,
                targetFOV,
                Time.deltaTime * fovTransitionSpeed
            );
        }
        #region ------- LISTENERS -------
        // On Talk
        private void Handle_PlayerTalk(bool isTalking, Transform target)
        {
            _isTalking = isTalking;
            if (isTalking)
            {
                // Handle talking logic here
                // Debug.Log($"Talking to: {target.name}");
                Vector3 targetRot = new Vector3(target.position.x, transform.position.y, target.position.z);
                transform.DOLookAt(targetRot, 0.6f).SetEase(Ease.OutExpo);
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                // Handle stopping talking logic here
                // Debug.Log("Stopped talking");
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void Handle_PlayerRestart()
        {
            _isDead = true;
            // controller.enabled = false;
            // transform.position = _RestartPoint.position;
            // transform.rotation = Quaternion.Euler(0f, _RestartPoint.eulerAngles.y, 0f);
            // controller.enabled = true;
        }
        #endregion
    }
}
