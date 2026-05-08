using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -7);
    public float smoothTime = 0.1f;

    private Vector3 currentVelocity = Vector3.zero;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;
    private float dampingSpeed = 1.0f;
    private Vector3 shakeOffset = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);


        if (shakeDuration > 0)
        {
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }


        transform.position = smoothedPosition + shakeOffset;

        transform.LookAt(target.position + Vector3.up);
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}