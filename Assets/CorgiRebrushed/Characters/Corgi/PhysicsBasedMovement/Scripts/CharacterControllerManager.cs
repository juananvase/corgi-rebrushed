using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CharacterControllerManager : MonoBehaviour
{
    [field: SerializeField][FoldoutGroup("References")] public InputActionAsset InputActions { get; private set; }
    [field: SerializeField][FoldoutGroup("References")] public Transform CharacterObject { get; private set; }
    [field: SerializeField][FoldoutGroup("References")] public Transform Orientation { get; private set; }
    
    [field: SerializeField][FoldoutGroup("Data")] public CharacterDataSO CharacterData { get; private set; }
    
    [ShowInInspector][FoldoutGroup("Testing")] public Vector3 MovementDirection {get; private set;}
    [ShowInInspector][FoldoutGroup("Testing")] public bool IsGrounded {get; private set;}
    
    public InputAction MoveAction {get; private set;}

    // Event that is invoked when the character takes a footstep. Can be used to trigger footstep sounds or other effects.
    [FoldoutGroup("Audio")] public UnityEvent OnFootstep;
    
    // Timer to control footstep audio interval
    private float _footstepTimer;

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
    }

    private void Update()
    {
        GetMovementDirection();
        
        IsGrounded = CheckGrounded();

        // Footstep audio
        if (IsGrounded && MovementDirection.magnitude > 0.1f)
        {
            float interval = Mathf.Lerp(0.55f, 0.3f, MovementDirection.magnitude);
            _footstepTimer += Time.deltaTime;
            if (_footstepTimer >= interval)
            {
                _footstepTimer = 0f;
                OnFootstep.Invoke();
            }
        }
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
