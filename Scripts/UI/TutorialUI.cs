using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the first-time tutorial overlay and persists user display preferences
/// </summary>
public class TutorialUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private Toggle _dontShowAgainToggle;
    [SerializeField] private Button _closeButton;

    // Static flag to ensure the tutorial only pops up once per application launch
    private static bool _hasShownThisSession = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _closeButton.onClick.AddListener(CloseTutorial);
        CheckAndShowTutorial();
    }

    /// <summary>
    /// Determines if the tutorial should be displayed based on session state and saved preferences
    /// </summary>
    private void CheckAndShowTutorial()
    {
        // Don't show if already seen in this session or if the player opted out globally
        if (_hasShownThisSession)
        {
            _tutorialPanel.SetActive(false);
            return;
        }
        bool playerWantsTutorial = GameManager.Instance.GetTutorialPreference();
        if (playerWantsTutorial)
        {
            ShowPanel();
        }
        else
        {
            _tutorialPanel.SetActive(false);
        }
    }

    private void ShowPanel()
    {
        if (_tutorialPanel != null)
        {
            _tutorialPanel.SetActive(true);
            _hasShownThisSession = true;
            // Pause the game to let the player read instructions
            Time.timeScale = 0f;
            // Sync with GameManager's pause state if necessary
            if (!GameManager.Instance.IsPaused) GameManager.Instance.TogglePause();
        }
    }

    /// <summary>
    /// Saves the "Don't Show Again" preference and resumes the game
    /// </summary>
    private void CloseTutorial()
    {
        // Save the inverse of the toggle state (if toggle is ON, shouldShow is FALSE)
        bool shouldShowNextTime = !_dontShowAgainToggle.isOn;
        GameManager.Instance.SaveTutorialPreference(shouldShowNextTime);
        _tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
        // Resume game state through the manager
        if (GameManager.Instance.IsPaused)
        {
            GameManager.Instance.TogglePause();
        }
    }
}
