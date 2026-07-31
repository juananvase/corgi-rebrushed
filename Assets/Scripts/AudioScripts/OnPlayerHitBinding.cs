using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays audio when the player receives damage.
/// Listens to Health.OnDamaged on the player's Health component.
/// Finds the player via PlayerHealth component — no tag dependency.
/// </summary>
[AddComponentMenu("Corgi Audio/On Player Hit Binding")]
public class OnPlayerHitBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // player hit sound (e.g., Player/HitReceived)

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
    // Lifecycle: subscribe to the damage event
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_playerHealth != null)
            _playerHealth.OnDamaged.AddListener(OnPlayerHit);
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_playerHealth != null)
            _playerHealth.OnDamaged.RemoveListener(OnPlayerHit);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: player took damage — play the hit sound
    // ────────────────────────────────────────────────────────────
    private void OnPlayerHit(DamageInfo info)
    {
        // PlayOneShot — short hit reaction that doesn't need parameter updates.
        if (!_fmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_fmodEvent);
    }
}