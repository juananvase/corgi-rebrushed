using GameEvents;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "AbilitiesDataSO", menuName = "Scriptable Objects/AbilitiesDataSO")]
[InlineEditor]
public class AbilitiesDataSO : ScriptableObject
{
    [field: SerializeField, FoldoutGroup("General")] public LayerMask HitLayer { get; private set; }
    //[field: SerializeField, FoldoutGroup("General")] public TransformEventAsset OnHitCamaraShake { get; private set; }
    
    
    [field: SerializeField, FoldoutGroup("Water Impulse")] public float WaterJumpCoolDownDuration { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public float WaterJumpImpulseForce { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public Vector3 WaterJumpAttackBoxHalfExtents { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public Vector3 WaterJumpAttackOffset { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public TweenSettings<Vector3> WaterCascadeInScaleTweenSettings { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public TweenSettings<Vector3> WaterCascadeOutScaleTweenSettings { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse")] public float WaterImpulseDamage { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse VFX")]  public VisualEffect WaterCascadeVfxPrefab { get; private set; }
    [field: SerializeField, FoldoutGroup("Water Impulse VFX")]  public Vector3 VfxOffset { get; private set; }
    
    [field: SerializeField, FoldoutGroup("Tree Fall")] public float TreeFallCoolDownDuration { get; private set; }
    [field: SerializeField, FoldoutGroup("Tree Fall")] public GameObject SeedPrefab { get; private set; }
    [field: SerializeField, FoldoutGroup("Tree Fall")] public GameObject TreePrefab { get; private set; }
    [field: SerializeField, FoldoutGroup("Tree Fall")] public TweenSettings<Vector3> TreeSpawnInScaleTweenSettings { get; private set; }
    [field: SerializeField, FoldoutGroup("Tree Fall")] public TweenSettings TreeFallTweenSettings { get; private set; }
    [field: SerializeField, FoldoutGroup("Tree Fall")] public ShakeSettings TreeShakeTweenSettings { get; private set; }
    [field: SerializeField, FoldoutGroup("Tree Fall")] public float TreeDestroyTime { get; private set; }
    [field: SerializeField, FoldoutGroup("Tree Fall")] public string[] TreeFallHitLayers { get; private set; }
    [field: SerializeField, FoldoutGroup("Tree Fall")] public float TreeFallDamage { get; private set; }
    
    [field: SerializeField, FoldoutGroup("Chilli Explosion")] public float ChilliExplosionCoolDownDuration { get; private set; }
    [field: SerializeField, FoldoutGroup("Chilli Explosion")] public GameObject ChilliPrefab { get; private set; }
    [field: SerializeField, FoldoutGroup("Chilli Explosion")] public TweenSettings<Vector3> ChilliInScaleTweenSettings { get; private set; }
    [field: SerializeField, FoldoutGroup("Chilli Explosion")] public ShakeSettings ChilliShakeTweenSettings { get; private set; }
    [field: SerializeField, FoldoutGroup("Chilli Explosion")] public Vector3 ChilliExplosionAttackBoxHalfExtents { get; private set; }
    [field: SerializeField, FoldoutGroup("Chilli Explosion")] public Vector3 ChilliExplosionAttackOffset { get; private set; }
    [field: SerializeField, FoldoutGroup("Chilli Explosion")] public float ChilliExplosionDamage { get; private set; }
    
    [field: SerializeField, FoldoutGroup("Chilli boost")] public float ChilliSateDuration { get; private set; }
    [field: SerializeField, FoldoutGroup("Chilli boost")] public float ChilliAcceleration { get; private set; }
    [field: SerializeField, FoldoutGroup("Chilli boost")] public float ChilliMaxSpeed { get; private set; }
    [field: SerializeField, FoldoutGroup("Chilli boost")] public Material ChilliCorgiMaterial { get; private set; }
    [field: SerializeField, FoldoutGroup("Chilli boost")] public Material CommonCorgiMaterial { get; private set; }
    
}
