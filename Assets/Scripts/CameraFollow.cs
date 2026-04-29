using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;     
    public Vector3 offset;      
    public float smoothSpeed = 0.125f; 

    void Start()
    {
        if (target != null)
        {
            //offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}