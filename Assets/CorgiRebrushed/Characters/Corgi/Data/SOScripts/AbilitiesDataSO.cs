using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "AbilitiesDataSO", menuName = "Scriptable Objects/AbilitiesDataSO")]
[InlineEditor]
public class AbilitiesDataSO : ScriptableObject
{
    [field: SerializeField, FoldoutGroup("General")] public LayerMask EnemyLayer { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public float WaterJumpImpulseForce { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public Vector3 WaterJumpAttackBoxHalfExtents { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public Vector3 WaterJumpAttackOffset { get; private set; }
    
    [field: SerializeField, FoldoutGroup("Water Impulse")] public TweenSettings<Vector3> InScaleTweenSettings { get; private set; }
    
    [field: SerializeField, FoldoutGroup("Water Impulse")] public TweenSettings<Vector3> OutScaleTweenSettings { get; private set; }
    
    [field: SerializeField, FoldoutGroup("Water Impulse VFX")]  public VisualEffect WaterCascadeVfxPrefab { get; private set; }
    
    [field: SerializeField, FoldoutGroup("Water Impulse VFX")]  public Vector3 VfxOffset { get; private set; }
}
