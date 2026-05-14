using UnityEngine;
public class AudioListenerController : MonoBehaviour
{
    [Tooltip("The player's Transform (position source).")]
    public Transform player;

    [Tooltip("The camera's Transform (rotation source).")]
    public Transform cameraTransform;

    [Tooltip("Smoothing for position tracking.")]
    [Range(0f, 20f)]
    public float positionSmoothing = 0f;

    [Tooltip("Smoothing for rotation tracking.")]
    [Range(0f, 20f)]
    public float rotationSmoothing = 0f;

    private void LateUpdate()
    {
        if (player == null || cameraTransform == null) return;

        // Position follows player
        if (positionSmoothing > 0f)
            transform.position = Vector3.Lerp(transform.position, player.position, positionSmoothing * Time.deltaTime);
        else
            transform.position = player.position;

        // Rotation follows camera
        if (rotationSmoothing > 0f)
            transform.rotation = Quaternion.Slerp(transform.rotation, cameraTransform.rotation, rotationSmoothing * Time.deltaTime);
        else
            transform.rotation = cameraTransform.rotation;
    }
}
