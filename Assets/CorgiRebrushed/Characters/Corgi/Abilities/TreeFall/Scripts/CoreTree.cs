using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CoreTree : AbilityInvokeable
{
    [SerializeField, FoldoutGroup("References")] private Transform _core;
    [SerializeField, FoldoutGroup("References")] private Collider _logCollider;
    [SerializeField, FoldoutGroup("References")] private Collider _generalCollider;
    [SerializeField, FoldoutGroup("References")] private ParticleSystem _fallDustParticles;
    [SerializeField, FoldoutGroup("References")] private ParticleSystem _destroyWoodParticles;
    [SerializeField, FoldoutGroup("References")] private GameObject _body;
    [SerializeField, FoldoutGroup("References")] private GameObject _colliders;
    
    private ParticleSystem.EmissionModule _fallDustParticlesEmission; 
    private ParticleSystem.EmissionModule _destroyWoodParticlesEmission; 

    
    private Coroutine _fallenTreeCoroutine;
    
    private void OnEnable()
    {
        _generalCollider.enabled = true;
        _logCollider.enabled = false;
        _logCollider.isTrigger = true;
        Tween.Scale(transform, _abilitiesData.TreeSpawnInScaleTweenSettings);
        
        _fallDustParticlesEmission = _fallDustParticles.emission;
        _destroyWoodParticlesEmission = _destroyWoodParticles.emission;
        
        _fallDustParticles.Stop();
        _destroyWoodParticles.Stop();
    }

    private void OnDestroy()
    {
        _destroyWoodParticlesEmission.enabled = false;
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
        Quaternion targetRotation = _core.rotation * Quaternion.Euler(88f, 0f, 0f);
        
        return targetRotation;
    }

    private IEnumerator OnFallenTreeRoutine(DamageInfo damageInfo)
    {
        _generalCollider.enabled = false;
        _logCollider.enabled = true;
        _logCollider.isTrigger = true;
        FaceTarget(damageInfo.Instigator.transform);
        yield return Tween.Rotation(_core, new TweenSettings<Quaternion>(endValue: GetEndFallRotation(), _abilitiesData.TreeFallTweenSettings)).ToYieldInstruction();
        
        _fallDustParticles.Play();
        _fallDustParticlesEmission.enabled = true;
        yield return Tween.Delay(1f).ToYieldInstruction();
        
        _fallDustParticlesEmission.enabled = false;
        _logCollider.isTrigger = false;
        yield return Tween.Delay(_abilitiesData.TreeDestroyTime).ToYieldInstruction();
        
        yield return Tween.ShakeLocalPosition(_core, _abilitiesData.TreeShakeTweenSettings).ToYieldInstruction();
        
        _destroyWoodParticles.transform.SetParent(transform.root);
        _destroyWoodParticles.Play();
        _destroyWoodParticlesEmission.enabled = true;
        _body.SetActive(false);
        _colliders.SetActive(false);
        yield return Tween.Delay(1f).ToYieldInstruction();
        
        Destroy(transform.root.gameObject);
    }
    
    public override void Damaged(DamageInfo damageInfo)
    {
        base.Damaged(damageInfo);
        
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
