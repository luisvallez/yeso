using UnityEngine;

//NO MOVERLE PLIS xd
public class GroundCheck : MonoBehaviour
{
    public PlayerController playerController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }
        playerController.SetGrounded(true);

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }
        playerController.SetGrounded(false);

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }
        playerController.SetGrounded(true);

    }
}
