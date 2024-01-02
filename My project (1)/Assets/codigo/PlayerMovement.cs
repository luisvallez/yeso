using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float WalkSpeed = 8.0f;
    public float SprintSpeed = 12.0f;
    public float RotationSpeed = 100.0f;
    public float JumpForce = 5.0f;
    public float maxLookUpAngle = 80.0f; 
    public float maxLookDownAngle = 80.0f;

    private Rigidbody Physics;
    private float currentRotationX = 0.0f;
    private bool hasJumped = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Physics = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Sprint
        float speed = Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : WalkSpeed;

        transform.Translate(new Vector3(horizontal, 0.0f, vertical) * Time.deltaTime * speed);

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
            Physics.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            hasJumped = true;
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
