using Sirenix.OdinInspector;
using UnityEngine;

public class TreeFall : Abilitiy
{
    private void OnEnable()
    {
        _abilityEventAsset.OnInvoked.AddListener(PerformTreeFall);
    }

    private void OnDisable()
    {
        _abilityEventAsset.OnInvoked.RemoveListener(PerformTreeFall);
    }

    [Button]
    private void TestTreeFall()
    {
        PerformTreeFall(ECorgiAbility.Tree);
    }
    private void PerformTreeFall(ECorgiAbility context)
    {
        if (context != ECorgiAbility.Tree) return;

        SpawnSeed();
    }

    private void SpawnSeed()
    {
        Instantiate(_abilitiesData.SeedPrefab, _spawnPoint.position, Quaternion.identity);
    }
    
}
