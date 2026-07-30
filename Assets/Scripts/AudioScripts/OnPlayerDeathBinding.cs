using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays a death stinger when the player dies.
/// Listens to Health.OnDeath on the player's Health component.
/// Finds the player via PlayerHealth component — no tag dependency.
/// </summary>
[AddComponentMenu("Corgi Audio/On Player Death Binding")]
public class OnPlayerDeathBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // player death stinger (e.g., Player/Death)

    // ────────────────────────────────────────────────────────────
    // Cached reference to the player's Health component
    // ────────────────────────────────────────────────────────────
    private Health _playerHealth;

    // ────────────────────────────────────────────────────────────
    // Lifecycle: find the player via PlayerHealth component
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to the death event
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_playerHealth != null)
            _playerHealth.OnDeath.AddListener(OnPlayerDeath);
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_playerHealth != null)
            _playerHealth.OnDeath.RemoveListener(OnPlayerDeath);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: player died — play the death stinger
    // ────────────────────────────────────────────────────────────
    private void OnPlayerDeath()
    {
        if (!_fmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_fmodEvent);
    }
}