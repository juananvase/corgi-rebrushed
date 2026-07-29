using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Controls audio when the player enters or exits painting mode.
/// - Sets the global "PaintingState" parameter (used by bus effects like LPF + Reverb on the Master bus).
/// - Plays enter/exit stingers via PlayOneShot.
/// </summary>
[AddComponentMenu("Corgi Audio/On Painting Mode Binding")]
public class OnPaintingModeBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event References — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _enterFmodEvent;   // stinger when painting mode activates
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _exitFmodEvent;    // stinger when painting mode deactivates

    // ────────────────────────────────────────────────────────────
    // Cached reference to the gameplay controller
    // ────────────────────────────────────────────────────────────
    private PaintingModeController _controller;

    // ────────────────────────────────────────────────────────────
    // Lifecycle: cache the controller reference
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        // FindObjectOfType is safe here because there is only one
        // PaintingModeController in the scene (attached to the Corgi).
        _controller = FindObjectOfType<PaintingModeController>();
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to gameplay events
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_controller != null)
        {
            _controller.OnPaintingModeEnter.AddListener(OnEnter);
            _controller.OnPaintingModeExit.AddListener(OnExit);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks / stale calls
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_controller != null)
        {
            _controller.OnPaintingModeEnter.RemoveListener(OnEnter);
            _controller.OnPaintingModeExit.RemoveListener(OnExit);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: painting mode entered
    // ────────────────────────────────────────────────────────────
    private void OnEnter()
    {
        // Set the global parameter so bus effects (LPF, Reverb)
        // transition to the "painting" state.
        RuntimeManager.StudioSystem.setParameterByName("PaintingState", 1f);

        // Play the enter stinger if an event is assigned.
        if (!_enterFmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_enterFmodEvent);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: painting mode exited
    // ────────────────────────────────────────────────────────────
    private void OnExit()
    {
        // Return bus effects to their default (neutral) state.
        RuntimeManager.StudioSystem.setParameterByName("PaintingState", 0f);

        // Play the exit stinger if an event is assigned.
        if (!_exitFmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_exitFmodEvent);
    }
}