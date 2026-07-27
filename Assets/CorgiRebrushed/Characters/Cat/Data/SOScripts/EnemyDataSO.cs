using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
[InlineEditor]
public class EnemyDataSO : ScriptableObject
{
    [field: SerializeField, FoldoutGroup("Layers")] public LayerMask GorundLayer { get; private set; }
    [field: SerializeField, FoldoutGroup("Layers")] public LayerMask PlayerLayer { get; private set; }

    [field: SerializeField, FoldoutGroup("Patrol")] public float PatrolRadius { get; private set; } = 10f;

    [field: SerializeField, FoldoutGroup("Combat")] public float AttackCooldown { get; private set; } = 1f;
    [field: SerializeField, FoldoutGroup("Combat")] public float MeleeAttackDamage { get; private set; } = 10f;
    [field: SerializeField, FoldoutGroup("Combat")] public Vector3 AttackHalfExtents { get; private set; }
    [field: SerializeField, FoldoutGroup("Combat")] public Vector3 AttackOffset { get; private set; }

    [field: SerializeField, FoldoutGroup("Detection Ranges")] public float VisionRange { get; private set; } = 20f;

    [field: SerializeField, FoldoutGroup("Detection Ranges")] public float EngagementRange { get; private set; } = 10f;
}
