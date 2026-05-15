using System.Collections;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform doorMesh;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2.5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Rotation")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Prompt (optional)")]
    [SerializeField] private GameObject interactPrompt;

    [Header("Audio")]
    [SerializeField] private AudioSource openAudioSource;
    [SerializeField] private AudioClip openSFX;
    [SerializeField] private AudioSource closeAudioSource;
    [SerializeField] private AudioClip closeSFX;

    private Transform _player;
    private bool _isOpen = false;
    private bool _isAnimating = false;
    private Coroutine _doorCoroutine;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (_player == null) return;

        bool inRange = IsPlayerInRange();

        if (interactPrompt != null)
            interactPrompt.SetActive(inRange && !_isAnimating);

        if (inRange && Input.GetKeyDown(interactKey) && !_isAnimating)
            ToggleDoor();
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, _player.position) <= interactionRange;
    }

    private void ToggleDoor()
    {
        if (doorMesh == null)
        {
            Debug.LogError("DoorInteraction: doorMesh is not assigned!", this);
            return;
        }

        _isOpen = !_isOpen;

        if (_isOpen)
        {
            if (openAudioSource != null && openSFX != null)
                openAudioSource.PlayOneShot(openSFX);
            else
                Debug.LogWarning("DoorInteraction: openAudioSource or openSFX is missing.", this);
        }
        else
        {
            if (closeAudioSource != null && closeSFX != null)
                closeAudioSource.PlayOneShot(closeSFX);
            else
                Debug.LogWarning("DoorInteraction: closeAudioSource or closeSFX is missing.", this);
        }

        if (_doorCoroutine != null)
            StopCoroutine(_doorCoroutine);

        _doorCoroutine = StartCoroutine(AnimateDoor(_isOpen ? openAngle : 0f));
    }

    private IEnumerator AnimateDoor(float targetAngle)
    {
        if (doorMesh == null)
        {
            Debug.LogError("DoorInteraction: doorMesh is null, aborting animation.", this);
            _isAnimating = false;
            yield break;
        }

        _isAnimating = true;

        float startAngle = doorMesh.localEulerAngles.y;
        if (startAngle > 180f) startAngle -= 360f;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = easeCurve.Evaluate(elapsed / animationDuration);
            float angle = Mathf.Lerp(startAngle, targetAngle, t);
            doorMesh.localEulerAngles = new Vector3(
                doorMesh.localEulerAngles.x,
                angle,
                doorMesh.localEulerAngles.z
            );
            yield return null;
        }

        doorMesh.localEulerAngles = new Vector3(
            doorMesh.localEulerAngles.x,
            targetAngle,
            doorMesh.localEulerAngles.z
        );

        _isAnimating = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}