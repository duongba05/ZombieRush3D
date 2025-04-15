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
    public float cameraDistance = 3f;

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
        HandleInput();  // Gọi hàm để xử lý các đầu vào (di chuyển, nhảy)
        RotateCamera(); // Xử lý quay camera
    }

    private void HandleInput()
    {
        Move();  // Di chuyển
        HandleAnimation();  // Cập nhật animation

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();  // Xử lý nhảy
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
            velocity.y = -1f; // Set y velocity to simulate gravity when on the ground
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime; // Apply gravity if not grounded
        }

        controller.Move((moveDirection + velocity) * Time.deltaTime);  // Move the player
    }

    private void Jump()
    {
        velocity.y = jumpForce;  // Set y velocity to simulate a jump
        animator.SetTrigger("isJump");  // Trigger jump animation
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        rotationY += mouseX;  // Rotate horizontally (left/right)
        rotationX -= mouseY;  // Rotate vertically (up/down)
        rotationX = Mathf.Clamp(rotationX, -60f, 60f);  // Clamp vertical rotation

        Quaternion camRotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 offset = new Vector3(0, 2f, -cameraDistance);  // Camera offset
        cameraTransform.position = transform.position + camRotation * offset;  // Set camera position
        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);  // Make the camera look at the player
    }

    private void HandleAnimation()
    {
        bool isMoving = horizontal != 0 || vertical != 0;

        if (controller.isGrounded)
        {
            animator.SetBool("isRunning", isMoving);  // Update running animation when moving
        }
        else
        {
            animator.SetBool("isRunning", false);  // Stop running animation if not grounded
        }

        if (!controller.isGrounded && velocity.y < 0)
        {
            animator.SetTrigger("isDead");  // Trigger dead animation if the player falls
        }
    }
}
