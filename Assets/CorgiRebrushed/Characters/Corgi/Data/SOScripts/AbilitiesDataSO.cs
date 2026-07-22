using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilitiesDataSO", menuName = "Scriptable Objects/AbilitiesDataSO")]
[InlineEditor]
public class AbilitiesDataSO : ScriptableObject
{
    [field: SerializeField, FoldoutGroup("General")] public LayerMask EnemyLayer { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public float WaterJumpImpulseForce { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public Vector3 WaterJumpAttackBoxHalfExtents { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public Vector3 WaterJumpAttackOffset { get; private set; }
}
