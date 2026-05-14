using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    AudioSource source;

    [SerializeField] private AudioMixer m_Mixer;
    [SerializeField] private AudioMixerSnapshot m_DefaultSnapshot;
    [SerializeField] private AudioMixerSnapshot[] m_Snapshots;

    [SerializeField] private GameObject Player;

    private AudioState currentState = AudioState.GAMEPLAY;
    private bool isInsideInterior = false;

    public enum AudioState
    {
        GAMEPLAY,
        PAUSE_MENU,
        INTERIOR
    }

    private void Update()
    {
        AudioState desiredState;
        if (PauseMenu.GameIsPaused)
            desiredState = AudioState.PAUSE_MENU;
        else if (isInsideInterior)
            desiredState = AudioState.INTERIOR;
        else
            desiredState = AudioState.GAMEPLAY;

        if (desiredState != currentState)
        {
            SetCurrentSnapshot(desiredState, 0.5f);
            currentState = desiredState;
        }
    }
    public void SetInsideInterior(bool inside)
    {
        isInsideInterior = inside;
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