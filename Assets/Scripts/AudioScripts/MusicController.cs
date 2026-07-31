using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays adaptive background music using an FMOD event with two parameters:
/// - musicState: 0 = Exploration, 1 = Combat
/// - depth: intensity/depth of the music (0…1, higher = more intense layers)
///
/// Starts in Exploration mode with a fixed depth on Start.
/// Future: connect musicState and depth to gameplay systems.
/// </summary>
[AddComponentMenu("Corgi Audio/Music Controller")]
public class MusicController : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _musicEvent;   // music event with musicState + depth params (e.g., Music/Exploration)

    // ────────────────────────────────────────────────────────────
    // Persistent music instance — created once, plays for lifetime
    // ────────────────────────────────────────────────────────────
    private FMOD.Studio.EventInstance _musicInstance;

    // ────────────────────────────────────────────────────────────
    // Lifecycle: start music in Exploration mode with fixed depth
    // ────────────────────────────────────────────────────────────
    private void Start()
    {
        if (_musicEvent.IsNull) return;

        _musicInstance = RuntimeManager.CreateInstance(_musicEvent);
        _musicInstance.setParameterByName("musicState", 0f);  // 0 = Exploration
        _musicInstance.setParameterByName("depth", 66.5f);      // fixed mid intensity
        _musicInstance.start();
    }

    // ────────────────────────────────────────────────────────────
    // Cleanup: stop with fadeout when the controller is destroyed
    // ────────────────────────────────────────────────────────────
    private void OnDestroy()
    {
        if (_musicInstance.hasHandle())
        {
            _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _musicInstance.release();
        }
    }

    // ────────────────────────────────────────────────────────────
    // Public API (future) — switch between music states
    // ────────────────────────────────────────────────────────────
    // public void SetMusicState(float state) { ... }
    // public void SetDepth(float value) { ... }
}