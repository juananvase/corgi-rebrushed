using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays footstep sounds for the Corgi character.
/// Listens to ForcesBasedCharacterMovementController.OnFootstep<float> UnityEvent,
/// sets the global "PlayerSpeed" FMOD parameter, and fires a one-shot footstep event.
/// </summary>
[AddComponentMenu("Corgi Audio/On Footstep Binding")]
public class OnFootstepBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // footstep event (e.g., Player/Footstep)

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
    // Lifecycle: subscribe to the footstep event
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_controller != null)
            _controller.OnFootstep.AddListener(OnFootstepRaised);
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_controller != null)
            _controller.OnFootstep.RemoveListener(OnFootstepRaised);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: footstep triggered — update speed param + play
    // ────────────────────────────────────────────────────────────
    private void OnFootstepRaised(float speed)
    {
        if (_fmodEvent.IsNull) return;

        // Update the global PlayerSpeed parameter so the FMOD event
        // can blend between walk/run/gallop layers based on speed.
        RuntimeManager.StudioSystem.setParameterByName("PlayerSpeed", speed);
        RuntimeManager.PlayOneShot(_fmodEvent);
    }
}