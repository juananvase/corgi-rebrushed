using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChilliExplosion : Abilitiy
{
    private void OnEnable()
    {
        _abilityEventAsset.OnInvoked.AddListener(PerformChilliExplosion);
    }

    private void OnDisable()
    {
        _abilityEventAsset.OnInvoked.RemoveListener(PerformChilliExplosion);
    }
    
    [Button]
    private void TestChilliExplosion()
    {
        PerformChilliExplosion(ECorgiAbility.Fire);
    }
    private void PerformChilliExplosion(ECorgiAbility context)
    {
        if (context != ECorgiAbility.Fire) return;
        SpawnChilli();
    }

    private void SpawnChilli()
    {
        Instantiate(_abilitiesData.ChilliPrefab, _spawnPoint.position, Quaternion.identity);
    }
    
}
