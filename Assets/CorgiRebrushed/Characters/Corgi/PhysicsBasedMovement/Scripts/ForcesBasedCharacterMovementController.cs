using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events; //Required for audio

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class ForcesBasedCharacterMovementController : CharacterController
{
    [SerializeField][FoldoutGroup("References")] private Rigidbody _rigidbody;
    
    [ShowInInspector, FoldoutGroup("Testing")] public float CurrentAcceleration { get; set; }
    [ShowInInspector, FoldoutGroup("Testing")] public float CurrentMaxSpeed { get; set; }
    [ShowInInspector, FoldoutGroup("Testing")] private bool _canMove = true;
    
    //Audio
    [FoldoutGroup("Audio")] public UnityEvent<float> OnFootstep;
    [FoldoutGroup("Audio")] public UnityEvent OnJump;
    [FoldoutGroup("Audio")] public UnityEvent OnLand;
    private float _footstepTimer; /// Audio — footstep timing
    private bool _wasMoving;
    private bool _wasGrounded = true;
    // End Audio

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

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        CustomFalling();
        ApplyForceToHorizontalMovement();
        CapVelocity();

        // Audio — footstep system
        float horizontalSpeed = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z).magnitude;
        float speed = Mathf.Clamp01(horizontalSpeed / Mathf.Max(CurrentMaxSpeed, 0.01f));

        if (IsGrounded && speed > 0.01f)
        {
            _footstepTimer -= Time.fixedDeltaTime;

            // First step on movement start — play immediately
            if (!_wasMoving)
                _footstepTimer = 0f;

            if (_footstepTimer <= 0f)
            {
                // Inverted curve: faster steps when walking (0.3s), slower when galloping (0.55s)
                float interval = Mathf.Lerp(0.3f, 0.55f, speed);
                _footstepTimer = interval;

                OnFootstep?.Invoke(speed);
            }
        }
        _wasMoving = speed > 0.01f;

        if (!_wasGrounded && IsGrounded) // Audio — land detection
            OnLand?.Invoke();
        _wasGrounded = IsGrounded;
        // End Audio
    }

    public void AllowMovement()
    {
        _canMove = true;
    }

    public void RestrictMovement()
    {
        _canMove = false;
    }
    
    private void ApplyForceToHorizontalMovement()
    {
        if (_movementDirection == Vector3.zero || !_canMove)
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
        if(!IsGrounded || !_canMove) return;
        
        _rigidbody.AddForce(_characterObject.up * CharacterData.JumpForce, ForceMode.Impulse);
        
        // TODO
        // Audio
        OnJump?.Invoke();
        // End Audio

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
