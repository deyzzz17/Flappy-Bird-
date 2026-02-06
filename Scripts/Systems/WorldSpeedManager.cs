using UnityEngine;

/// <summary>
/// Central authority for the world's movement speed
/// Coordinates speed changes across all moving entities (scrolling backgrounds, obstacles)
/// </summary>
public class WorldSpeedManager : MonoBehaviour
{
    public static WorldSpeedManager Instance { get; private set; }

    [Header("Settings")]
    [Tooltip("The default movement speed of the world")]
    [SerializeField] private float _baseSpeed = 5f;
    [Tooltip("Multiplier applied to the base speed when the player is dashing")]
    [SerializeField] private float _dashMultiplier = 2.5f;

    /// <summary> The final calculated speed used by moving objects this frame </summary>
    public float CurrentSpeed { get; private set; }

    /// <summary> Exposes the base speed for spawners to calculate distance intervals </summary>
    public float BaseSpeed => _baseSpeed;

    private void Awake()
    {
        // Singleton pattern to ensure global access to speed data
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        CurrentSpeed = _baseSpeed;
    }

    /// <summary>
    /// Adjusts the current speed based on the player's dash state
    /// </summary>
    /// <param name="isDashing">True if the dash multiplier should be applied</param>
    public void SetDashState(bool isDashing)
    {
        CurrentSpeed = isDashing ? _baseSpeed * _dashMultiplier : _baseSpeed;
    }
}
