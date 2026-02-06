using UnityEngine;
using System;

/// <summary>
/// Defines the available input actions within the game
/// Used for mapping keys and identifying specific player intents
/// </summary>
public enum GameAction
{
    Jump,
    Fire,
    Dash,
    Pause
}

/// <summary>
/// Provides a decoupled interface for reading player inputs
/// This allows the game to switch between different input providers (Keyboard, Gamepad, Mobile) without modifying the core gameplay logic
/// </summary>
public interface IInputReader
{
    /// <summary> Triggered the moment the jump input is detected </summary>
    event Action OnJumpInitiated;
    /// <summary> Triggered the moment the dash input is detected </summary>
    event Action OnDashInitiated;
    /// <summary> Triggered when the fire command is given </summary>
    event Action OnFireInitiated;
    /// <summary> Triggered when the user requests to pause or unpause the game </summary>
    event Action OnPauseInitiated;

    /// <summary>
    /// Retrieves the current physical key assigned to a specific game action
    /// </summary>
    /// <param name="action">The game action to query</param>
    /// <returns>The KeyCode mapped to this action</returns>
    KeyCode GetKeyForAction(GameAction action);

    /// <summary>
    /// Attempts to change the key binding for a specific action
    /// </summary>
    /// <param name="action">The action to rebind</param>
    /// <param name="newKey">The new KeyCode to assign</param>
    /// <returns>True if the rebinding was successful (e.g., key not already in use)</returns>
    bool TryRebindKey(GameAction action, KeyCode newKey);
}
