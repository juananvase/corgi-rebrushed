using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;

public class CoreTree : AbilityInvokeable, IDamageable
{
    [SerializeField, FoldoutGroup("References")] private Transform _core;
    [SerializeField, FoldoutGroup("References")] private Collider _logCollider;
    [SerializeField, FoldoutGroup("References")] private Collider _generalCollider;
    
    private Coroutine _fallenTreeCoroutine;

    public bool IsAlive { get; private set; } = true;
    
    private void OnEnable()
    {
        _generalCollider.enabled = true;
        _logCollider.enabled = false;
        _logCollider.isTrigger = true;
        Tween.Scale(transform, _abilitiesData.TreeSpawnInScaleTweenSettings);
    }

    private void FaceTarget(Transform other)
    {
        Vector3 fallDirection = (transform.position - other.position);
        fallDirection.y = 0f;
        fallDirection.Normalize();
        
        _core.rotation = Quaternion.LookRotation(fallDirection, Vector3.up);
    }

    private Quaternion GetEndFallRotation()
    {
        Quaternion targetRotation = _core.rotation * Quaternion.Euler(93f, 0f, 0f);
        
        return targetRotation;
    }

    private IEnumerator OnFallenTreeRoutine(DamageInfo damageInfo)
    {
        IsAlive = false;
        _generalCollider.enabled = false;
        _logCollider.enabled = true;
        _logCollider.isTrigger = true;
        FaceTarget(damageInfo.Instigator.transform);
        yield return Tween.Rotation(_core, new TweenSettings<Quaternion>(endValue: GetEndFallRotation(), _abilitiesData.TreeFallTweenSettings)).ToYieldInstruction();
        _logCollider.isTrigger = false;
        yield return Tween.Delay(_abilitiesData.TreeDestroyTime).ToYieldInstruction();
        yield return Tween.ShakeLocalPosition(_core, _abilitiesData.TreeShakeTweenSettings).ToYieldInstruction();
        Destroy(transform.root.gameObject);
    }
    
    public void Damaged(DamageInfo damageInfo)
    {
        if (!IsAlive) return;
        
        if(_fallenTreeCoroutine != null) StopCoroutine(_fallenTreeCoroutine);
        else _fallenTreeCoroutine = StartCoroutine(OnFallenTreeRoutine(damageInfo));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _abilitiesData.TreeFallHitLayers.Length; i++)
        {
            int hitLayerIndex = LayerMask.NameToLayer(_abilitiesData.TreeFallHitLayers[i]);
            if (other.gameObject.layer == hitLayerIndex)
            {
                if (other.transform.root.gameObject.TryGetComponent(out IDamageable target))
                {
                    target.Damaged(new DamageInfo(_abilitiesData.TreeFallDamage, other.gameObject, this.gameObject, Owner, EDamageType.Tree));
                }
            }
        }
    }
}
