using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Scriptable Objects/CharacterDataSO")]
[InlineEditor]
public class CharacterDataSO : ScriptableObject
{
    [Header("Movement Variables")]
    [field: SerializeField] public float Acceleration { get; private set; }
    [field: SerializeField] public float Desacceleration { get; private set; }
    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }
    
    [Header("Jump Variables")]
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float FallMultiplier { get; private set; }
    [field: SerializeField] public float LowJumpMultiplier { get; private set; }
    
    [Header("Grounded Variables")]
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public float CharacterHeight { get; private set; }
    [field: SerializeField] public float CharacterLength { get; private set; }
    [field: SerializeField] public float CharacterWidth { get; private set; }
    
    [Tooltip("How far below the character's feet to check for ground.")]
    [field: SerializeField] public float CastCushion { get; private set; }
}
