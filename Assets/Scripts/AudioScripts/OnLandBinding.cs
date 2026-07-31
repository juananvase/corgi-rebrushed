using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays a land sound when the Corgi touches the ground after being airborne.
/// Listens to ForcesBasedCharacterMovementController.OnLand UnityEvent
/// and fires a one-shot FMOD event.
/// </summary>
[AddComponentMenu("Corgi Audio/On Land Binding")]
public class OnLandBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // land event (e.g., Player/Land)

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
    // Lifecycle: subscribe to the land event
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_controller != null)
            _controller.OnLand.AddListener(OnLandRaised);
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_controller != null)
            _controller.OnLand.RemoveListener(OnLandRaised);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: land triggered — play the audio event
    // ────────────────────────────────────────────────────────────
    private void OnLandRaised()
    {
        // PlayOneShot is sufficient — the land sound is a short
        // one-shot that doesn't need parameter updates or manual lifetime.
        if (_fmodEvent.IsNull) return;
        RuntimeManager.PlayOneShot(_fmodEvent);
    }
}