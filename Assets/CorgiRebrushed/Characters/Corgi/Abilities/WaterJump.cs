using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

public class WaterJump : Abilitiy
{
    [SerializeField] [FoldoutGroup("References")] private Rigidbody _rigidbody;
    [SerializeField] [FoldoutGroup("References")] private VisualEffect _waterCascadeVfxPrefab;

    [SerializeField] [FoldoutGroup("Data")] private float _vfxGroundOffset = -0.95f;
    [SerializeField] [FoldoutGroup("Data")] private float _vfxActiveDuration = 3f;
    [SerializeField] [FoldoutGroup("Data")] private float _vfxGrowInDuration = 0.5f;
    [SerializeField] [FoldoutGroup("Data")] private float _vfxFadeOutBuffer = 1f;
    [SerializeField] [FoldoutGroup("Data")] private float _boostSuppressionDuration = 0.5f;

    // Cache an array for non-allocating physics checks (Max 20 targets per hit)
    private readonly Collider[] hitBuffer = new Collider[20];

    private void OnEnable()
    {
        _abilityEventAsset.OnInvoked.AddListener(WaterImpulse);
    }

    private void OnDisable()
    {
        _abilityEventAsset.OnInvoked.RemoveListener(WaterImpulse);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void WaterImpulse(ECorgiAbility context)
    {
        if (context != ECorgiAbility.Water) return;
        
        ApplyImpulseForce();
        PlayWaterJetVfx();
        ApplyDamage();
    }

    private void ApplyImpulseForce()
    {
        _rigidbody.AddForce(_characterObject.up * _abilitiesData.WaterJumpImpulseForce, ForceMode.Impulse);
    }
    

    private void PlayWaterJetVfx()
    {
        if (_waterCascadeVfxPrefab == null) return;

        Vector3 spawnPosition = _characterObject.position + Vector3.up * _vfxGroundOffset;
        VisualEffect vfxInstance = Instantiate(_waterCascadeVfxPrefab, spawnPosition, Quaternion.identity);

        Transform vfxTransform = vfxInstance.transform;
        Vector3 targetScale = vfxTransform.localScale;
        vfxTransform.localScale = Vector3.zero;
        Tween.Scale(vfxTransform, endValue: targetScale, duration: _vfxGrowInDuration);

        vfxInstance.Play();
        StartCoroutine(StopAndDestroyVfx(vfxInstance));
    }

    private IEnumerator StopAndDestroyVfx(VisualEffect vfxInstance)
    {
        yield return new WaitForSeconds(_vfxActiveDuration);
        vfxInstance.Stop();

        Transform vfxTransform = vfxInstance.transform;
        Tween.Scale(vfxTransform, endValue: Vector3.zero, duration: _vfxFadeOutBuffer)
            .OnComplete(() => Destroy(vfxTransform.gameObject));
    }

    private void ApplyDamage()
    {
        Vector3 boxCenter = _characterObject.position + _characterObject.TransformDirection(_abilitiesData.WaterJumpAttackOffset);
        
        int hitCount = Physics.OverlapBoxNonAlloc(
            boxCenter,
            _abilitiesData.WaterJumpAttackBoxHalfExtents,
            hitBuffer,
            transform.rotation,
            _abilitiesData.EnemyLayer
        );
        
        for (int i = 0; i < hitCount; i++)
        {
            Collider enemyCollider = hitBuffer[i];
            
            // Try to get damageable component (adjust to your project's health script)
            Debug.Log($"Hit {enemyCollider.name}");
            
        }
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 boxCenter = transform.position + transform.TransformDirection(_abilitiesData.WaterJumpAttackOffset);
        
        // Match matrix to character rotation so gizmo turns when player turns
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _abilitiesData.WaterJumpAttackBoxHalfExtents * 2f);
    }
#endif
    
}
