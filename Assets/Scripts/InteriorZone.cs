using UnityEngine;

public class InteriorZone : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            audioManager.SetInsideInterior(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            audioManager.SetInsideInterior(false);
    }
}