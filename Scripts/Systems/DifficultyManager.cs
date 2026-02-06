using UnityEngine;

/// <summary>
/// Monitors game progression and triggers difficulty phase shifts based on the player's score
/// Acts as the bridge between the Scoring system and the ObstacleSpawner
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    [Header("Phase Configuration")]
    [SerializeField] private DifficultyProfileSO _phase1;
    [SerializeField] private DifficultyProfileSO _phase2;
    [SerializeField] private DifficultyProfileSO _phase3;
    [SerializeField] private DifficultyProfileSO _phase4;

    [Header("References")]
    [SerializeField] private ObstacleSpawner _spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Initialize the first phase immediately
        if (_spawner != null && _phase1 != null)
        {
            _spawner.SetPhase(_phase1);
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += CheckDifficultyProgression;
        }
    }

    private void OnDestroy()
    {
        // Cleanup event subscription to avoid memory leaks
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= CheckDifficultyProgression;
        }
    }

    /// <summary>
    /// Evaluates if the current score meets the threshold for a difficulty increase
    /// </summary>
    /// <param name="currentScore">The updated score from GameManager</param>
    private void CheckDifficultyProgression(int currentScore)
    {
        // Hardcoded milestones for phase transitions
        if (currentScore == 5)
        {
            ChangePhase(_phase2);
            Debug.Log("Moving Phase 2");
        }
        else if(currentScore == 15)
        {
            ChangePhase(_phase3);
            Debug.Log("Moving Phase 3");
        }
        else if(currentScore == 30)
        {
            ChangePhase(_phase4);
            Debug.Log("Moving Phase 4");
        }
    }

    /// <summary>
    /// Updates the spawner with new difficulty parameters
    /// </summary>
    private void ChangePhase(DifficultyProfileSO nextPhase)
    {
        if(_spawner != null && nextPhase != null)
        {
            _spawner.SetPhase(nextPhase);
        }
    }
}
