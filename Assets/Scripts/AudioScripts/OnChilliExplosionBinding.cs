using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays audio when the Chilli Explosion ability is activated.
/// Listens to ChilliExplosion.OnChilliExplosionActivated UnityEvent
/// and fires a one-shot FMOD event.
/// </summary>
[AddComponentMenu("Corgi Audio/On Chilli Explosion Binding")]
public class OnChilliExplosionBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // chilli explosion sound (e.g., Abilities/ChilliExplosion)

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
    // Lifecycle: subscribe to the explosion event
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_chilli != null)
            _chilli.OnChilliExplosionActivated.AddListener(OnExplosion);
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_chilli != null)
            _chilli.OnChilliExplosionActivated.RemoveListener(OnExplosion);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: explosion activated — play the audio event
    // ────────────────────────────────────────────────────────────
    private void OnExplosion()
    {
        // PlayOneShot is sufficient — the explosion sound is a short
        // one-shot that doesn't need parameter updates or manual lifetime.
        if (!_fmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_fmodEvent);
    }
}