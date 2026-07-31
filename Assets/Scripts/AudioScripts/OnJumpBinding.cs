using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays a jump sound when the Corgi jumps.
/// Listens to ForcesBasedCharacterMovementController.OnJump UnityEvent
/// and fires a one-shot FMOD event.
/// </summary>
[AddComponentMenu("Corgi Audio/On Jump Binding")]
public class OnJumpBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // jump event (e.g., Player/Jump)

    // ────────────────────────────────────────────────────────────
    // Cached reference to the movement controller
    // ────────────────────────────────────────────────────────────
    private ForcesBasedCharacterMovementController _controller;

    // ────────────────────────────────────────────────────────────
    // Lifecycle: cache the controller reference
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        _controller = FindObjectOfType<ForcesBasedCharacterMovementController>();
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to the jump event
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_controller != null)
            _controller.OnJump.AddListener(OnJumpRaised);
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_controller != null)
            _controller.OnJump.RemoveListener(OnJumpRaised);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: jump triggered — play the audio event
    // ────────────────────────────────────────────────────────────
    private void OnJumpRaised()
    {
        // PlayOneShot is sufficient — the jump sound is a short
        // one-shot that doesn't need parameter updates or manual lifetime.
        if (_fmodEvent.IsNull) return;
        RuntimeManager.PlayOneShot(_fmodEvent);
    }
}