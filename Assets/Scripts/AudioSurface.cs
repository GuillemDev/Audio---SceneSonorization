using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AudioSurface : MonoBehaviour
{
    [SerializeField] private FootStepManager.SurfaceType m_Surface = FootStepManager.SurfaceType.STONE;
    private void Awake()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        UpdateSurface();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        UpdateSurface();
    }

    private void UpdateSurface()
    {
        FootStepManager.Surface = m_Surface;
        Debug.Log($"[AudioSurface] Surface changed to: {m_Surface}");
    }
}