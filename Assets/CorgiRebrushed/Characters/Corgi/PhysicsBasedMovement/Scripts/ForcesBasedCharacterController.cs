using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ForcesBasedCharacterController : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] private Rigidbody _rigidbody;
    [SerializeField][FoldoutGroup("References")] private InputActionAsset _inputActions;
    
    [SerializeField][FoldoutGroup("Data")] private CharacterDataSO _characterData;
    
    [ShowInInspector] private Vector3 _forceDirection;
    [ShowInInspector] private bool _isGrounded;
    
    private InputAction _moveAction;
    
    private void OnEnable()
    {
        _inputActions.FindActionMap("Player").Enable();
    }
    
    private void OnDisable()
    {
        _inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        GetForceDirection();
        _isGrounded = CheckGrounded();
    }

    private void FixedUpdate()
    {
        ApplyForceToHorizontalMovement();
        CapVelocity();
    }
    
    private void GetForceDirection()
    {
        Vector2 moveInputValue =  _moveAction.ReadValue<Vector2>();
        _forceDirection = new Vector3(moveInputValue.x, 0, moveInputValue.y).normalized;
    }

    private void ApplyForceToHorizontalMovement()
    {
        if (_forceDirection == Vector3.zero)
        {
            Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
            _rigidbody.AddRelativeForce(horizontalVelocity * -_characterData.Desacceleration, ForceMode.Force);
            return;
        }
        
        _rigidbody.AddRelativeForce(_forceDirection * _characterData.Acceleration, ForceMode.Force);
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

    private bool CheckGrounded()
    {
        Vector3 halfExtents = new Vector3(_characterData.CharacterWidth * 0.95f, 0.05f, _characterData.CharacterLength * 0.95f);
        float maxDistance = (_characterData.CharacterHeight * 0.5f) + _characterData.CastCushion - halfExtents.y;
        
        bool hit = Physics.BoxCast(transform.position, halfExtents, Vector3.down, out RaycastHit hitInfo, transform.rotation, maxDistance, _characterData.GroundLayer);
        return hit;
    }
    
    [Button("JumpTest")]
    private void Jump()
    {
        if(!_isGrounded) return;
        _rigidbody.AddForce(transform.up * 10, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        
        Vector3 halfExtents = new Vector3(_characterData.CharacterWidth * 0.95f, 0.05f, _characterData.CharacterLength * 0.95f);
        float maxDistance = (_characterData.CharacterHeight * 0.5f) + _characterData.CastCushion - halfExtents.y;
        
        // Draw a wireframe box at the destination where the check finishes
        Vector3 endPosition = transform.position + (Vector3.down * maxDistance);
        
        // Matrix trick to ensure the gizmo rotates with the player character
        Gizmos.matrix = Matrix4x4.TRS(endPosition, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
        
    }
}
