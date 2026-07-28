using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

[AddComponentMenu("Corgi Audio/On Painting Mode Binding")]
public class OnPaintingModeBinding : MonoBehaviour
{
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _enterFmodEvent;
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _exitFmodEvent;

    private PaintingModeController _controller;

    private void Awake()
    {
        _controller = FindObjectOfType<PaintingModeController>();
    }

    private void OnEnable()
    {
        if (_controller != null)
        {
            _controller.OnPaintingModeEnter.AddListener(OnEnter);
            _controller.OnPaintingModeExit.AddListener(OnExit);
        }
    }

    private void OnDisable()
    {
        if (_controller != null)
        {
            _controller.OnPaintingModeEnter.RemoveListener(OnEnter);
            _controller.OnPaintingModeExit.RemoveListener(OnExit);
        }
    }

    private void OnEnter()
    {
        // PaintingState parameter controls LPF + Reverb on the Master bus in FMOD
        RuntimeManager.StudioSystem.setParameterByName("PaintingState", 1f);
        if (!_enterFmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_enterFmodEvent);
    }

    private void OnExit()
    {
        RuntimeManager.StudioSystem.setParameterByName("PaintingState", 0f);
        if (!_exitFmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_exitFmodEvent);
    }
}
