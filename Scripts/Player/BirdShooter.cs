using UnityEngine;

/// <summary>
/// Handles the projectile firing logic
/// Manages weapon cooldowns and ensures only one projectile exists at a time
/// </summary>
public class BirdShooter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [Tooltip("Minimum time between two shots in seconds")]
    [SerializeField] private float _cooldownTime = 3f;

    private IInputReader _inputReader;
    private float _lastFireTime;
    // Reference to the currently active projectile to limit firing rate
    private GameObject _currentBullet;

    private void Awake()
    {
        _inputReader = GetComponent<IInputReader>();
    }

    private void OnEnable()
    {
        _inputReader.OnFireInitiated += TryShoot;
    }

    private void OnDisable()
    {
        _inputReader.OnFireInitiated -= TryShoot;
    }

    /// <summary>
    /// Validates if a shot can be fired based on cooldown and existing projectiles
    /// </summary>
    private void TryShoot()
    {
        // Check if enough time has passed since the last shot
        if (Time.time < _lastFireTime + _cooldownTime)
        {
            Debug.Log("CoolDown activated");
            return;
        }
        // Prevent firing if the previous projectile is still in the scene
        if (_currentBullet != null)
        {
            Debug.Log("Ball currently flying");
            return;
        }
        Shoot();
    }

    /// <summary>
    /// Spawns the projectile and records the firing time
    /// </summary>
    private void Shoot()
    {
        _lastFireTime = Time.time;
        _currentBullet = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
    }
}
