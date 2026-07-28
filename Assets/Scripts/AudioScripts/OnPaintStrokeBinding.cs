using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays a looping brush sound while the player is painting.
/// - Creates a single FMOD event instance to avoid "Channel stolen" errors.
/// - Updates the "BrushSpeed" parameter every frame based on mouse movement speed.
/// - Starts on stroke begin, stops with fadeout on stroke end, and cleans up on destroy.
/// </summary>
[AddComponentMenu("Corgi Audio/On Paint Stroke Binding")]
public class OnPaintStrokeBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;    // looping brush event (e.g., Painting/Brush)

    // ────────────────────────────────────────────────────────────
    // Internal state
    // ────────────────────────────────────────────────────────────
    private PaintCanvas _canvas;
    private FMOD.Studio.EventInstance _activeBrushInstance; // single instance reused for the entire stroke
    private bool _isPlaying;                                // prevents overlapping CreateInstance calls

    // ────────────────────────────────────────────────────────────
    // Lifecycle: cache the canvas reference
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        // Safe to use FindObjectOfType — only one PaintCanvas per scene.
        _canvas = FindObjectOfType<PaintCanvas>();
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to stroke events
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_canvas != null)
        {
            _canvas.OnPaintStrokeBegin.AddListener(OnStrokeBegin);
            _canvas.OnPaintStrokeEnd.AddListener(OnStrokeEnd);
            _canvas.OnPaintStroke.AddListener(OnStrokeUpdate);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_canvas != null)
        {
            _canvas.OnPaintStrokeBegin.RemoveListener(OnStrokeBegin);
            _canvas.OnPaintStrokeEnd.RemoveListener(OnStrokeEnd);
            _canvas.OnPaintStroke.RemoveListener(OnStrokeUpdate);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: stroke started — create and start the loop
    // ────────────────────────────────────────────────────────────
    private void OnStrokeBegin()
    {
        // Guard: skip if no event assigned or already playing.
        if (_fmodEvent.IsNull || _isPlaying) return;

        // CreateInstance is used instead of PlayOneShot so we can
        // keep a handle to the instance and update parameters / stop it later.
        _activeBrushInstance = RuntimeManager.CreateInstance(_fmodEvent);
        _activeBrushInstance.start();
        _isPlaying = true;
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: every frame while painting — update brush speed
    // ────────────────────────────────────────────────────────────
    private void OnStrokeUpdate(float speed)
    {
        // Guard: skip if no active instance or handle is invalid.
        if (!_isPlaying || !_activeBrushInstance.hasHandle()) return;

        // speed is already normalized 0…1 by PaintCanvas.
        _activeBrushInstance.setParameterByName("BrushSpeed", speed);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: stroke ended — stop with fadeout and release
    // ────────────────────────────────────────────────────────────
    private void OnStrokeEnd()
    {
        if (!_isPlaying) return;

        // ALLOWFADEOUT lets the tail / release of the event play naturally.
        _activeBrushInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _activeBrushInstance.release();
        _isPlaying = false;
    }

    // ────────────────────────────────────────────────────────────
    // Cleanup: force-stop if the object is destroyed mid-stroke
    // ────────────────────────────────────────────────────────────
    private void OnDestroy()
    {
        if (_isPlaying && _activeBrushInstance.hasHandle())
        {
            // IMMEDIATE because OnDestroy runs during scene unload / teardown —
            // there is no time for a graceful fadeout.
            _activeBrushInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _activeBrushInstance.release();
        }
    }
}