using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

[AddComponentMenu("Corgi Audio/On Jump Binding")]
public class OnJumpBinding : MonoBehaviour
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
            _controller.OnJump.AddListener(OnJumpRaised);
    }

    private void OnDisable()
    {
        if (_controller != null)
            _controller.OnJump.RemoveListener(OnJumpRaised);
    }

    private void OnJumpRaised()
    {
        if (_fmodEvent.IsNull) return;
        RuntimeManager.PlayOneShot(_fmodEvent);
    }
}