using UnityEngine;
using TMPro;
using System.Text;

/// <summary>
/// Updates the real-time score display during gameplay using sprite-based typography
/// </summary>
public class ScoreUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TMP_SpriteAsset _myNumberSprites;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Link the specific sprite asset for numerical styling
        if (_scoreText != null && _myNumberSprites != null)
        {
            _scoreText.spriteAsset = _myNumberSprites;
        }
        UpdateScoreDisplay(0);
    }

    private void OnEnable()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScoreDisplay;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
        }
    }

    /// <summary>
    /// Rebuilds the score string using rich text sprite tags for optimal performance and visuals
    /// </summary>
    private void UpdateScoreDisplay(int newScore)
    {
        if (_scoreText != null)
        {
            string scoreString = newScore.ToString();
            StringBuilder stringBuilder = new StringBuilder();
            foreach(char c in scoreString)
            {
                // Direct char-to-int conversion (ASCII)
                int spriteIndex = c - '0';
                stringBuilder.Append($"<sprite index={spriteIndex}>");
            }
            _scoreText.text = stringBuilder.ToString();
        }
    }
}
