using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    private const float GRAVITY = 9.81f;
    
    private CharacterController characterController;

    private Animator animator;
    
    [Header("Player Movement")] 
    
    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private float sprint = 3f;

    private bool stopped;

    private Vector2 movement;

    [Header("Player Camera")] 
    
    [SerializeField]
    private Transform camera;

    [Header("Player Jumping and Velocity")] 
    
    [SerializeField]
    private float turnCalmTime = .1f;
    
    private float turnCalmVelocity;

    [SerializeField] 
    private float jumpRange = 1f;

    private Vector3 velocity;
    
    [SerializeField]
    private Transform surfaceCheck;

    [SerializeField] 
    private float surfaceDistance = .4f;
    
    [SerializeField] 
    private LayerMask surfaceMask;
    
    [Header("Player Jumping and Velocity")] 
    
    [SerializeField]
    private float dampTime = .1f;

    private int velocityHash;
    private int velocityXHash;
    private int velocityZHash;
    private int isAimingHash;
    private int jumpHash;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;

        velocityHash = Animator.StringToHash("Velocity");
        velocityXHash = Animator.StringToHash("Velocity X");
        velocityZHash = Animator.StringToHash("Velocity Z");
        isAimingHash = Animator.StringToHash("Is Aiming");
        jumpHash = Animator.StringToHash("Jump");
    }

    private void Update()
    {
        HandleMove();
        HandleJump();
    }

    private bool IsAiming()
    {
        return Input.GetButton("Fire2");
    }

    private bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private bool IsJumpPressed()
    {
        return Input.GetButtonDown("Jump");
    }

    private void HandleMove()
    {
        if (stopped)
            return;

        animator.SetBool(isAimingHash, IsAiming());

        if (IsAiming())
        {
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, camera.eulerAngles.y, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f); 
        }
        
        Vector3 direction = new Vector3(movement.x, 0f, movement.y).normalized;
        animator.SetFloat(velocityHash, IsRunning() ? direction.magnitude : direction.magnitude / 2, dampTime, Time.deltaTime);

        if (direction.magnitude > 0)
        {
            float playerSpeed;
            float velocityX;
            float velocityZ;
            if (IsRunning())
            {
                playerSpeed = sprint;
                velocityX = movement.x * 2;
                velocityZ = movement.y * 2;
            }
            else
            {
                playerSpeed = speed;
                velocityX = movement.x;
                velocityZ = movement.y;
            }
            
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;

            if (!IsAiming())
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f); 
            }
            
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            animator.SetFloat(velocityXHash, velocityX, dampTime, Time.deltaTime);
            animator.SetFloat(velocityZHash, velocityZ, dampTime, Time.deltaTime);
            characterController.Move(moveDirection.normalized * (playerSpeed * Time.deltaTime));
        }
        else
        {
            animator.SetFloat(velocityXHash, 0f, dampTime, Time.deltaTime);
            animator.SetFloat(velocityZHash, 0f, dampTime, Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        bool onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if (onSurface && IsJumpPressed())
        {
            animator.SetTrigger(jumpHash);
            
            velocity.y = Mathf.Sqrt(jumpRange * 2 * GRAVITY);
        }
        else
        {
            animator.ResetTrigger(jumpHash);

            velocity.y -= GRAVITY * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
        
    }

    public void OnMove(InputValue value) => movement = value.Get<Vector2>();

    public void SetStopped(bool playerStopped)
    {
        stopped = playerStopped;
    }
}
