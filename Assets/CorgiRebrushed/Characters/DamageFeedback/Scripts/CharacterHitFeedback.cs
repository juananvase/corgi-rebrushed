using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterHitFeedback : HitFeedback
{
    [SerializeField, FoldoutGroup("References")]  private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void PerformHitFeedback(DamageInfo damageInfo)
    {
        base.PerformHitFeedback(damageInfo);
        PerformPushBack(damageInfo.Instigator.transform, damageInfo.Victim.transform);

    }

    [Button("PushBack Test")]
    private void PerformPushBack(Transform instigator, Transform victim)
    {
        if(_rigidbody == null) return;
        
        Vector3 direction = (victim.position - instigator.position).normalized;
        _rigidbody.AddForce(direction * _hitFeedbackData.PushBackForce, ForceMode.Impulse);
        
    }
}
