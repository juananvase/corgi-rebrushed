using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Connects CharacterControllerManager.OnFootstep to FMOD Player/Footstep.
/// Add this component to the AudioManager prefab.
/// Automatically finds the CharacterControllerManager — no manual drag required.
/// </summary>
[AddComponentMenu("Corgi Audio/On Footstep Binding")]
public class OnFootstepBinding : MonoBehaviour
{
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;

    private CharacterControllerManager _controller;

    private void Awake()
    {
        _controller = FindObjectOfType<CharacterControllerManager>();
    }

    private void OnEnable()
    {
        if (_controller != null)
            _controller.OnFootstep.AddListener(OnFootstepRaised);
    }

    private void OnDisable()
    {
        if (_controller != null)
            _controller.OnFootstep.RemoveListener(OnFootstepRaised);
    }

    private void OnFootstepRaised()
    {
        if (_fmodEvent.IsNull || _controller == null) return;

        float speed = _controller.MovementDirection.magnitude;
        RuntimeManager.StudioSystem.setParameterByName("PlayerSpeed", speed);
        RuntimeManager.PlayOneShot(_fmodEvent);
    }
}