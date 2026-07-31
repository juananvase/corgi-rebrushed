using System.Collections;
using GameEvents;
using PrimeTween;
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

    protected override void PerfomDeath()
    {
        base.PerfomDeath();
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return Tween.Delay(1.5f).ToYieldInstruction();
        GameManager.instance.Respawn();
    }
}
