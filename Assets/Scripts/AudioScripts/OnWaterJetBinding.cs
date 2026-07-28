using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Corgi Audio/On Water Jet Binding")]
public class OnWaterJetBinding : MonoBehaviour
{
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;

    private void Awake()
    {
        var waterJump = FindObjectOfType<WaterJump>();
        if (waterJump != null)
            waterJump.OnWaterJetActivated.AddListener(OnWaterJet);
    }

    private void OnDestroy()
    {
        var waterJump = FindObjectOfType<WaterJump>();
        if (waterJump != null)
            waterJump.OnWaterJetActivated.RemoveListener(OnWaterJet);
    }

    private void OnWaterJet()
    {
        if (!_fmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_fmodEvent);
    }
}
