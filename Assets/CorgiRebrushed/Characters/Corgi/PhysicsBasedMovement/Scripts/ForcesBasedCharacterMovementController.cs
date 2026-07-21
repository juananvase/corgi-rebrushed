using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterControllerManager))]
[RequireComponent(typeof(Rigidbody))]
public class ForcesBasedCharacterMovementController : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] private CharacterControllerManager _manager;
    [SerializeField][FoldoutGroup("References")] private Rigidbody _rigidbody;
    
    
    private void OnEnable()
    {
        _manager.InputActions.FindActionMap("Player").Enable();
    }
    
    private void OnDisable()
    {
        _manager.InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _manager.JumpAction.performed += Jump;
    }

    private void FixedUpdate()
    {
        CustomFalling();
        ApplyForceToHorizontalMovement();
        CapVelocity();
    }

    private void ApplyForceToHorizontalMovement()
    {
        if (!_manager.IsGrounded) return;
        
        if (_manager.MovementDirection == Vector3.zero)
        {
            Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
            _rigidbody.AddForce(horizontalVelocity * -_manager.CharacterData.Desacceleration, ForceMode.Force);
            return;
        }
        
        _rigidbody.AddForce(_manager.MovementDirection * _manager.CharacterData.Acceleration, ForceMode.Force);
    }
    
    private void CapVelocity()
    {
        Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);

        if (horizontalVelocity.magnitude > _manager.CharacterData.MaxSpeed)
        {
            Vector3 cappedVelocity = horizontalVelocity.normalized * _manager.CharacterData.MaxSpeed;
            _rigidbody.linearVelocity = new Vector3(cappedVelocity.x, _rigidbody.linearVelocity.y, cappedVelocity.z);
        }
    }
    
    private void Jump(InputAction.CallbackContext context)
    {
        if(!_manager.IsGrounded) return;
        
        _rigidbody.AddForce(_manager.CharacterObject.up * _manager.CharacterData.JumpForce, ForceMode.Impulse);
        
        //TODO Implementar sonido de salto
    }

    private void CustomFalling()
    {
        if(_manager.IsGrounded) return;
        
        if (_rigidbody.linearVelocity.y < 0)
        {
            _rigidbody.AddForce(_manager.CharacterObject.up * (Physics.gravity.y * (_manager.CharacterData.FallMultiplier - 1)), ForceMode.Force);
            return;
        }
        
        if (_rigidbody.linearVelocity.y > 0 && !_manager.JumpAction.IsPressed())
        {
            _rigidbody.AddForce(_manager.CharacterObject.up * (Physics.gravity.y * (_manager.CharacterData.LowJumpMultiplier - 1)), ForceMode.Force);
        }
    }
    
}
