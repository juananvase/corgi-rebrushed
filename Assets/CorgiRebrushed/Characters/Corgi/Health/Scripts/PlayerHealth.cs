using GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] [FoldoutGroup("References")] private TransformEventAsset _onStartChilliState;
    public override void Damaged(DamageInfo damageInfo)
    {
        if(damageInfo.EDamageType == EDamageType.Chilli) _onStartChilliState.Invoke(transform);
        if(damageInfo.Instigator == gameObject) return;
        
        base.Damaged(damageInfo);
    }
}
