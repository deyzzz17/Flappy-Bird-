using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages the visual progression of the game by transitioning backgrounds based on the player's score milestones
/// </summary>
public class BackgroundManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _currentBackground;
    [SerializeField] private Image _nextBackground;

    [Header("Configurations")]
    [Tooltip("List of sprites to cycle through during the game")]
    [SerializeField] private Sprite[] _backgrounds;
    [Tooltip("Number of points required to trigger a background change")]
    [SerializeField] private int _scoreThreshold = 5;
    [Tooltip("Duration of the cross-fade transition in seconds")]
    [SerializeField] private float _fadeDuration = 1.0f;

    private int _currentIndex = 0;
    private Coroutine _fadeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_backgrounds.Length > 0)
        {
            // Initialize the first background and hide the overlay image
            _currentBackground.sprite = _backgrounds[0];
            Color c = _nextBackground.color;
            c.a = 0f;
            _nextBackground.color = c;
        }
    }

    private void OnEnable()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += CheckScoreForBackgroundChange;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= CheckScoreForBackgroundChange;
        }
    }

    /// <summary>
    /// Checks if the current score has reached the threshold for a visual change
    /// </summary>
    private void CheckScoreForBackgroundChange(int score)
    {
        if (score == 0) return;
        // Trigger transition every time the score is a multiple of the threshold
        if (score % _scoreThreshold == 0)
        {
            TransitionToNextBackground();
        }
    }

    /// <summary>
    /// Calculates the next index and manages the fade coroutine state
    /// </summary>
    private void TransitionToNextBackground()
    {
        int nextIndex = (_currentIndex +1) % _backgrounds.Length;
        // Stop any ongoing transition to prevent overlapping color calculations
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeRoutine(nextIndex));
        _currentIndex = nextIndex;
    }

    /// <summary>
    /// Performs a cross-fade by increasing the opacity of the next background over the current one
    /// </summary>
    private IEnumerator FadeRoutine(int targetIndex)
    {
        _nextBackground.sprite = _backgrounds[targetIndex];
        float timer = 0f;
        Color color = _nextBackground.color;
        while(timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            // Linear interpolation of the alpha channel
            color.a = Mathf.Lerp(0f, 1f, timer / _fadeDuration);
            _nextBackground.color = color;
            yield return null;
        }
        // Finalize transition: set the main background and reset the overlay
        _currentBackground.sprite = _backgrounds[targetIndex];
        color.a = 0f;
        _nextBackground.color = color;
    }
}
