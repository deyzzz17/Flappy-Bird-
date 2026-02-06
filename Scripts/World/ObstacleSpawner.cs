using UnityEngine;

/// <summary>
/// Manages procedural obstacle generation based on world distance 
/// Uses difficulty profiles to determine spawn rates and obstacle types
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [Header("Initial Configuration")]
    /// <summary> The current difficulty profile defining spawn behavior and available prefabs </summary>
    [SerializeField] private DifficultyProfileSO _currentPhase;

    [Header("Spawn Limits")]
    [Tooltip("Maximum Y position for obstacle spawning")]
    [SerializeField] private float _maxSpawnY = 1.5f;
    [Tooltip("Minimum Y position for obstacle spawning")]
    [SerializeField] private float _minSpawnY = -2.04f;

    private float _distanceTraveled;
    private float _targetDistanceBetweenPipes;

    private void Start()
    {
        RecalculateTargetDistance();
    }

    private void Update()
    {
        // Guard clauses to ensure the game is active and configuration is valid
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive) return;
        if (_currentPhase == null) return;
        if (WorldSpeedManager.Instance != null)
        {
            // Calculate distance covered this frame based on world speed
            float moveStep = WorldSpeedManager.Instance.CurrentSpeed * Time.deltaTime;
            _distanceTraveled += moveStep;
        }
        // Check if we have traveled enough distance to spawn the next obstacle
        if (_distanceTraveled >= _targetDistanceBetweenPipes)
        {
            SpawnPipe();
            // Subtract instead of resetting to 0 to preserve precision over time
            _distanceTraveled -= _targetDistanceBetweenPipes;
        }
    }

    /// <summary>
    /// Updates the spawner with a new difficulty phase and resets progress
    /// </summary>
    /// <param name="newPhase">The new difficulty profile to apply</param>
    public void SetPhase(DifficultyProfileSO newPhase)
    {
        _currentPhase = newPhase;
        RecalculateTargetDistance();
        _distanceTraveled = 0f;
        Debug.Log($"Phase changée : {newPhase.name}");
    }

    /// <summary>
    /// Calculates the required distance between obstacles based on current speed and spawn intervals
    /// </summary>
    private void RecalculateTargetDistance()
    {
        if (_currentPhase != null && WorldSpeedManager.Instance != null)
        {
            _targetDistanceBetweenPipes = WorldSpeedManager.Instance.BaseSpeed * _currentPhase.SpawnInterval;
        }
    }

    /// <summary>
    /// Instantiates a random obstacle from the current phase and calculates its vertical position
    /// </summary>
    private void SpawnPipe()
    {
        GameObject prefabToSpawn = _currentPhase.GetRandomPipe();
        if (prefabToSpawn == null) return;
        float currentMin = _minSpawnY;
        float currentMax = _maxSpawnY;
        // Adjust spawn range based on the obstacle's specific movement constraints (if any)
        SpawnSettings settings = prefabToSpawn.GetComponent<SpawnSettings>();
        if (settings != null)
        {
            currentMin += settings.MovementAmplitude;
            currentMax -= settings.MovementAmplitude;
            // Safety check: if amplitude is too high, center the obstacle
            if (currentMin > currentMax)
            {
                float center = (_minSpawnY + _maxSpawnY) / 2f;
                currentMin = center;
                currentMax = center;
            }
        }
        Vector3 spawnPos = transform.position;
        spawnPos.y = Random.Range(currentMin, currentMax);
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}