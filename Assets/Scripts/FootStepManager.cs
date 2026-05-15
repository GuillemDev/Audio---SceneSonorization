using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootStepManager : MonoBehaviour
{
    [System.Serializable]
    struct SurfaceFootstep
    {
        [SerializeField] private SurfaceType m_Surface;
        [SerializeField] private AudioClip[] m_Footsteps;

        [Space]
        [Tooltip("Random volume range for this surface. X = min, Y = max.")]
        [SerializeField] private Vector2 m_VolumeRange;

        [Tooltip("Random pitch range for this surface. X = min, Y = max.")]
        [SerializeField] private Vector2 m_PitchRange;

        public SurfaceType Surface => m_Surface;

        public AudioClip GetRandomClip()
        {
            if (m_Footsteps == null || m_Footsteps.Length == 0) return null;
            return m_Footsteps[Random.Range(0, m_Footsteps.Length)];
        }

        public float GetRandomVolume() => Random.Range(m_VolumeRange.x, m_VolumeRange.y);
        public float GetRandomPitch() => Random.Range(m_PitchRange.x, m_PitchRange.y);
    }

    public enum SurfaceType { STONE, RUG }

    // Set by AudioSurface when the player collides with a tagged surface collider
    public static SurfaceType Surface = SurfaceType.STONE;

    [SerializeField] private SurfaceFootstep[] m_FootstepSurfaces;

    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.playOnAwake = false;
    }

    /// <summary>
    /// Call this from an Animation Event on each footfall frame.
    /// </summary>
    public void PlayFootstep()
    {
        if (!TryGetSurface(Surface, out SurfaceFootstep entry))
        {
            Debug.LogWarning($"[FootStepManager] No entry found for surface: {Surface}");
            return;
        }

        AudioClip clip = entry.GetRandomClip();
        if (clip == null)
        {
            Debug.LogWarning($"[FootStepManager] No clips assigned for surface: {Surface}");
            return;
        }

        m_AudioSource.pitch = entry.GetRandomPitch();
        m_AudioSource.PlayOneShot(clip, entry.GetRandomVolume());
    }

    private bool TryGetSurface(SurfaceType surface, out SurfaceFootstep result)
    {
        foreach (var entry in m_FootstepSurfaces)
        {
            if (entry.Surface == surface)
            {
                result = entry;
                return true;
            }
        }
        result = default;
        return false;
    }
}