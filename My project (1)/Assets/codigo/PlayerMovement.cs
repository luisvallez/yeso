using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float caminarSpeed = 8.0f;
    public float CorrerSpeed = 12.0f;
    public float RotationSpeed = 100.0f;
    public float fuerzaSalto = 5.0f;
    public float maxLookUpAngle = 80.0f;
    public float maxLookDownAngle = 80.0f;
    private Rigidbody fisicas;
    private float currentRotationX = 0.0f;
    private bool hasJumped = false;

    private readonly float StandingHeight = 1.0f;
    private readonly float CrouchingHeight = 0.7f;
    private bool isCrouching = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        fisicas = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Sprint
        float speed = Input.GetKey(KeyCode.LeftShift) ? CorrerSpeed : caminarSpeed;

        transform.Translate(speed * Time.deltaTime * new Vector3(horizontal, 0.0f, vertical));

        // Rotacion
        float rotationY = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0, rotationY * Time.deltaTime * RotationSpeed, 0));

        // Rotacion de la cámara arriba y abajo
        float rotationX = -Input.GetAxis("Mouse Y");
        currentRotationX += rotationX * Time.deltaTime * RotationSpeed;
        currentRotationX = Mathf.Clamp(currentRotationX, -maxLookUpAngle, maxLookDownAngle);

        Camera.main.transform.localRotation = Quaternion.Euler(currentRotationX, 0, 0);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && !hasJumped)
        {
            fisicas.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
            hasJumped = true;
        }

        // Agacharse
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching) { StandUp(); }
            else { Crouch(); }

            void Crouch()
            {
                isCrouching = true;
                transform.localScale = new Vector3(transform.localScale.x, CrouchingHeight, transform.localScale.z);
                fisicas.AddForce(new Vector3(0, -1.5f, 0), ForceMode.Impulse);
                // Aquí se agacha y se cambia la posicion del jugador para evitar la caida del mono fake
            }
            void StandUp()
            {
                isCrouching = false;
                transform.localScale = new Vector3(transform.localScale.x, StandingHeight, transform.localScale.z);
                // Aquí también podrías cambiar la posición del personaje para que parezca que se está levantando
            }
        }
    }

    // Verificar si el jugador está en el suelo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            hasJumped = false; // Restablecer la bandera cuando toca el suelo
        }
    }
}
