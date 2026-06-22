using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Scriptable Objects/CharacterDataSO")]
[InlineEditor]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField][FoldoutGroup("Movement Variables")] public float Acceleration { get; private set; }
    [field: SerializeField][FoldoutGroup("Movement Variables")] public float Desacceleration { get; private set; }
    [field: SerializeField][FoldoutGroup("Movement Variables")] public float MaxSpeed { get; private set; }
    
    [field: SerializeField][FoldoutGroup("Grounded Variables")] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField][FoldoutGroup("Grounded Variables")] public float CharacterHeight { get; private set; }
    [field: SerializeField][FoldoutGroup("Grounded Variables")] public float CharacterLength { get; private set; }
    [field: SerializeField][FoldoutGroup("Grounded Variables")] public float CharacterWidth { get; private set; }
    
    [Tooltip("How far below the character's feet to check for ground.")]
    [field: SerializeField][FoldoutGroup("Grounded Variables")] public float CastCushion { get; private set; }
}
