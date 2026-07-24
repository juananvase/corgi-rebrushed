using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

[AddComponentMenu("Corgi Audio/On Land Binding")]
public class OnLandBinding : MonoBehaviour
{
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;

    private ForcesBasedCharacterMovementController _controller;

    private void Awake()
    {
        _controller = FindObjectOfType<ForcesBasedCharacterMovementController>();
    }

    private void OnEnable()
    {
        if (_controller != null)
            _controller.OnLand.AddListener(OnLandRaised);
    }

    private void OnDisable()
    {
        if (_controller != null)
            _controller.OnLand.RemoveListener(OnLandRaised);
    }

    private void OnLandRaised()
    {
        if (_fmodEvent.IsNull) return;
        RuntimeManager.PlayOneShot(_fmodEvent);
    }
}
