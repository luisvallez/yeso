using UnityEngine;
using UnityEngine.InputSystem;

//NO MOVERLE PLIS xd
public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public GameObject camHolder;
    private readonly float speed = 4;
    private readonly float sprintSpeed = 6.5f;
    private readonly float crouchSpeed = 1.75f;
    private readonly float sensitivity = .1f;
    private readonly float maxForce = 0;
    private readonly float jumpForce = 6;
    private float lookRotation;
    private Vector2 move, look;
    private bool isSprinting, isCrouching, grounded;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        isCrouching = context.ReadValueAsButton();
    }

    private void Update()
    {
        anim.SetBool("isCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new(move.x, 0, move.y);
        targetVelocity *= isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : speed);

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Jump()
    {
        Vector3 jumpForces = rb.velocity;

        if (grounded)
        {
            jumpForces.y = jumpForce;
        }
        rb.velocity = jumpForces;
    }

    void Look()
    {
        transform.Rotate(look.x * sensitivity * Vector3.up);

        lookRotation += -look.y * sensitivity;
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        Look();
    }

    public void SetGrounded(bool state)
    {
        grounded = state;
    }
}
