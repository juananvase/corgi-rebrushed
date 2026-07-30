using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays a stinger every time the player unlocks an ability.
/// Listens to AbilityManager.OnAbilityUnlocked UnityEvent.
/// </summary>
[AddComponentMenu("Corgi Audio/On Ability Unlock Binding")]
public class OnAbilityUnlockBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // ability unlock stinger (e.g., Music/AbilityUnlocked)

    // ────────────────────────────────────────────────────────────
    // Cached reference to the ability manager
    // ────────────────────────────────────────────────────────────
    private AbilityManager _manager;

    // ────────────────────────────────────────────────────────────
    // Lifecycle: cache the manager reference
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        _manager = FindObjectOfType<AbilityManager>();
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to the unlock event
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_manager != null)
            _manager.OnAbilityUnlocked.AddListener(OnAbilityUnlock);
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_manager != null)
            _manager.OnAbilityUnlocked.RemoveListener(OnAbilityUnlock);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: ability unlocked — play the stinger
    // ────────────────────────────────────────────────────────────
    private void OnAbilityUnlock()
    {
        if (!_fmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_fmodEvent);
    }
}