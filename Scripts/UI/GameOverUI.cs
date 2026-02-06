using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

/// <summary>
/// Manages the end-game overlay, displaying final scores and handling menu navigation
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    [SerializeField] private TMP_SpriteAsset _myNumberSprites;

    [Header("Pamameters")]
    [SerializeField] private float _delayBeforeShow = 1.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameOverPanel.SetActive(false);
        SetupTextComponent(_currentScoreText);
        SetupTextComponent(_highScoreText);
        if(GameManager.Instance != null )
        {
            GameManager.Instance.OnGameOver += HandleGameOver;
        }
        // Event delegation for UI buttons
        _restartButton.onClick.AddListener(RestartClicked);
        _quitButton.onClick.AddListener(QuitClicked);
    }

    private void SetupTextComponent(TextMeshProUGUI textComp)
    {
        if(textComp != null && _myNumberSprites != null)
        {
            textComp.spriteAsset = _myNumberSprites;
        }
    }

    private void OnDisable()
    {
        if(GameManager.Instance != null )
        {
            GameManager.Instance.OnGameOver -= HandleGameOver;
        }
        _restartButton.onClick.RemoveListener(RestartClicked);
        _quitButton.onClick.RemoveListener(QuitClicked);
    }

    private void HandleGameOver()
    {
        StartCoroutine(ShowPanelWithDelay());
    }

    private IEnumerator ShowPanelWithDelay()
    {
        yield return new WaitForSeconds(_delayBeforeShow);
        if(GameManager.Instance != null )
        {
            // Convert numerical scores into string-based sprite tags
            _currentScoreText.text = ConvertScoreToSprites(GameManager.Instance.CurrentScore);
            _highScoreText.text = ConvertScoreToSprites(GameManager.Instance.HighScore);
        }
        _gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Converts a numeric score into a string of TextMeshPro sprite tags
    /// Example: 123 -> <sprite index=1><sprite index=2><sprite index=3>
    /// </summary>
    private string ConvertScoreToSprites(int score)
    {
        string scoreString = score.ToString();
        StringBuilder stringBuilder = new StringBuilder();
        foreach (char c in scoreString)
        {
            int spriteIndex = c - '0'; // ASCII math to get integer from char
            stringBuilder.Append($"<sprite index={spriteIndex}>");
        }
        return stringBuilder.ToString();
    }

    private void RestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

    private void QuitClicked()
    {
        GameManager.Instance.QuitGame();
    }
}
