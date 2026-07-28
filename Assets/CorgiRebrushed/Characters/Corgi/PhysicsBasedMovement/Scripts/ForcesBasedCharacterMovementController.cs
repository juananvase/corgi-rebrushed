using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events; //Required for Audio

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class ForcesBasedCharacterMovementController : CharacterController
{
    [SerializeField][FoldoutGroup("References")] private Rigidbody _rigidbody;
    // Audio — footstep
    private float _footstepTimer;
    private bool _wasMoving;
    private bool _wasGrounded = true; // starts true so it doesn't trigger on scene load
    public UnityEvent<float> OnFootstep;
    public UnityEvent OnJump;
    public UnityEvent OnLand;
    
    // End Audio
    
    [ShowInInspector, FoldoutGroup("Testing")] public float CurrentAcceleration { get; set; }
    [ShowInInspector, FoldoutGroup("Testing")] public float CurrentMaxSpeed { get; set; }
    
    // Set briefly by external abilities (e.g. WaterJump) so their impulse isn't immediately
    // cut short by the low-jump-multiplier logic below, which only expects the Jump button.
    public bool ExternalBoostActive { get; set; }

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
        
        CurrentAcceleration = CharacterData.Acceleration;
        CurrentMaxSpeed = CharacterData.MaxSpeed;
    }

    private void Start()
    {
        JumpAction.performed += Jump;
    }

    private void FixedUpdate()
    {
        CustomFalling();
        ApplyForceToHorizontalMovement();
        CapVelocity();
        //AUDIO - Footstep audio
        bool isMoving = _movementDirection.magnitude > 0.1f;
        if (IsGrounded && isMoving)
        {
            float interval = Mathf.Lerp(0.3f, 0.55f, _movementDirection.magnitude);

            // If the player just started moving, force the first footstep immediately
            if (!_wasMoving)
            {
                _footstepTimer = interval;
            }

            _footstepTimer += Time.fixedDeltaTime;
            if (_footstepTimer >= interval)
            {
                _footstepTimer = 0f;
                OnFootstep?.Invoke(_movementDirection.magnitude);
            }
        }
        _wasMoving = isMoving; // Audio - Land detection
        if (IsGrounded && !_wasGrounded)
        {
            OnLand?.Invoke();
        }
        _wasGrounded = IsGrounded;

        // End Audio

    }

    private void ApplyForceToHorizontalMovement()
    {
        if (_movementDirection == Vector3.zero)
        {
            Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
            _rigidbody.AddForce(horizontalVelocity * -CharacterData.Desacceleration, ForceMode.Force);
            return;
        }
        
        _rigidbody.AddForce(_movementDirection * CurrentAcceleration, ForceMode.Force);
    }
    
    private void CapVelocity()
    {
        Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);

        if (horizontalVelocity.magnitude > CurrentMaxSpeed)
        {
            Vector3 cappedVelocity = horizontalVelocity.normalized * CurrentMaxSpeed;
            _rigidbody.linearVelocity = new Vector3(cappedVelocity.x, _rigidbody.linearVelocity.y, cappedVelocity.z);
        }
    }
    
    private void Jump(InputAction.CallbackContext context)
    {
        if(!IsGrounded) return;
        
        _rigidbody.AddForce(_characterObject.up * CharacterData.JumpForce, ForceMode.Impulse);
        
        //TODO Implementar sonido de salto
        //Audio
        OnJump?.Invoke();
        //End Audio

    }

    private void CustomFalling()
    {
        if(IsGrounded) return;
        
        if (_rigidbody.linearVelocity.y < 0)
        {
            _rigidbody.AddForce(_characterObject.up * (Physics.gravity.y * (CharacterData.FallMultiplier - 1)), ForceMode.Force);
            return;
        }
        
        if (_rigidbody.linearVelocity.y > 0 && !JumpAction.IsPressed() && !ExternalBoostActive)
        {
            _rigidbody.AddForce(_characterObject.up * (Physics.gravity.y * (CharacterData.LowJumpMultiplier - 1)), ForceMode.Force);
        }
    }
}
