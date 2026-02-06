using UnityEngine;

/// <summary>
/// Bridge between game logic and animations
/// Listens to gameplay events to trigger appropriate visual states for the bird
/// </summary>
public class PlayerVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _birdAliveAnimator;
    [SerializeField] private BirdDash _birdDashScript;

    private void Awake()
    {
        // Safe reference fetching if not assigned in inspector
        if (_birdAliveAnimator == null)
        {
            _birdAliveAnimator = GetComponent<Animator>();
        }
        if(_birdDashScript == null)
        {
            _birdDashScript = GetComponent<BirdDash>();
        }
    }

    public void Start()
    {
        // Initialize state as idle/non-flying
        if (_birdAliveAnimator != null)
        {
            _birdAliveAnimator.SetBool("IsFlying", false);
        }
    }

    private void OnEnable()
    {
        // Subscribe to Game and Mechanic events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart += StartFlappingAnimation;
            GameManager.Instance.OnGameOver += TriggerDeathAnimation;
        }
        if(_birdDashScript != null)
        {
            _birdDashScript.OnDashStarted += StartDashVisual;
            _birdDashScript.OnDashEnded += StopDashVisual;
        }
    }

    private void OnDisable()
    {
        // Clean up subscriptions to avoid memory leaks
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= StartFlappingAnimation;
            GameManager.Instance.OnGameOver -= TriggerDeathAnimation;
        }
        if (_birdDashScript != null)
        {
            _birdDashScript.OnDashStarted -= StartDashVisual;
            _birdDashScript.OnDashEnded -= StopDashVisual;
        }
    }

    private void StartFlappingAnimation()
    {
        if(_birdAliveAnimator != null)
        {
            _birdAliveAnimator.SetBool("IsFlying", true);
        }
    }

    private void TriggerDeathAnimation()
    {
        if (_birdAliveAnimator != null)
        {
            _birdAliveAnimator.SetTrigger("Die");
        }
    }

    private void StartDashVisual()
    {
        if (_birdAliveAnimator != null) _birdAliveAnimator.SetBool("IsDashing", true);
    }

    private void StopDashVisual()
    {
        if (_birdAliveAnimator != null) _birdAliveAnimator.SetBool("IsDashing", false);
    }
}
