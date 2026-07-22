using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterController : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")] protected InputActionAsset _inputActions;
    [SerializeField, FoldoutGroup("References")] protected Transform _characterObject;
    [SerializeField, FoldoutGroup("References")] protected Transform _orientation;
    
    [SerializeField, FoldoutGroup("Data")] protected CharacterDataSO _characterData;
    
    [ShowInInspector, FoldoutGroup("Testing")] protected Vector3 _movementDirection;
    [ShowInInspector, FoldoutGroup("Testing")] public bool IsGrounded {get; private set;}
    
    public InputAction MoveAction {get; private set;}
    public InputAction JumpAction {get; private set;}
    
    
    protected virtual void OnEnable()
    {
        _inputActions.FindActionMap("Player").Enable();
    }
    
    protected virtual void OnDisable()
    {
        _inputActions.FindActionMap("Player").Disable();
    }
    
    protected virtual void Awake()
    {
        MoveAction = InputSystem.actions.FindAction("Move");
        JumpAction = InputSystem.actions.FindAction("Jump");
    }
    

    protected virtual void Update()
    {
        GetMovementDirection();
        
        IsGrounded = CheckGrounded();
    }

    private void GetMovementDirection()
    {
        Vector2 moveInputValue =  MoveAction.ReadValue<Vector2>();
        _movementDirection = (_orientation.forward * moveInputValue.y + _orientation.right * moveInputValue.x).normalized;
    }
    
    private bool CheckGrounded()
    {
        Vector3 halfExtents = new Vector3(_characterData.CharacterWidth * 0.95f, 0.05f, _characterData.CharacterLength * 0.95f);
        float maxDistance = (_characterData.CharacterHeight * 0.5f) + _characterData.CastCushion - halfExtents.y;
        
        bool hit = Physics.BoxCast(_characterObject.position, halfExtents, Vector3.down, out RaycastHit hitInfo, _characterObject.rotation, maxDistance, _characterData.GroundLayer);
        return hit;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        
        Vector3 halfExtents = new Vector3(_characterData.CharacterWidth * 0.95f, 0.05f, _characterData.CharacterLength * 0.95f);
        float maxDistance = (_characterData.CharacterHeight * 0.5f) + _characterData.CastCushion - halfExtents.y;
        
        // Draw a wireframe box at the destination where the check finishes
        Vector3 endPosition = _characterObject.position + (Vector3.down * maxDistance);
        
        // Matrix trick to ensure the gizmo rotates with the player character
        Gizmos.matrix = Matrix4x4.TRS(endPosition, _characterObject.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
        
    }
#endif
    
}
