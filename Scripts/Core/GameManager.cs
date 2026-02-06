using UnityEngine;
using System;
using UnityEngine.SceneManagement;


/// <summary>
/// Central manager that ochestrates the core loop, state management and persistent data
/// Acts as a Singleton to provide a global access point for game states and scoring
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Static instance of the GameManager for global access
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary> Triggered when the score is incremented, passes the new score </summary>
    public event Action<int> OnScoreChanged;
    /// <summary> Triggered when the player loses </summary>
    public event Action OnGameOver;
    /// <summary> Triggered when the gameplay officially begins </summary>
    public event Action OnGameStart;

    /// <summary> The current session's score </summary>
    public int CurrentScore { get; private set; }
    /// <summary> The highest score achieved across all sessions </summary>
    public int HighScore { get; private set; }

    /// <summary> Returns true if the game is currently being played </summary>
    public bool IsGameActive { get; private set; } = false;
    /// <summary> Returns true if the game logic is paused </summary>
    public bool IsPaused { get; private set; } = false;

    private const string TUTORIAL_PREF_KEY = "ShowTutorial";

    private void Awake()
    {
        // Standard Singleton pattern implementation
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        // Load persistent data from PlayerPrefs
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    /// <summary>
    /// Initializes and starts the game session
    /// </summary>
    public void StartGame()
    {
        IsGameActive = true;
        OnGameStart?.Invoke();
        Debug.Log("Game started");
    }

    /// <summary>
    /// Increments the score and notifies listeners 
    /// Only functions if the game is currently active
    /// </summary>
    public void AddScore()
    {
        if(!IsGameActive) return;
        CurrentScore++;
        Debug.Log($"Score: {CurrentScore}");
        OnScoreChanged?.Invoke(CurrentScore); ;
    }

    /// <summary>
    /// Ends the current game session and updates the High Score if necessary
    /// </summary>
    public void TriggerGameOver()
    {
        if(!IsGameActive) return;
        IsGameActive = false;
        if(CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt("HighScore", HighScore);
            PlayerPrefs.Save();
        }
        Debug.Log("Game Over");
        OnGameOver?.Invoke();
    }

    /// <summary>
    /// Reloads the current active scene to reset the game state
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Handles application closure accross different platforms and the Unity Editor
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Game closure");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    /// <summary>
    /// Toggles the game's pause state by modifying the Time Scale
    /// </summary>
    public void TogglePause()
    {
        IsPaused = !IsPaused;
        if(IsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Retrieves the user's preference regarding the tutorial display
    /// </summary>
    /// <returns>True if the tutorial should be shown</returns>
    public bool GetTutorialPreference()
    {
        return PlayerPrefs.GetInt(TUTORIAL_PREF_KEY, 1) == 1;
    }

    /// <summary>
    /// Saves the user's preference for showing the tutorial 
    /// </summary>
    /// <param name="shouldShow">Preference state to save</param>
    public void SaveTutorialPreference(bool shouldShow)
    {
        PlayerPrefs.SetInt(TUTORIAL_PREF_KEY, shouldShow ? 1 : 0);
        PlayerPrefs.Save();
    }
}
