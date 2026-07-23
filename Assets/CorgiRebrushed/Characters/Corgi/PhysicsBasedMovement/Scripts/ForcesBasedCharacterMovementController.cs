using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterControllerManager))]
[RequireComponent(typeof(Rigidbody))]
public class ForcesBasedCharacterMovementController : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] private CharacterControllerManager _manager;
    [SerializeField][FoldoutGroup("References")] private Rigidbody _rigidbody;

    //Sound Jump / Land
    [FoldoutGroup("Audio")] public UnityEvent OnJump;
    [FoldoutGroup("Audio")] public UnityEvent OnLand;
    private bool _wasGrounded;
    
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

    private void FixedUpdate()
    {
        //Sound - Land Detection
        if (!_wasGrounded && _manager.IsGrounded)
        {
            OnLand?.Invoke();
        }
        _wasGrounded = _manager.IsGrounded;

        ApplyForceToHorizontalMovement();
        CapVelocity();
    }

    private void ApplyForceToHorizontalMovement()
    {
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
    
    [Button("JumpTest")]
    private void Jump()
    {
        if(!_manager.IsGrounded) return;
        _rigidbody.AddForce(_manager.CharacterObject.up * 10, ForceMode.Impulse);
        
        //TODO Implementar sonido de salto
        OnJump?.Invoke();
    }
    
    [Button("DashTest")]
    private void Dash()
    {
        Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
        
        if (_manager.MovementDirection == Vector3.zero)
        {
            _rigidbody.AddForce(_manager.CharacterObject.forward * 10, ForceMode.Impulse);
            return;
        }
        
        _rigidbody.AddForce(_manager.MovementDirection * 10, ForceMode.Impulse);
    }
}
