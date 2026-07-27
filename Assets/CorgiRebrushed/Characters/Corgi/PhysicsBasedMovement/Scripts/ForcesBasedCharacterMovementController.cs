using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
//Audio
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class ForcesBasedCharacterMovementController : CharacterController
{
    [SerializeField][FoldoutGroup("References")] private Rigidbody _rigidbody;

    //AUDIO - 
    [FoldoutGroup("Audio")] public UnityEvent OnJump;
    [FoldoutGroup("Audio")] public UnityEvent OnLand;
    [FoldoutGroup("Audio")] public UnityEvent<float> OnFootstep;

    private bool _wasGrounded;
    private float _footstepTimer;
    private bool _wasMoving;
    //
    
    // Set briefly by external abilities (e.g. WaterJump) so their impulse isn't immediately
    // cut short by the low-jump-multiplier logic below, which only expects the Jump button.
    public bool ExternalBoostActive { get; set; }

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        JumpAction.performed += Jump;
    }

    private void FixedUpdate()
    {
       //AUDIO - Land detection
       if (!_wasGrounded && IsGrounded)
            OnLand?.Invoke();
        _wasGrounded = IsGrounded;

        //AUDIO - Footsteps audio
        bool isMoving = _movementDirection.magnitude > 0.1f;
        if (IsGrounded && isMoving)
        {
            float interval = Mathf.Lerp(0.3f, 0.59f, _movementDirection.magnitude);
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
        _wasMoving = isMoving;
        //AUDIO - End footsteps audio

        CustomFalling();
        ApplyForceToHorizontalMovement();
        CapVelocity();
    }

    private void ApplyForceToHorizontalMovement()
    {
        if (_movementDirection == Vector3.zero)
        {
            Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
            _rigidbody.AddForce(horizontalVelocity * -_characterData.Desacceleration, ForceMode.Force);
            return;
        }
        
        _rigidbody.AddForce(_movementDirection * _characterData.Acceleration, ForceMode.Force);
    }
    
    private void CapVelocity()
    {
        Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);

        if (horizontalVelocity.magnitude > _characterData.MaxSpeed)
        {
            Vector3 cappedVelocity = horizontalVelocity.normalized * _characterData.MaxSpeed;
            _rigidbody.linearVelocity = new Vector3(cappedVelocity.x, _rigidbody.linearVelocity.y, cappedVelocity.z);
        }
    }
    
    private void Jump(InputAction.CallbackContext context)
    {
        if(!IsGrounded) return;
        
        _rigidbody.AddForce(_characterObject.up * _characterData.JumpForce, ForceMode.Impulse);
        
        //TODO AUDIO - Implementar sonido de salto
        OnJump?.Invoke();

    }

    private void CustomFalling()
    {
        if(IsGrounded) return;
        
        if (_rigidbody.linearVelocity.y < 0)
        {
            _rigidbody.AddForce(_characterObject.up * (Physics.gravity.y * (_characterData.FallMultiplier - 1)), ForceMode.Force);
            return;
        }
        
        if (_rigidbody.linearVelocity.y > 0 && !JumpAction.IsPressed() && !ExternalBoostActive)
        {
            _rigidbody.AddForce(_characterObject.up * (Physics.gravity.y * (_characterData.LowJumpMultiplier - 1)), ForceMode.Force);
        }
    }
    
}
