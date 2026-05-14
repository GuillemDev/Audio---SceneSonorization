using UnityEngine;

/// <summary>
/// Third Person Camera Controller
/// 
/// SETUP INSTRUCTIONS:
/// 1. Attach this script to your Camera (or a camera rig GameObject).
/// 2. Set the 'Target' field in the Inspector to your player/character Transform.
/// 3. Adjust the offset, sensitivity, and clamp values to taste.
/// 4. Optional: assign a LayerMask to 'Collision Layers' to enable camera collision.
///
/// CONTROLS:
///   Mouse X/Y  — Orbit the camera around the target
///   Scroll Wheel — Zoom in / out
/// </summary>
public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The Transform the camera will orbit around (e.g. your player).")]
    public Transform target;

    [Header("Orbit Settings")]
    [Tooltip("Horizontal mouse sensitivity.")]
    public float sensitivityX = 4f;

    [Tooltip("Vertical mouse sensitivity.")]
    public float sensitivityY = 3f;

    [Tooltip("Minimum vertical angle (degrees). Negative = below horizon.")]
    public float minVerticalAngle = -30f;

    [Tooltip("Maximum vertical angle (degrees).")]
    public float maxVerticalAngle = 70f;

    [Tooltip("Smoothing applied to camera rotation (lower = snappier).")]
    [Range(0f, 20f)]
    public float rotationSmoothing = 8f;

    [Header("Distance / Zoom")]
    [Tooltip("Default distance from the target.")]
    public float defaultDistance = 5f;

    [Tooltip("Minimum zoom distance.")]
    public float minDistance = 1.5f;

    [Tooltip("Maximum zoom distance.")]
    public float maxDistance = 12f;

    [Tooltip("Scroll wheel zoom speed.")]
    public float zoomSpeed = 3f;

    [Tooltip("Smoothing applied to zoom (lower = snappier).")]
    [Range(0f, 20f)]
    public float zoomSmoothing = 8f;

    [Header("Pivot Offset")]
    [Tooltip("Local-space offset from target origin (e.g. raise to look at chest/head).")]
    public Vector3 pivotOffset = new Vector3(0f, 1.6f, 0f);

    [Header("Camera Collision")]
    [Tooltip("Enable to pull the camera forward when it clips into geometry.")]
    public bool enableCollision = true;

    [Tooltip("Layers that block the camera.")]
    public LayerMask collisionLayers = ~0; // Everything by default

    [Tooltip("Small buffer to keep camera from clipping into surfaces.")]
    public float collisionBuffer = 0.2f;

    // ── Private state ────────────────────────────────────────────────────────

    private float _yaw;          // horizontal rotation (degrees)
    private float _pitch;        // vertical rotation (degrees)
    private float _targetYaw;
    private float _targetPitch;

    private float _currentDistance;
    private float _targetDistance;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("[ThirdPersonCamera] No target assigned! Please set the Target field in the Inspector.");
        }

        // Initialise from current camera orientation so there's no jump on start
        Vector3 angles = transform.eulerAngles;
        _yaw = _targetYaw = angles.y;
        _pitch = _targetPitch = angles.x;

        _currentDistance = _targetDistance = defaultDistance;

        // Hide and lock the cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleInput();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        ApplySmoothing();
        PositionCamera();
    }

    // ── Input ────────────────────────────────────────────────────────────────

    private void HandleInput()
    {
        // Mouse look
        _targetYaw   += Input.GetAxis("Mouse X") * sensitivityX;
        _targetPitch -= Input.GetAxis("Mouse Y") * sensitivityY;  // inverted so up = up
        _targetPitch  = Mathf.Clamp(_targetPitch, minVerticalAngle, maxVerticalAngle);

        // Scroll zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _targetDistance -= scroll * zoomSpeed;
        _targetDistance  = Mathf.Clamp(_targetDistance, minDistance, maxDistance);

        // Optional: unlock cursor with Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Re-lock on click
        if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // ── Smoothing ────────────────────────────────────────────────────────────

    private void ApplySmoothing()
    {
        float t = Time.deltaTime;

        _yaw   = Mathf.LerpAngle(_yaw,   _targetYaw,   rotationSmoothing * t);
        _pitch = Mathf.LerpAngle(_pitch, _targetPitch, rotationSmoothing * t);

        _currentDistance = Mathf.Lerp(_currentDistance, _targetDistance, zoomSmoothing * t);
    }

    // ── Positioning ──────────────────────────────────────────────────────────

    private void PositionCamera()
    {
        // World-space pivot point (character + offset)
        Vector3 pivot = target.position + target.TransformDirection(pivotOffset);

        // Desired camera direction
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 direction   = rotation * Vector3.back; // behind the character

        float distance = _currentDistance;

        // Camera collision: shorten distance if something is in the way
        if (enableCollision)
        {
            if (Physics.SphereCast(pivot, collisionBuffer, direction, out RaycastHit hit,
                                   _currentDistance, collisionLayers,
                                   QueryTriggerInteraction.Ignore))
            {
                distance = Mathf.Max(hit.distance - collisionBuffer, minDistance);
            }
        }

        transform.position = pivot + direction * distance;
        transform.LookAt(pivot);
    }
}
