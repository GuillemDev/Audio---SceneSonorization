using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class FootStepManager : MonoBehaviour
{
    [System.Serializable]
    struct SurfaceFootstep
    {
        [SerializeField] private SurfaceType m_Surface;
        [SerializeField] private AudioResource m_AudioContainer;

        public SurfaceType Surface => m_Surface;
        public AudioResource AudioContainer => m_AudioContainer;
    }

    public enum SurfaceType { STONE, RUG }

    // Set by AudioSurface when the player collides with a tagged surface collider
    public static SurfaceType Surface = SurfaceType.STONE;

    [SerializeField] private SurfaceFootstep[] m_FootstepSurfaces;

    // A dictionary to store a dedicated AudioSource for each surface type
    private Dictionary<SurfaceType, AudioSource> m_AudioSourcePool = new Dictionary<SurfaceType, AudioSource>();

    private void Awake()
    {
        // Get the primary AudioSource to copy its settings (Spatial blend, mixer groups, volume, etc.)
        AudioSource baseSource = GetComponent<AudioSource>();
        baseSource.playOnAwake = false;

        // Initialize a dedicated AudioSource for each surface configuration once at startup
        foreach (var entry in m_FootstepSurfaces)
        {
            if (entry.AudioContainer == null) continue;

            // Create a hidden/sub-AudioSource component to prevent resource swapping leaks
            AudioSource surfaceSpecificSource = gameObject.AddComponent<AudioSource>();

            // Copy base configuration settings
            surfaceSpecificSource.outputAudioMixerGroup = baseSource.outputAudioMixerGroup;
            surfaceSpecificSource.spatialBlend = baseSource.spatialBlend;
            surfaceSpecificSource.minDistance = baseSource.minDistance;
            surfaceSpecificSource.maxDistance = baseSource.maxDistance;
            surfaceSpecificSource.playOnAwake = false;

            // Assign the resource ONCE at startup. This prevents the runtime memory leak.
            surfaceSpecificSource.resource = entry.AudioContainer;

            m_AudioSourcePool[entry.Surface] = surfaceSpecificSource;
        }

        // Disable the base source since we are using the specialized ones
        baseSource.enabled = false;
    }

    /// <summary>
    /// Call this from an Animation Event on each footfall frame.
    /// </summary>
    public void PlayFootstep()
    {
        // Grab the pre-allocated AudioSource for the active surface
        if (m_AudioSourcePool.TryGetValue(Surface, out AudioSource targetSource))
        {
            // Simply call play. No resource re-assignments = zero memory allocations!
            targetSource.Play();
        }
        else
        {
            Debug.LogWarning($"[FootStepManager] No pre-configured AudioSource found for surface: {Surface}");
        }
    }
}