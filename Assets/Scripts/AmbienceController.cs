using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

[AddComponentMenu("Corgi Audio/Ambience Controller")]
public class AmbienceController : MonoBehaviour
{
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _ambienceEvent;

    private FMOD.Studio.EventInstance _ambienceInstance;

    private void Start()
    {
        if (!_ambienceEvent.IsNull)
        {
            _ambienceInstance = RuntimeManager.CreateInstance(_ambienceEvent);
            _ambienceInstance.start();
        }
    }

    private void OnDestroy()
    {
        if (_ambienceInstance.isValid())
        {
            _ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _ambienceInstance.release();
        }
    }
}
