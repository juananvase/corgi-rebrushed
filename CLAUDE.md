# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview
Unity game project — Universidad de los Andes Summer Camp (EA track).
Product name: **CorgiRebrushed**

## Tech Stack
- **Engine:** Unity (URP — Universal Render Pipeline 17.5.0)
- **Input:** Unity InputSystem 1.19.0
- **Camera:** Cinemachine 3.1.7 (CinemachineCamera + CinemachineOrbitalFollow)
- **Inspector:** Odin Inspector (Sirenix) — used everywhere; use `[FoldoutGroup]`, `[ShowInInspector]`, `[Button]`, `[InlineEditor]`
- **Tweening:** PrimeTween
- **AI Nav:** Unity AI Navigation (NavMesh)

## Folder Structure Convention
All game content lives under `Assets/CorgiRebrushed/`. Every feature follows this layout:

```
FeatureName/
├── Art/
├── Prefabs/
├── Scenes/
└── Scripts/
```

Characters follow the same pattern under `Assets/CorgiRebrushed/Characters/Corgi/<FeatureName>/`.

## Architecture — Character System

### Central Hub: `CharacterControllerManager`
Single MonoBehaviour that owns input and shared state. Other controllers read from it — they do NOT read input independently.
- Exposes `MovementDirection` (world-space, orientation-relative) and `IsGrounded`
- Owns `InputActionAsset`; enables/disables the `"Player"` action map on Enable/Disable
- Ground check via `Physics.BoxCast` using dimensions from `CharacterDataSO`

### Movement: `ForcesBasedCharacterMovementController`
- Requires `CharacterControllerManager` + `Rigidbody` on the same GameObject
- Physics in `FixedUpdate`: adds forces for acceleration/deceleration, caps horizontal velocity
- Jump and Dash wired as `[Button]` test methods (not production input yet)

### Rotation: `CharacterRotationController`
- Reads camera position to set `Orientation.forward` each frame
- Slaps the character mesh toward `MovementDirection` using `Vector3.Slerp` + `RotationSpeed`

### Camera: `ThirdPersonCameraController`
- Wraps Cinemachine orbital camera; sets sensitivity on awake via `SetCameraSensitivity`
- Zoom is **implemented but commented out** (`ZoomInOut()` exists, not called in `Update`)
- Locks cursor on awake

## ScriptableObjects (Data)

| SO | Location | Purpose |
|---|---|---|
| `CharacterDataSO` | `Characters/Corgi/Data/SOScripts/` | Movement, deceleration, speed, rotation, ground-check box dims |
| `ThirdPersonCameraDataSO` | `Characters/Corgi/ThirdPersonCameraControl/Data/SOScripts/` | Camera sensitivity (X/Y), zoom speed, min/max distance |

Always create new SOs with `[CreateAssetMenu]` and decorate with `[InlineEditor]`.

## Architecture — Painting System (`Assets/CorgiRebrushed/Characters/Corgi/PaintingSystem/Scripts/`)
Active branch: `painting-system`. Mouse-driven freehand drawing triggered by right-click-hold, recognized as a `SymbolType` on release.

- `PaintingModeController` — entry point, polls right/left mouse buttons in `Update`. Right-click enters paint mode (disables `ThirdPersonCameraController`, shows overlay, calls `PaintCanvas.PrepareSession`); left-click drag begins/pauses a stroke; releasing right-click exits, ends the session, and hands the stroke to `SymbolRecognizer.Recognize`.
- `PaintCanvas` — owns a runtime `Texture2D` blitted onto a `RawImage`; draws circles along the mouse path into the texture and records raw screen-space points into a `List<Vector2>` stroke.
- `PaintOverlay` — fades a UI `Image` in/out with `PrimeTween` and toggles cursor lock/visibility for paint mode.
- `SymbolRecognizer` — takes the finished stroke's points and returns a `SymbolType`. Currently a stub (always returns `SymbolType.Unknown`); this is where recognition logic goes.
- `PaintingDataSO` — ScriptableObject driving brush size/color and overlay color/fade duration (follows the `CharacterDataSO` pattern).
- `Scripts/Editor/PaintingSystemWireup.cs` — editor-only wiring helper for the system's prefab/scene setup.

## Coding Conventions
- MonoBehaviours use `[field: SerializeField]` for auto-property serialization
- Group inspector fields with `[FoldoutGroup("References")]`, `[FoldoutGroup("Data")]`, `[FoldoutGroup("Testing")]`
- `[ShowInInspector]` on private runtime state for debugging
- `[Button]` for quick editor-only test actions on MonoBehaviours
- No comments unless the WHY is non-obvious

## Key Input Actions
Defined in `Assets/InputSystem_Actions.inputactions`:
- Action map `"Player"` — `Move`, and others for gameplay
- Action map `"CameraControl"` — camera orbit input
- `"Zoom"` action used by camera controller

## Git Branches
- `main` — stable branch, target for PRs
- `develop` — integration branch
- `painting-system` — current feature branch (active work)