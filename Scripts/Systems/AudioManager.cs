using UnityEngine;

/// <summary>
/// Global service for handling background music and sound effects
/// Manages volume persistence and reacts to gameplay events
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip _scoreClip;

    private const string MUSIC_VOL_KEY = "MusicVolume";
    private const string SFX_VOL_KEY = "SFXVolume";

    private void Awake()
    {
        // Singleton pattern to ensure persistent audio across scenes
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeAudio();
        PlayMusic();
    }

    private void OnEnable()
    {
        // Subscribe to GameManager events to trigger sounds automatically
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += PlayScoreSound;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks and errors on scene destroy
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= PlayScoreSound;
        }
    }

    /// <summary>
    /// Loads saved volume preferences and configures the music source
    /// </summary>
    private void InitializeAudio()
    {
        float savedMusicLevel = PlayerPrefs.GetFloat(MUSIC_VOL_KEY, 5f);
        float savedSFXLevel = PlayerPrefs.GetFloat(SFX_VOL_KEY, 5f);
        SetMusicVolume(savedMusicLevel);
        SetSFXVolume(savedSFXLevel);
        _musicSource.loop = true;
        _musicSource.clip = _backgroundMusic;
    }

    private void PlayMusic()
    {
        if(_musicSource.clip != null && !_musicSource.isPlaying)
        {
            _musicSource.Play();
        }
    }

    /// <summary>
    /// Plays the scoring sound effect
    /// Callback for the OnScoreChanged event
    /// </summary>
    private void PlayScoreSound(int score)
    {
        // Use PlayOneShot to allow overlapping sounds without cutting off
        if (_scoreClip != null)
        {
            _sfxSource.PlayOneShot(_scoreClip);
        }
    }

    /// <summary>
    /// Updates music volume and persists the value
    /// </summary>
    /// <param name="sliderValue">Value from UI slider (usually 0-5)</param>
    public void SetMusicVolume(float sliderValue)
    {
        float volume = sliderValue / 5f; // Normalize UI value to 0-1 range
        _musicSource.volume = volume;
        PlayerPrefs.SetFloat(MUSIC_VOL_KEY, sliderValue);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float sliderValue)
    {
        float volume = sliderValue / 5f; // Normalize UI value to 0-1 range
        _sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SFX_VOL_KEY, sliderValue);
        PlayerPrefs.Save();
    }

    public float GetSavedMusicLevel() => PlayerPrefs.GetFloat(MUSIC_VOL_KEY, 5f);
    public float GetSavedSFXLevel() => PlayerPrefs.GetFloat(SFX_VOL_KEY, 5f);
}
