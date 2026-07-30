using System;
using System.Collections;
using PrimeTween;
using UnityEngine;

public class Chilli : AbilityInvokeable
{
    
    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _colliders;
    [SerializeField] private ParticleSystem _explosionParticles;
    private ParticleSystem.EmissionModule _explosionParticlesEmission; 
    private Coroutine _chilliExplosionCoroutine;
    private Coroutine _chilliQuickExplosionCoroutine;
    
    // Cache an array for non-allocating physics checks (Max 20 targets per hit)
    private readonly Collider[] hitBuffer = new Collider[20];
    
    private void OnEnable()
    {
        Tween.Scale(transform, _abilitiesData.ChilliInScaleTweenSettings).OnComplete(()=>PerformExplosion());
        
        _explosionParticlesEmission = _explosionParticles.emission;
        _explosionParticles.Stop();
    }

    private void OnDestroy()
    {
        _explosionParticlesEmission.enabled = false;
    }

    private void PerformExplosion()
    {
        if(_chilliExplosionCoroutine  != null) StopCoroutine(_chilliExplosionCoroutine);
        else _chilliExplosionCoroutine = StartCoroutine(ExplosionRoutine());
    }
    
    public override void Damaged(DamageInfo damageInfo)
    {
        base.Damaged(damageInfo);
        
        if(_chilliExplosionCoroutine  != null) StopCoroutine(_chilliExplosionCoroutine);
        Explode();
    }

    private IEnumerator ExplosionRoutine()
    {
        yield return Tween.ShakeLocalPosition(transform, _abilitiesData.ChilliShakeTweenSettings).ToYieldInstruction();
        
        _body.SetActive(false);
        _colliders.SetActive(false);
        
        _explosionParticles.Play();
        _explosionParticlesEmission.enabled = true;
        
        ApplyDamage(_abilitiesData.ChilliExplosionAttackBoxHalfExtents, _abilitiesData.ChilliExplosionAttackOffset, _abilitiesData.ChilliExplosionDamage, Owner, EDamageType.Chilli);
        
        yield return Tween.Delay(1f).ToYieldInstruction();
        
        Destroy(gameObject);
    }
    
    private IEnumerator QuickExplosionRoutine()
    {
        _body.SetActive(false);
        _colliders.SetActive(false);
        
        _explosionParticles.Play();
        _explosionParticlesEmission.enabled = true;
        
        ApplyDamage(_abilitiesData.ChilliExplosionAttackBoxHalfExtents, _abilitiesData.ChilliExplosionAttackOffset, _abilitiesData.ChilliExplosionDamage, Owner, EDamageType.Chilli);
        
        yield return Tween.Delay(1f).ToYieldInstruction();
        
        Destroy(gameObject);
    }

    private void Explode()
    {
        if (_chilliQuickExplosionCoroutine != null) 
            StopCoroutine(QuickExplosionRoutine());
        
        StartCoroutine(QuickExplosionRoutine());
    }

    private void ApplyDamage(Vector3 halfExtents, Vector3 attackOffset, float damage, GameObject instigator, EDamageType damageType)
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(attackOffset);
        
        int hitCount = Physics.OverlapBoxNonAlloc(
            boxCenter,
            halfExtents,
            hitBuffer,
            transform.rotation,
            _abilitiesData.HitLayer
        );
        
        for (int i = 0; i < hitCount; i++)
        {
            Collider other = hitBuffer[i];
            
            if (other.transform.root.gameObject.TryGetComponent(out IDamageable target))
            {
                target.Damaged(new DamageInfo(damage, other.gameObject, this.gameObject, instigator, damageType));
            }
            
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cornsilk;
        Vector3 boxCenter = transform.position + transform.TransformDirection(_abilitiesData.ChilliExplosionAttackOffset);
        
        // Match matrix to character rotation so gizmo turns when player turns
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _abilitiesData.ChilliExplosionAttackBoxHalfExtents * 2f);
    }
#endif
}
