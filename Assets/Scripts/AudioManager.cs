using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    AudioSource source;

    [SerializeField] private AudioMixer m_Mixer;
    [SerializeField] private AudioMixerSnapshot m_DefaultSnapshot;
    [SerializeField] private AudioMixerSnapshot[] m_Snapshots;

    private AudioState currentState = AudioState.GAMEPLAY;

    public enum AudioState
    {
        GAMEPLAY,
        PAUSE_MENU
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            ActivateLoopSound();
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            DeactivateLoopSound();

        AudioState desiredState = PauseMenu.GameIsPaused ? AudioState.PAUSE_MENU : AudioState.GAMEPLAY;
        if (desiredState != currentState)
        {
            SetCurrentSnapshot(desiredState, 0.5f);
            currentState = desiredState;
        }
    }

    public void SetCurrentSnapshot(AudioState state, float duration)
    {
        if (state == AudioState.GAMEPLAY)
            m_DefaultSnapshot.TransitionTo(duration);
        else
            m_Snapshots[(int)state].TransitionTo(duration);
    }

    private void ActivateLoopSound()
    {
        source.Play();
    }

    private void DeactivateLoopSound()
    {
        source.Stop();
    }
}