using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays audio when the Tree ability spawns.
/// Listens to TreeFall.OnTreeSpawnActivated UnityEvent and fires a one-shot FMOD event.
/// </summary>
[AddComponentMenu("Corgi Audio/On Tree Binding")]
public class OnTreeBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // tree spawn sound (e.g., Abilities/TreeSpawn)

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to the TreeFall spawn event
    // Uses Awake/OnDestroy because TreeFall is always active on the
    // same GameObject — no enable/disable toggling expected.
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        var treeFall = FindObjectOfType<TreeFall>();
        if (treeFall != null)
            treeFall.OnTreeSpawnActivated.AddListener(OnTreeSpawn);
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDestroy()
    {
        var treeFall = FindObjectOfType<TreeFall>();
        if (treeFall != null)
            treeFall.OnTreeSpawnActivated.RemoveListener(OnTreeSpawn);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: tree spawn activated — play the audio event
    // ────────────────────────────────────────────────────────────
    private void OnTreeSpawn()
    {
        // PlayOneShot is sufficient — a short one-shot stinger that
        // doesn't need parameter updates or manual lifetime control.
        if (!_fmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_fmodEvent);
    }
}