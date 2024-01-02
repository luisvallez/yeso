
using UnityEngine;

public class cameraPosition : MonoBehaviour
{
    public Transform cameraPositionn;
    private void Update()
    {
        transform.position = cameraPositionn.position;


    }
}
