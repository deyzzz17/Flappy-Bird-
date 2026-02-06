# Flappy Bird : Modular Game Logic Framework
This repository contains the core C# source code for an advanced 2D arcade framework. Instead of a full Unity projec, this is a showcase of clean code, design patterns, and modular architecture applied to game development.

## Architecture Vision
The codebase is build on the principle of High Cohesion and Low Coupling. Every system is designed to function independently or through well-defined inerfaces.

### Key Systems Included:
- Decoupled Input System: Uses `IInputReader` to isolate input logic from gameplay mechanics.
- Global State Management: A robust `GameManager` using the Singleton pattern and C# Actions to broadcast game states (Start, Pause, Game Over).
- Synchronized World Physics: A `WorldSpeedManager' that acts as a "Single Source of Truth" for all moving entities, ensuring consistency during speed ramps or dash mechanics.
- Smart UI Controllers: Optimized UI scripts using `StringBuilder` and `TMP_SpriteAsset` to maximize performance and minimize Garbage Collection (GC) spikes.

### Modular Difficulty and Phase System
The game is handled trhough a Data-Driven approach using Unity `ScriptableObjects`.
- Phase-Based Progression: Instead of hardcoding difficulty, I created `DifficultyProfile` assets. Each profile defines specific world speed, spawn rates, and obstacle types.
- Global Synchronization: The `WorldSpeedManager` interprets these assets to synchronize the speed of every moving object in the scene simultaneously.

### Design Pattern and Best Practices
- Singleton Pattern: Implemented for global services (`AudioManager`, `DifficultyManager`, `GameManager`, ...).
- Observer Pattern: Extensive use of C# events to allow UI and Audio systems to react to gameplay without direct references.
- Strategy Pattern: Used for input handling, allowing easy swaps between Keyboard and Controller layouts.
- Guard Clauses: All methods prioritize early returns to improve code readability and reduce nesting.
- XML Documentation: Critical architectural scripts are fully documented with XML tags for IDE IntelliSense support.

### Code Organization
The scripts are organized by responsablity to reflect a professional production environment:
- `/Core`: Global managers and singleton services.
- `/Inputs`: Interface-based input abstraction (`IInputReader`) and hardware-specific implementations.
- `/Obstacles`: Specialized logic for environmental hazards, including interaction triggers and automated movement.
- `/Player`: Physics-based movement, collision handling, and visual bridges.
- `/Systems`: High-level gameplay mechanics such as the Difficulty scaling logic, Audio management and World synchronization.
- `/UI`: Performance-optimized display controllers and settings management.
- `/World`: Spawning logic, environmental, and difficulty scaling.
