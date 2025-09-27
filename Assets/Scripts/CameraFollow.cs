using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // assign Player here

    [Header("Settings")]
    public Vector3 offset = new Vector3(0f, 8f, -6f); // camera position relative to player
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // desired camera position
        Vector3 desiredPosition = target.position + offset;

        // smooth follow
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothed;

        // make camera look at player (optional)
        transform.LookAt(target);
    }
}
