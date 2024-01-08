using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRB;
    private Transform objectGrabPointTransform;
    private BoxCollider objectCollider;

    private void Awake()
    {
        objectRB = GetComponent<Rigidbody>();
        objectCollider = GetComponent<BoxCollider>();
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRB.useGravity = false;
        objectCollider.isTrigger = true;

        // Mantener la posición y rotación del objeto según el punto de agarre
        objectRB.MovePosition(objectGrabPointTransform.position);
        objectRB.MoveRotation(objectGrabPointTransform.rotation);
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRB.useGravity = true;
        objectCollider.isTrigger = false;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            Vector3 newPosition = objectGrabPointTransform.position;
            objectRB.MovePosition(newPosition);

            Quaternion newRotation = Quaternion.LookRotation(objectGrabPointTransform.forward, objectGrabPointTransform.up);
            objectRB.MoveRotation(newRotation);
        }
    }

    // En tu script ObjectGrabbable
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NoColisionar"))
        {
            Physics.IgnoreCollision(objectCollider, collision.collider);
        }
    }


}
