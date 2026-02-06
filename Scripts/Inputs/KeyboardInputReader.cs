using UnityEngine;
using System;

/// <summary>
/// Keyboard implementation of IInputReader
/// Handles key detection and persistent rebinding logic
/// </summary>
public class KeyboardInputReader : MonoBehaviour, IInputReader
{
    // Events defined by IInputReader interface
    public event Action OnJumpInitiated;
    public event Action OnDashInitiated;
    public event Action OnFireInitiated;
    public event Action OnPauseInitiated;

    private KeyCode _jumpKey;
    private KeyCode _fireKey;
    private KeyCode _dashKey;
    private KeyCode _pauseKey;

    private void Awake()
    {
        LoadKeys();
    }

    private void Update()
    {
        // Poll for key presses and invoke corresponding events if anyone is listening
        if (Input.GetKeyDown(_jumpKey)) OnJumpInitiated?.Invoke();
        if (Input.GetKeyDown(_fireKey)) OnFireInitiated?.Invoke();
        if (Input.GetKeyDown(_dashKey)) OnDashInitiated?.Invoke();
        if (Input.GetKeyDown(_pauseKey)) OnPauseInitiated?.Invoke();
    }

    /// <summary>
    /// Loads key bindings from PlayerPrefs with hardcoded defaults as fallback
    /// </summary>
    private void LoadKeys()
    {
        _jumpKey = (KeyCode)PlayerPrefs.GetInt("Key_Jump", (int)KeyCode.Space);
        _fireKey = (KeyCode)PlayerPrefs.GetInt("Key_Fire", (int)KeyCode.F);
        _dashKey = (KeyCode)PlayerPrefs.GetInt("Key_Dash", (int)KeyCode.D);
        _pauseKey = (KeyCode)PlayerPrefs.GetInt("Key_Pause", (int)KeyCode.P);
    }

    public KeyCode GetKeyForAction(GameAction action)
    {
        switch (action)
        {
            case GameAction.Jump: return _jumpKey;
            case GameAction.Fire: return _fireKey;
            case GameAction.Dash: return _dashKey;
            case GameAction.Pause: return _pauseKey;
            default: return KeyCode.None;
        }
    }

    public bool TryRebindKey(GameAction action, KeyCode newKey)
    {
        // Basic validation: ignore mouse clicks and prevent duplicate bindings
        if (newKey == KeyCode.Mouse0 || newKey == KeyCode.Mouse1) return false;
        if (IsKeyUsed(newKey, action)) return false;
        switch (action)
        {
            case GameAction.Jump: _jumpKey = newKey; break;
            case GameAction.Fire: _fireKey = newKey; break;
            case GameAction.Dash: _dashKey = newKey; break;
            case GameAction.Pause: _pauseKey = newKey; break;
        }
        SaveKey(action, newKey);
        return true;
    }

    /// <summary>
    /// Verifies if a KeyCode is already assigned to another action to prevent conflicts
    /// </summary>
    private bool IsKeyUsed(KeyCode key, GameAction actionToIgnore)
    {
        if (actionToIgnore != GameAction.Jump && _jumpKey == key) return true;
        if (actionToIgnore != GameAction.Fire && _fireKey == key) return true;
        if (actionToIgnore != GameAction.Dash && _dashKey == key) return true;
        if (actionToIgnore != GameAction.Jump && _pauseKey == key) return true;
        return false;
    }

    private void SaveKey(GameAction action, KeyCode key)
    {
        PlayerPrefs.SetInt("Key_" + action.ToString(), (int)key);
        PlayerPrefs.Save();
    }
}
