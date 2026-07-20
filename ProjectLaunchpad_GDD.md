# Project: Launchpad - Game Design Document & AI Workflow

## 🎮 Core Concept
A 2D physics-based puzzle/action game where the player uses a slingshot mechanic to launch a character. Before launching, the player uses a "Card Matrix Interface" to select modifiers that alter the player's physical properties (mass, bounciness, gravity) to solve environmental puzzles.

## 🚀 Development Phases (4-Day Jam Schedule)

*   **[X] Phase 1: Minimum Viable Physics (MVP)**
    *   2D Rigidbody physics setup.
    *   Click-and-drag slingshot trajectory.
    *   Input System integration.
*   **[ ] Phase 2: The Card Matrix Interface**
    *   JSON-based card registry to store modifier data.
    *   UI Canvas Draft Pick overlay (pauses the game).
    *   Applying selected card stats to the `Rigidbody2D` and `PhysicsMaterial2D`.
*   **[ ] Phase 3: The Game Loop**
    *   Win/Loss conditions (hitting a target vs. falling out of bounds).
    *   Level resetting and scene management.
*   **[ ] Phase 4: Polish & Juice**
    *   Particle effects on launch and impact.
    *   Sound effects (drag tension, launch, bounce).
    *   Menu screens.

---

## 🤖 Antigravity CLI (`agy`) Best Practices

To prevent timeouts and keep the AI from hallucinating during the game jam, always follow these three rules when prompting the agent:

### 1. Atomic Prompts (One File at a Time)
Never ask the AI to "build the UI system." Ask it to build *one specific script*.
*   **Bad:** "Make the card system work."
*   **Good:** "Read `ProjectLaunchpad_GDD.md`. Now, create a C# script called `CardData.cs` that defines a struct for a Card (Name, MassModifier, BouncinessModifier). Do not write any other scripts."

### 2. Provide the Context, Restrict the Action
When starting a new system, explicitly tell the agent to read this `.md` file first so it understands the big picture, but forbid it from writing code until you give the go-ahead.
*   **Example:** "Read `ProjectLaunchpad_GDD.md` to understand Phase 2. Acknowledge the architecture, but do not generate any code yet."

### 3. Bypass the Scene Reader
Unity's scene-reading tools are slow. If you already know your object names and hierarchy, tell the AI to skip reading the scene and just write the code.
*   **Example:** "Do not scan the active scene. Generate a `UIManager.cs` script and save it to `Assets/Scripts`. It needs a reference to a `GameObject` called 'DraftPanel'."