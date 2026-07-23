using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CorgiAnimationDriver : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")] private Animator _animator;
    [SerializeField, FoldoutGroup("References")] private CharacterController _characterController;
    [SerializeField, FoldoutGroup("References")] private Rigidbody _rigidbody;

    [SerializeField, FoldoutGroup("Data")] private float _speedBlendMax = 8f;
    [SerializeField, FoldoutGroup("Data")] private float _maxLandingVerticalSpeed = 0.5f;

    private bool _wasGrounded = true;

    private static readonly int SpeedParam = Animator.StringToHash("Speed");
    private static readonly int GroundedParam = Animator.StringToHash("Grounded");
    private static readonly int JumpParam = Animator.StringToHash("Jump");

    private void Awake()
    {
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    { 
        _characterController.JumpAction.performed += OnJumpPerformed;
    }

    private void OnDestroy()
    {
        _characterController.JumpAction.performed -= OnJumpPerformed;
    }

    private void Update()
    {
        Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
        float speed01 = Mathf.Clamp01(horizontalVelocity.magnitude / _speedBlendMax);
        _animator.SetFloat(SpeedParam, speed01);
        
        bool grounded = _characterController.IsGrounded && Mathf.Abs(_rigidbody.linearVelocity.y) <= _maxLandingVerticalSpeed;
        _animator.SetBool(GroundedParam, grounded);
        _wasGrounded = grounded;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (_characterController.IsGrounded)
            _animator.SetTrigger(JumpParam);
    }
}
