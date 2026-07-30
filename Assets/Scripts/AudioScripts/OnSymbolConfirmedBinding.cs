using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays audio stingers when a symbol is confirmed (recognized) or failed (not recognized).
/// - Listens to PaintingModeController.OnSymbolConfirmed and OnSymbolFailed UnityEvents.
/// - Uses PlayOneShot for both — no persistent instance needed since these are one-shot stingers.
/// </summary>
[AddComponentMenu("Corgi Audio/On Symbol Confirmed Binding")]
public class OnSymbolConfirmedBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event References — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _confirmedFmodEvent; // stinger when the drawn symbol matches an ability
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _failedFmodEvent;    // stinger when the symbol is not recognized

    // ────────────────────────────────────────────────────────────
    // Cached reference to the gameplay controller
    // ────────────────────────────────────────────────────────────
    private PaintingModeController _controller;

    // ────────────────────────────────────────────────────────────
    // Lifecycle: cache the controller reference
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        // Safe — only one PaintingModeController exists in the scene.
        _controller = FindObjectOfType<PaintingModeController>();
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to symbol resolution events
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_controller != null)
        {
            _controller.OnSymbolConfirmed.AddListener(OnConfirmed);
            _controller.OnSymbolFailed.AddListener(OnFailed);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_controller != null)
        {
            _controller.OnSymbolConfirmed.RemoveListener(OnConfirmed);
            _controller.OnSymbolFailed.RemoveListener(OnFailed);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: symbol successfully recognized
    // ────────────────────────────────────────────────────────────
    private void OnConfirmed()
    {
        // PlayOneShot is sufficient — these are short stingers that
        // don't need parameter updates or manual lifetime control.
        if (!_confirmedFmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_confirmedFmodEvent);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: symbol not recognized
    // ────────────────────────────────────────────────────────────
    private void OnFailed()
    {
        if (!_failedFmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_failedFmodEvent);
    }
}