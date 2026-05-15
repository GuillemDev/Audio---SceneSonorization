using System.Collections;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    [Header("References")]

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2.5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Prompt (optional)")]
    [SerializeField] private GameObject interactPrompt;

    [Header("Audio")]
    [SerializeField] private AudioSource characterAudioSource;
    [SerializeField] private AudioClip characterSFX;

    private Transform _player;

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
            interactPrompt.SetActive(inRange);

        if (inRange && Input.GetKeyDown(interactKey))
            ToggleDialogue();
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, _player.position) <= interactionRange;
    }

    private void ToggleDialogue()
    {
        if (characterAudioSource != null && characterSFX != null)
            characterAudioSource.PlayOneShot(characterSFX);
    }
}