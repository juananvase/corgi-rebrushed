using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays audio when the Water Jump ability is activated.
/// Listens to WaterJump.OnWaterJetActivated UnityEvent and fires a one-shot FMOD event.
/// </summary>
[AddComponentMenu("Corgi Audio/On Water Jet Binding")]
public class OnWaterJetBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // water jet / splash sound (e.g., Abilities/WaterJet)

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to the WaterJump event
    // Uses Awake/OnDestroy because WaterJump is always active on the
    // same GameObject — no enable/disable toggling expected.
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        var waterJump = FindObjectOfType<WaterJump>();
        if (waterJump != null)
            waterJump.OnWaterJetActivated.AddListener(OnWaterJet);
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDestroy()
    {
        var waterJump = FindObjectOfType<WaterJump>();
        if (waterJump != null)
            waterJump.OnWaterJetActivated.RemoveListener(OnWaterJet);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: water jet activated — play the audio event
    // ────────────────────────────────────────────────────────────
    private void OnWaterJet()
    {
        // PlayOneShot is sufficient — the water jet sound is a short
        // one-shot that doesn't need parameter updates or manual lifetime.
        if (!_fmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_fmodEvent);
    }
}