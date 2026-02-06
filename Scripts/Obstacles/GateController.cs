using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the visual opening animation of the pipes
/// Uses a Coroutine for smooth movement interpolation
/// </summary>
public class GateController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Transform _topPipe;
    [SerializeField] private Transform _bottomPipe;
    [Tooltip("Distance each pipe moves from its starting position")]
    [SerializeField] private float _openDistance = 3f;
    [Tooltip("Duration of the opening animation in seconds")]
    [SerializeField] private float _openDuration = 0.2f;

    private bool _isOpen = false;

    /// <summary>
    /// Starts the opening sequence if it hasn't been triggered yet
    /// </summary>
    public void OpenGate()
    {
        if (_isOpen) return;
        _isOpen = true;
        StartCoroutine(AnimateOpening());
    }

    /// <summary>
    /// Smoothly animates pipes moving apart using a Lerp with SmoothStep for ease-in/out
    /// </summary>
    private IEnumerator AnimateOpening()
    {
        float elapsed = 0f;
        Vector3 startTop = _topPipe.localPosition;
        Vector3 startBottom = _bottomPipe.localPosition;
        // Define targets based on initial positions and offset
        Vector3 targetTop = startTop + Vector3.up * _openDistance;
        Vector3 targetBottom = startBottom + Vector3.down * _openDistance;
        while (elapsed < _openDuration)
        {
            // Normalize time and apply non-linear smoothing
            float x = elapsed / _openDuration;
            x = Mathf.SmoothStep(0f, 1f, x);
            _topPipe.localPosition = Vector3.Lerp(startTop, targetTop, x);
            _bottomPipe.localPosition = Vector3.Lerp(startBottom, targetBottom, x);

            elapsed += Time.deltaTime;
            yield return null;
        }
        // Snap to final values to avoid floating point imprecision
        _topPipe.localPosition = targetTop;
        _bottomPipe.localPosition = targetBottom;
    }
}