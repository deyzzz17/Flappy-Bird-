using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Data container defining the parameters for a specific gameplay difficulty phase
/// Allows Game Designers to tweak speed, spawn rates, and obstacle variety without touching code
/// </summary>
[CreateAssetMenu(fileName = "New Difficulty Phase", menuName = "FlappyTech/Difficulty Phase")]
public class DifficultyProfileSO : ScriptableObject
{
    [Header("General Settings")]
    /// <summary> User-friendly name for this difficulty level (e.g., "Early Game", "Insane") </summary>
    public string PhaseName;
    /// <summary> Base movement speed for obstacles during this phase </summary>
    public float GlobalSpeed = 5f;
    /// <summary> Time or distance interval between each obstacle spawn </summary>
    public float SpawnInterval = 0f;

    [Header("Phase Content")]
    /// <summary> List of obstacle prefabs that can be spawned during this phase </summary>
    /// [Tooltip("Add different pipe variants to increase variety in this phase")]
    public List<GameObject> AllowedPipePrefabs;

    /// <summary>
    /// Selects a random obstacle prefab from the allowed list
    /// </summary>
    /// <returns>A GameObject prefab or null if the list is empty</returns>
    public GameObject GetRandomPipe()
    {
        if (AllowedPipePrefabs.Count == 0) return null;
        return AllowedPipePrefabs[Random.Range(0, AllowedPipePrefabs.Count)];
    }
}
