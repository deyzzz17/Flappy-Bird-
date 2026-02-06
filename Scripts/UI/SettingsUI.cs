using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Manages the settings menu, including volume control, key rebinding, and system navigation
/// </summary>
public class SettingsUI : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private IInputReader _inputReader;

    [Header("Sliders")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [Header("Text for Keys")]
    [SerializeField] private TextMeshProUGUI _txtJump;
    [SerializeField] private TextMeshProUGUI _txtFire;
    [SerializeField] private TextMeshProUGUI _txtDash;
    [SerializeField] private TextMeshProUGUI _txtPause;

    [Header("System Button")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    private GameObject _currentKeyButton;
    private bool _isRebinding = false;

    private void Awake()
    {
        _inputReader = FindFirstObjectByType<KeyboardInputReader>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _settingsPanel.SetActive(false);
        UpdateKeyDisplays();
        SetupSliders();
        // System button listeners using anonymous functions for simplicity
        _restartButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePause();
            GameManager.Instance.RestartGame();
        });
        _quitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
    }

    private void OnEnable()
    {
        if(_inputReader != null)
        {
            _inputReader.OnPauseInitiated += HandlePauseInput;
        }
    }

    private void OnDisable()
    {
        if (_inputReader != null)
        {
            _inputReader.OnPauseInitiated -= HandlePauseInput;
        }
    }

    private void HandlePauseInput()
    {
        if (_isRebinding) return;
        ToggleSettings();
    }

    private void ToggleSettings()
    {
        bool isActive = !_settingsPanel.activeSelf;
        _settingsPanel.SetActive(isActive);
        GameManager.Instance.TogglePause();
    }

    /// <summary>
    /// Initiates the asynchronous process of listening for a new key press
    /// </summary>
    public void StartRebindProcess(int actionIdex)
    {
        if(_isRebinding) return;
        GameAction action = (GameAction)actionIdex;
        StartCoroutine(WaitForKeyPress(action));
    }

    private IEnumerator WaitForKeyPress(GameAction action)
    {
        _isRebinding = true;
        TextMeshProUGUI textToUpdate = GetTextComponent(action);
        string originalText = textToUpdate.text;
        textToUpdate.text = "...";
        yield return null;
        float timer = 0f;
        float timeout = 5f;
        while(timer < timeout)
        {
            if (Input.anyKeyDown)
            {
                // Iterate through all keycodes to find the pressed key
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    { 
                        if (_inputReader.TryRebindKey(action, keyCode))
                        {
                            UpdateKeyDisplays();
                            _isRebinding = false;
                            yield break;
                        }
                        else
                        {
                            Debug.Log("Invalid Key");
                            textToUpdate.text = originalText;
                            _isRebinding = false;
                            yield break;
                        }
                    }   
                }
            }
            // Use unscaledDeltaTime because the game is paused (Time.timeScale = 0)
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        textToUpdate.text = originalText;
        _isRebinding = false;
    }

    private void UpdateKeyDisplays()
    {
        _txtJump.text = _inputReader.GetKeyForAction(GameAction.Jump).ToString();
        _txtFire.text = _inputReader.GetKeyForAction(GameAction.Fire).ToString();
        _txtDash.text = _inputReader.GetKeyForAction(GameAction.Dash).ToString();
        _txtPause.text = _inputReader.GetKeyForAction(GameAction.Pause).ToString();
    }

    private void SetupSliders()
    {
        _musicSlider.minValue = 0;
        _musicSlider.maxValue = 5;
        _musicSlider.wholeNumbers = true;
        _sfxSlider.minValue = 0;
        _sfxSlider.maxValue = 5;
        _sfxSlider.wholeNumbers = true;
        if(AudioManager.Instance != null)
        {
            _musicSlider.value = AudioManager.Instance.GetSavedMusicLevel();
            _sfxSlider.value = AudioManager.Instance.GetSavedSFXLevel();
        }
        _musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        _sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
    }

    private void OnMusicSliderChanged(float value)
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }

    private void OnSFXSliderChanged(float value)
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }

    private TextMeshProUGUI GetTextComponent(GameAction action)
    {
        switch(action)
        {
            case GameAction.Jump: return _txtJump;
            case GameAction.Fire: return _txtFire;
            case GameAction.Dash: return _txtDash;
            case GameAction.Pause: return _txtPause;
            default: return null;
        }
    }
}
