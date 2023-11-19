using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public Transform collectPoint;

    [SerializeField] Rig aimRig;


    public float moveSpeed = 2.0f;
    public float sprintSpeed = 5.335f;
    public float rotationSmoothTime = 0.12f;
    public float speedChangeRate = 10.0f;
    public float sensitivity = 1f;

    public float jumpHeight = 1.2f;
    public float gravity = -15.0f;
    public float jumpTimeout = 0.50f;
    public float fallTimeout = 0.15f;

    [HideInInspector]
    public bool isGrounded = true;
    float groundedOffset = -0.14f;
    float groundedRadius = 0.28f;
    public LayerMask groundLayers;

    public GameObject cameraTarget;


    public bool lockCameraPosition = false;

    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private float speed;
    private float animationBlend;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;

    private int animIDAim;

    [SerializeField] private Animator animator;
    private CharacterController characterController;
    [SerializeField] PlayerInputHandler playerInput;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] bool rotateOnMove = true;

    private const float threshold = 0.01f;

    private bool hasAnimator = true;


    private Vector2 movement;


    Vector3 direction;
    [SerializeField]
    Transform[] hitWeapons;

    Transform _weapon;



    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {

        characterController = GetComponent<CharacterController>();

        AssignAnimationIDs();

        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }


    private void ShooterController()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (playerInput.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            SetSensitivity(aimSensitivity);
            SetRotateOnMove(false);
            // animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0.5f, Time.deltaTime * 13f));
            animator.SetBool(animIDAim, true);

            aimRig.weight = Mathf.Lerp(aimRig.weight, 1f, Time.deltaTime * 13f);
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            if (playerInput.shoot)
            {

                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

                playerInput.shoot = false;
            }
        }

        else
        {
            animator.SetBool(animIDAim, false);

            aimVirtualCamera.gameObject.SetActive(false);
            SetSensitivity(normalSensitivity);
            SetRotateOnMove(true);
            //  animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 13f));
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0f, Time.deltaTime * 13f);

            playerInput.shoot = false;
        }




    }


    private void Update()
    {


        JumpAndGravity();
        GroundedCheck();
        if (!playerInput.buildMode)
        {
            ShooterController();


        }
        Move();


    }


    private void LateUpdate()
    {
        CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        animIDAim = Animator.StringToHash("Aim");
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

        if (hasAnimator)
        {
            animator.SetBool(animIDGrounded, isGrounded);
        }
    }

    // Build Mode Player Controllers

    private void OnMove(InputValue value)
    {
        if (!playerInput.buildMode)
            return;

        movement = value.Get<Vector2>();

        direction = new Vector3(movement.x, 0f, movement.y);

        if (movement.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

    }



    public void OnAttackStart()
    {
        moveSpeed = 0f;
    }

    public void OnAttackRelease()
    {
        moveSpeed = 2;
    }







    public void PerformPunch(Enums.Weapons hitWeapon)
    {
        _weapon = hitWeapons[(int)hitWeapon];

        Collider[] hitDamagables = Physics.OverlapSphere(_weapon.position, 0.5f);

        foreach (Collider hit in hitDamagables)
        {
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(2);
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect(collectPoint);
        }
    }


    //




    private void CameraRotation()
    {
        if (playerInput.buildMode)
            return;

        if (playerInput.look.sqrMagnitude >= threshold && !lockCameraPosition)
        {
            cinemachineTargetYaw += playerInput.look.x * Time.deltaTime * sensitivity;
            cinemachineTargetPitch += playerInput.look.y * Time.deltaTime * sensitivity;
        }

        cameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0.0f);
    }

    private void Move()
    {
        float targetSpeed = playerInput.sprint ? sprintSpeed : moveSpeed;

        if (playerInput.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = playerInput.analogMovement ? playerInput.move.magnitude : 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }
        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);

        Vector3 inputDirection = new Vector3(playerInput.move.x, 0.0f, playerInput.move.y).normalized;

        if (playerInput.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

            if (rotateOnMove && !playerInput.buildMode)
            {
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            }
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        if (!playerInput.buildMode)
        {
            characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        }
        else
        {
            characterController.Move(direction * moveSpeed * Time.deltaTime);

        }


        if (hasAnimator)
        {
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, inputMagnitude);
        }
    }

    private void JumpAndGravity()
    {
        if (isGrounded)
        {
            fallTimeoutDelta = fallTimeout;

            if (hasAnimator)
            {
                animator.SetBool(animIDJump, false);
                animator.SetBool(animIDFreeFall, false);
            }

            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if (playerInput.jump && jumpTimeoutDelta <= 0.0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                if (hasAnimator)
                {
                    animator.SetBool(animIDJump, true);
                }
            }

            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;

            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (hasAnimator)
                {
                    animator.SetBool(animIDFreeFall, true);
                }
            }

            playerInput.jump = false;
        }

        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }



    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }

    public void SetRotateOnMove(bool newRotateOnMove)
    {
        rotateOnMove = newRotateOnMove;
    }
}
