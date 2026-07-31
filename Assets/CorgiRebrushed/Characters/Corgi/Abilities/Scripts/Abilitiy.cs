using GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Abilitiy : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] protected Transform _spawnPoint;
    [SerializeField][FoldoutGroup("References")] protected AbilitiesDataSO _abilitiesData;
    [SerializeField][FoldoutGroup("References")] protected ECorgiHabilityEventAsset _abilityEventAsset;
    
    // Cache an array for non-allocating physics checks (Max 20 targets per hit)
    private readonly Collider[] hitBuffer = new Collider[20];
    
    protected float _nextReadyTime;
    protected bool _isCooldownOver => Time.time >= _nextReadyTime;
    
    protected void ApplyDamage(Vector3 halfExtents, Vector3 attackOffset, float damage, GameObject instigator, EDamageType damageType)
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
    
    protected void SpawnObject(GameObject objectPrefab, Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = Instantiate(objectPrefab, position, rotation);
        
        if (objectToSpawn.TryGetComponent(out AbilityInvokeable other))
        {
            other.Owner = gameObject;
        }
    }
    
    protected float ResetCooldown(float cooldownDuration)
    {
        return Time.time + cooldownDuration;
    }
    
    protected float GetCooldownProgress(float cooldownDuration)
    {
        if (_isCooldownOver) return 1f;
        float timeRemaining = _nextReadyTime - Time.time;
        return 1f - Mathf.Clamp01(timeRemaining / cooldownDuration);
    }
    
}
