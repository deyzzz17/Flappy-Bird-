using UnityEngine;

/// <summary>
/// Handles the physical movement of the bird using impulses
/// Also triggers the game start sequence on the first player input
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BirdMover : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The upward force applied when the bird jumps")]
    [SerializeField] private float _jumpForce = 6.5f;

    private IInputReader _inputReader;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _inputReader = GetComponent<IInputReader>();
    }

    private void Start()
    {
        // Keep the bird suspended until the first jump initiates the game
        _rigidbody.gravityScale = 0f;
        _rigidbody.linearVelocity = Vector2.zero;
    }

    private void OnEnable()
    {
        if (_inputReader != null)
        {
            _inputReader.OnJumpInitiated += HandleJumpInput;
        }
    }

    private void OnDisable()
    {
        if (_inputReader != null)
        {
            _inputReader.OnJumpInitiated -= HandleJumpInput;
        }
    }

    /// <summary>
    /// Decides whether to start the game or simply jump based on the current game state
    /// </summary>
    private void HandleJumpInput()
    {
        if(GameManager.Instance != null && !GameManager.Instance.IsGameActive)
        {
            StartGameSequence();
            Jump();
        }
        else
        {
            Jump();
        }
    }

    /// <summary>
    /// Activates gravity and signals the GameManager to begin the world movement
    /// </summary>
    private void StartGameSequence()
    {
        _rigidbody.gravityScale = 1.5f;
        GameManager.Instance.StartGame();
    }

    private void Jump()
    {
        // Reset vertical velocity before jumping to ensure consistent jump height
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0f);
        _rigidbody.AddForce(Vector2.up *  _jumpForce, ForceMode2D.Impulse);
    }
}
