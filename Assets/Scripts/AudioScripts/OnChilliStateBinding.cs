using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays stingers when the Chilli speed-boost state starts and ends.
/// Listens to ChilliExplosion.OnChilliStateEnter and OnChilliStateExit UnityEvents.
/// </summary>
[AddComponentMenu("Corgi Audio/On Chilli State Binding")]
public class OnChilliStateBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event References — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _enterFmodEvent;   // chilli state enter stinger (e.g., Abilities/ChilliStateEnter)

    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _exitFmodEvent;    // chilli state exit stinger (e.g., Abilities/ChilliStateExit)

    // ────────────────────────────────────────────────────────────
    // Cached reference to the ability
    // ────────────────────────────────────────────────────────────
    private ChilliExplosion _chilli;

    // ────────────────────────────────────────────────────────────
    // Lifecycle: cache the ability reference
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        _chilli = FindObjectOfType<ChilliExplosion>();
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to state events
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_chilli != null)
        {
            _chilli.OnChilliStateEnter.AddListener(OnEnter);
            _chilli.OnChilliStateExit.AddListener(OnExit);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_chilli != null)
        {
            _chilli.OnChilliStateEnter.RemoveListener(OnEnter);
            _chilli.OnChilliStateExit.RemoveListener(OnExit);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: chilli state entered — play enter stinger
    // ────────────────────────────────────────────────────────────
    private void OnEnter()
    {
        // PlayOneShot — short stingers that don't need parameter updates.
        if (!_enterFmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_enterFmodEvent);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: chilli state exited — play exit stinger
    // ────────────────────────────────────────────────────────────
    private void OnExit()
    {
        if (!_exitFmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_exitFmodEvent);
    }
}