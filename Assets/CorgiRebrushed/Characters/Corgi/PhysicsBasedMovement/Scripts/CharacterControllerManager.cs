using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerManager : MonoBehaviour
{
    [field: SerializeField, FoldoutGroup("References")] public InputActionAsset InputActions { get; private set; }
    [field: SerializeField, FoldoutGroup("References")] public Transform CharacterObject { get; private set; }
    [field: SerializeField, FoldoutGroup("References")] public Transform Orientation { get; private set; }
    
    [field: SerializeField, FoldoutGroup("Data")] public CharacterDataSO CharacterData { get; private set; }
    
    [ShowInInspector, FoldoutGroup("Testing")] public Vector3 MovementDirection {get; private set;}
    [ShowInInspector, FoldoutGroup("Testing")] public bool IsGrounded {get; private set;}
    
    public InputAction MoveAction {get; private set;}
    public InputAction JumpAction {get; private set;}
    
    
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }
    
    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }
    
    private void Awake()
    {
        MoveAction = InputSystem.actions.FindAction("Move");
        JumpAction = InputSystem.actions.FindAction("Jump");
    }
    

    private void Update()
    {
        GetMovementDirection();
        
        IsGrounded = CheckGrounded();
    }

    private void GetMovementDirection()
    {
        Vector2 moveInputValue =  MoveAction.ReadValue<Vector2>();
        MovementDirection = (Orientation.forward * moveInputValue.y + Orientation.right * moveInputValue.x).normalized;
    }
    
    private bool CheckGrounded()
    {
        Vector3 halfExtents = new Vector3(CharacterData.CharacterWidth * 0.95f, 0.05f, CharacterData.CharacterLength * 0.95f);
        float maxDistance = (CharacterData.CharacterHeight * 0.5f) + CharacterData.CastCushion - halfExtents.y;
        
        bool hit = Physics.BoxCast(CharacterObject.position, halfExtents, Vector3.down, out RaycastHit hitInfo, CharacterObject.rotation, maxDistance, CharacterData.GroundLayer);
        return hit;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        
        Vector3 halfExtents = new Vector3(CharacterData.CharacterWidth * 0.95f, 0.05f, CharacterData.CharacterLength * 0.95f);
        float maxDistance = (CharacterData.CharacterHeight * 0.5f) + CharacterData.CastCushion - halfExtents.y;
        
        // Draw a wireframe box at the destination where the check finishes
        Vector3 endPosition = CharacterObject.position + (Vector3.down * maxDistance);
        
        // Matrix trick to ensure the gizmo rotates with the player character
        Gizmos.matrix = Matrix4x4.TRS(endPosition, CharacterObject.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
        
    }
    
}
