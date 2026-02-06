using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Handles the Dash mechanic, providing temporary invincibility or speed boosts
/// Coordinates with the WorldSpeedManager to affect global game speed during the dash
/// </summary>
public class BirdDash : MonoBehaviour
{
    [Header("Dash Settings")]
    [Tooltip("How long the dash effect lasts in seconds")]
    [SerializeField] private float _dashDuration = 1.0f;

    // Events to notify other systems (VFX, Audio, Score) of dash state changes
    public event Action OnDashStarted;
    public event Action OnDashEnded;

    private IInputReader _inputReader;
    private bool _isDashing = false;

    private void Awake()
    {
        _inputReader = GetComponent<IInputReader>();
    }

    private void OnEnable()
    {
        // Important: Subscribing to events when the object is active
        _inputReader.OnDashInitiated += PerformDash;
    }

    private void OnDisable()
    {
        // Important: Unsubscribing to prevent memory leaks and null reference errors
        _inputReader.OnDashInitiated -= PerformDash;
    }

    /// <summary>
    /// Validates and starts the dash sequence
    /// </summary>
    private void PerformDash()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive) return;
        if (_isDashing) return;
        StartCoroutine(DashRoutine());
    }

    /// <summary>
    /// Manages the dash lifecycle: notifies systems, waits for duration, and resets state
    /// </summary>
    private IEnumerator DashRoutine()
    {
        _isDashing = true;
        OnDashStarted?.Invoke();
        // Signal the world to speed up or change visual state
        if (WorldSpeedManager.Instance != null) WorldSpeedManager.Instance.SetDashState(true);
        Debug.Log("Dash Started");
        yield return new WaitForSeconds(_dashDuration);
        if (WorldSpeedManager.Instance != null) WorldSpeedManager.Instance.SetDashState(false);
        _isDashing = false;
        OnDashEnded?.Invoke();
        Debug.Log("Dash Ended");
    }
}
