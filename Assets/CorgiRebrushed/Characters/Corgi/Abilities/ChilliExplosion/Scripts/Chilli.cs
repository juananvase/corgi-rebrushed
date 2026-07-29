using System.Collections;
using PrimeTween;
using UnityEngine;

public class Chilli : AbilityInvokeable, IDamageable
{
    private Coroutine _chilliExplosionCoroutine;
    public bool IsAlive { get; }
    
    // Cache an array for non-allocating physics checks (Max 20 targets per hit)
    private readonly Collider[] hitBuffer = new Collider[20];
    
    private void OnEnable()
    {
        Tween.Scale(transform, _abilitiesData.ChilliInScaleTweenSettings).OnComplete(()=>PerformExplosion());
    }
    
    private void PerformExplosion()
    {
        if(_chilliExplosionCoroutine  != null) StopCoroutine(_chilliExplosionCoroutine);
        else _chilliExplosionCoroutine = StartCoroutine(ExplosionRoutine());
    }
    
    public void Damaged(DamageInfo damageInfo)
    {
        if(_chilliExplosionCoroutine  != null) StopCoroutine(_chilliExplosionCoroutine);
        Tween.ShakeLocalPosition(transform, _abilitiesData.ChilliShakeTweenSettings).OnComplete(() => Explode(), warnIfTargetDestroyed: false);
    }

    private IEnumerator ExplosionRoutine()
    {
        yield return Tween.Delay(_abilitiesData.ChilliExplosionTime).ToYieldInstruction();
        yield return Tween.ShakeLocalPosition(transform, _abilitiesData.ChilliShakeTweenSettings).ToYieldInstruction();
        Explode();
    }

    private void Explode()
    {
        ApplyDamage(_abilitiesData.ChilliExplosionAttackBoxHalfExtents, _abilitiesData.ChilliExplosionAttackOffset, _abilitiesData.ChilliExplosionDamage, Owner, EDamageType.Chilli);
        Destroy(gameObject);
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
