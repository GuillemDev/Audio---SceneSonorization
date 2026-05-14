using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public static AudioSettings Instance { get; private set; }

    private const string MASTER_VOLUME_PARAM = "MASTER_VOLUME";
    private const string MUSIC_VOLUME_PARAM = "MUSIC_VOLUME";
    private const string AMBIENCE_VOLUME_PARAM = "AMBIENCE_VOLUME";
    private const string SFX_VOLUME_PARAM = "SFX_VOLUME";

    [SerializeField] private AudioMixer m_Mixer;

    [SerializeField] public Slider m_MasterSlider;
    [SerializeField] public Slider m_MusicSlider;
    [SerializeField] public Slider m_AmbienceSlider;
    [SerializeField] public Slider m_SfxSlider;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            LoadMasterVolume();
        }
        else
        {
            SetMasterVolumeFromSlider();
        }
        
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolumeFromSlider();
        }

        if (PlayerPrefs.HasKey("ambienceVolume"))
        {
            LoadAmbienceVolume();
        }
        else
        {
            SetAmbienceVolumeFromSlider();
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadSfxVolume();
        }
        else
        {
            SetSFXVolumeFromSlider();
        }
    }
    private void LoadMasterVolume()
    {
        m_MasterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        SetMasterVolumeFromSlider();
    }
    private void LoadMusicVolume()
    {
        m_MusicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolumeFromSlider();
    }
    private void LoadAmbienceVolume()
    {
        m_AmbienceSlider.value = PlayerPrefs.GetFloat("ambienceVolume");
        SetAmbienceVolumeFromSlider();
    }
    private void LoadSfxVolume()
    {
        m_SfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetSFXVolumeFromSlider();
    }

    public float MasterVolume
    {
        get => m_MasterVolume;
        set
        {
            float v = Mathf.Clamp01(value);
            if (Mathf.Approximately(v, m_MasterVolume)) return;

            m_MasterVolume = v;
            m_Mixer.SetFloat(MASTER_VOLUME_PARAM, m_MasterVolume);
        }
    }
    private float m_MasterVolume = 1;

    public float MusicVolume
    {
        get => m_MusicVolume;
        set
        {
            float v = Mathf.Clamp01(value);
            if (Mathf.Approximately(v, m_MusicVolume)) return;

            m_MusicVolume = v;
            m_Mixer.SetFloat(MUSIC_VOLUME_PARAM, m_MusicVolume);
        }
    }
    private float m_MusicVolume = 1;

    public float AmbienceVolume
    {
        get => m_AmbienceVolume;
        set
        {
            float v = Mathf.Clamp01(value);
            if (Mathf.Approximately(v, m_AmbienceVolume)) return;

            m_AmbienceVolume = v;
            m_Mixer.SetFloat(AMBIENCE_VOLUME_PARAM, m_AmbienceVolume);
        }
    }
    private float m_AmbienceVolume = 1;

    public float SFXVolume
    {
        get => m_SFXVolume;
        set
        {
            float v = Mathf.Clamp01(value);
            if (Mathf.Approximately(v, m_SFXVolume)) return;

            m_SFXVolume = v;
            m_Mixer.SetFloat(SFX_VOLUME_PARAM, m_SFXVolume);
        }
    }
    private float m_SFXVolume = 1;

    // Master Set/Get
    public float GetMasterVolume() => m_MasterVolume;
    public void SetMasterVolumeFromSlider()
    {
        float volume = m_MasterSlider.value;
        float v = Mathf.Clamp01(volume);
        if (Mathf.Approximately(v, m_MasterVolume)) return;

        m_MasterVolume = v;
        PlayerPrefs.SetFloat("masterVolume", m_MasterVolume);
        m_Mixer.SetFloat(MASTER_VOLUME_PARAM, Mathf.Log10(m_MasterVolume) * 20f);
    }
    // Music Set/Get
    public float GetMusicVolume() => m_MusicVolume;
    public void SetMusicVolumeFromSlider()
    {
        float volume = m_MusicSlider.value;
        float v = Mathf.Clamp01(volume);
        if (Mathf.Approximately(v, m_MusicVolume)) return;

        m_MusicVolume = v;
        PlayerPrefs.SetFloat("musicVolume", m_MusicVolume);
        m_Mixer.SetFloat(MUSIC_VOLUME_PARAM, Mathf.Log10(m_MusicVolume) * 20f);
    }
    // Ambience Set/Get
    public float GetAmbienceVolume() => m_AmbienceVolume;
    public void SetAmbienceVolumeFromSlider()
    {
        float volume = m_AmbienceSlider.value;
        float v = Mathf.Clamp01(volume);
        if (Mathf.Approximately(v, m_AmbienceVolume)) return;

        m_AmbienceVolume = v;
        PlayerPrefs.SetFloat("ambienceVolume", m_AmbienceVolume);
        m_Mixer.SetFloat(AMBIENCE_VOLUME_PARAM, Mathf.Log10(m_AmbienceVolume) * 20f);
    }
    // Sfx Set/Get
    public float GetSFXVolume() => m_SFXVolume;
    public void SetSFXVolumeFromSlider()
    {
        float volume = m_SfxSlider.value;
        float v = Mathf.Clamp01(volume);
        if (Mathf.Approximately(v, m_SFXVolume)) return;

        m_SFXVolume = v;
        PlayerPrefs.SetFloat("sfxVolume", m_SFXVolume);
        m_Mixer.SetFloat(SFX_VOLUME_PARAM, Mathf.Log10(m_SFXVolume) * 20f);
    }
}
