using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    bool canJump = true;
    bool isCrouching = true; // Inicia en posición agachada
    float standingHeight; // Guarda la altura original del jugador en posición de pie
    float crouchingHeight; // Altura cuando está agachado
    float jumpForce; // Fuerza aplicada al saltar
    Vector3 respawnPoint; // Punto de respawn

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        standingHeight = playerHeight; // Guarda la altura original al inicio
        crouchingHeight = standingHeight * 0.2f; // Calcula la altura cuando está agachado
        jumpForce = Mathf.Sqrt(-2 * Physics.gravity.y * (standingHeight * 0.5f)); // Fuerza para alcanzar la mitad de la altura original

        // Guarda el punto de respawn al inicio
        respawnPoint = transform.position;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);
        Debug.Log("Grounded: " + grounded); // Agrega esto para verificar el estado del suelo
        MyInput();

        if (grounded)
        {
            rb.drag = groundDrag;
            canJump = true;
        }
        else
        {
            rb.drag = 0;
            // Restablecer canJump cuando toca el suelo

        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 4.5f;
        }
        else
        {
            moveSpeed = 2.5f;
        }

        // Salto
        if (grounded && Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }

        // Agacharse
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouch();
        }
        else
        {
            StandUp();
        }

        // Verifica si el jugador ha caído por debajo de la altura -10 y realiza el respawn
        if (transform.position.y < -10)
        {
            Respawn();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false;
    }

    private void Crouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            playerHeight = crouchingHeight; // Establecer la altura cuando está agachado
            transform.localScale = new Vector3(transform.localScale.x, 0.2f, transform.localScale.z);
        }
    }

    private void StandUp()
    {
        if (isCrouching)
        {
            isCrouching = false;
            playerHeight = standingHeight; // Restablecer la altura original
            transform.localScale = new Vector3(transform.localScale.x, 0.4f, transform.localScale.z);
        }
    }

    private void Respawn()
    {
        // Teletransporta al jugador al punto de respawn
        transform.position = respawnPoint;
        rb.velocity = Vector3.zero; // Detiene la velocidad del jugador
    }
}
