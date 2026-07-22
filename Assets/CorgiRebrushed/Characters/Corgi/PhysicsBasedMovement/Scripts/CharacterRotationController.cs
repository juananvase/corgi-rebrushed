using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRotationController : CharacterController
{
    [SerializeField][FoldoutGroup("References")] private Transform _cameraTransform;
    
    [ShowInInspector][FoldoutGroup("Testing")] private Vector3 _viewDirection;

    protected override void Update()
    {
        base.Update();
        Rotate();
    }
    
    private void Rotate()
    {
        _viewDirection = (transform.position - new Vector3(_cameraTransform.transform.position.x, transform.position.y, _cameraTransform.transform.position.z)).normalized;
        _orientation.forward = _viewDirection;

        if(_movementDirection  == Vector3.zero) return;
        
        _characterObject.forward = Vector3.Slerp(_characterObject.forward, _movementDirection, Time.deltaTime * _characterData.RotationSpeed);
    }
    
}
