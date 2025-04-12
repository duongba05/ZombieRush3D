using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;
    private float horizontal;
    private float vertical;

    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform cameraTransform;
    public float cameraDistance = 5f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        RotateCamera();
    }

    private void HandleInput()
    {
        Move();
        HandleAnimation();

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("isShoot");
        }
    }

    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = camForward * vertical + camRight * horizontal;

        if (move != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), rotationSpeed * Time.deltaTime);
        }

        moveDirection = move * runSpeed;

        if (controller.isGrounded)
        {
            velocity.y = -1f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        controller.Move((moveDirection + velocity) * Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = jumpForce;
        animator.SetBool("isJump", true);
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -60f, 60f);

        Quaternion camRotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 offset = new Vector3(0, 2f, -cameraDistance);
        cameraTransform.position = transform.position + camRotation * offset;
        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);
    }

    private void HandleAnimation()
    {
        bool isMoving = horizontal != 0 || vertical != 0;
        animator.SetBool("isRunning", isMoving);

        if (!controller.isGrounded)
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Ground"))
        {
            animator.SetBool("isJump", false);
        }
    }
}
