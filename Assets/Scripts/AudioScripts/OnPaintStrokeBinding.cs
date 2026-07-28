using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

[AddComponentMenu("Corgi Audio/On Paint Stroke Binding")]
public class OnPaintStrokeBinding : MonoBehaviour
{
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;

    private PaintCanvas _canvas;
    private FMOD.Studio.EventInstance _activeBrushInstance;
    private bool _isPlaying;

    private void Awake()
    {
        _canvas = FindObjectOfType<PaintCanvas>();
    }

    private void OnEnable()
    {
        if (_canvas != null)
        {
            _canvas.OnPaintStrokeBegin.AddListener(OnStrokeBegin);
            _canvas.OnPaintStrokeEnd.AddListener(OnStrokeEnd);
            _canvas.OnPaintStroke.AddListener(OnStrokeUpdate);
        }
    }

    private void OnDisable()
    {
        if (_canvas != null)
        {
            _canvas.OnPaintStrokeBegin.RemoveListener(OnStrokeBegin);
            _canvas.OnPaintStrokeEnd.RemoveListener(OnStrokeEnd);
            _canvas.OnPaintStroke.RemoveListener(OnStrokeUpdate);
        }
    }

    private void OnStrokeBegin()
    {
        if (_fmodEvent.IsNull || _isPlaying) return;

        _activeBrushInstance = RuntimeManager.CreateInstance(_fmodEvent);
        _activeBrushInstance.start();
        _isPlaying = true;
    }

    private void OnStrokeUpdate(float speed)
    {
        if (!_isPlaying || !_activeBrushInstance.hasHandle()) return;
        _activeBrushInstance.setParameterByName("BrushSpeed", speed);
    }

    private void OnStrokeEnd()
    {
        if (!_isPlaying) return;

        _activeBrushInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _activeBrushInstance.release();
        _isPlaying = false;
    }

    private void OnDestroy()
    {
        if (_isPlaying && _activeBrushInstance.hasHandle())
        {
            _activeBrushInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _activeBrushInstance.release();
        }
    }
}
