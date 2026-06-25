using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRotationController : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] private CharacterControllerManager _manager;
    [SerializeField][FoldoutGroup("References")] private Transform _cameraTransform;
    
    [ShowInInspector][FoldoutGroup("Testing")] private Vector3 _viewDirection;
    
    
    private void OnEnable()
    {
        _manager.InputActions.FindActionMap("Player").Enable();
    }
    
    private void OnDisable()
    {
        _manager.InputActions.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        Rotate();
    }
    
    private void Rotate()
    {
        _viewDirection = (transform.position - new Vector3(_cameraTransform.transform.position.x, transform.position.y, _cameraTransform.transform.position.z)).normalized;
        _manager.Orientation.forward = _viewDirection;

        if(_manager.MovementDirection  == Vector3.zero) return;
        
        _manager.CharacterObject.forward = Vector3.Slerp(_manager.CharacterObject.forward, _manager.MovementDirection, Time.deltaTime * _manager.CharacterData.RotationSpeed);
    }
    
}
