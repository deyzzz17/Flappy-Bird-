using UnityEngine;

/// <summary>
/// Handles visual animations for the top pipes, such as eye movements when the player approaches
/// </summary>
public class TopPipeVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _pipeAnimator;

    [Header("Settings")]
    [Tooltip("The X position threshold that triggers the look animation")]
    [SerializeField] private float _triggerPositionX = 0.5f;

    private bool _hasTriggered = false;

    private void Awake()
    {
        if(_pipeAnimator == null) _pipeAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _hasTriggered = false;
        if(_pipeAnimator != null)
        {
            _pipeAnimator.Rebind(); // Reset animator state when object is reused
        }
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver += HandleGameOver;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver -= HandleGameOver;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive) return;
        if (_hasTriggered) return;
        // Trigger animation when the pipe reaches a certain horizontal point
        if ((transform.position.x <= _triggerPositionX))
        {
            PlayEyeAnimation();
        }
    }

    private void PlayEyeAnimation()
    {
        _hasTriggered = true;
        if(_pipeAnimator != null)
        {
            _pipeAnimator.SetTrigger("Look");
        }
    }

    private void HandleGameOver()
    {
        if(_pipeAnimator != null)
        {
            _pipeAnimator.SetTrigger("GameOver");
        }
    }
}
