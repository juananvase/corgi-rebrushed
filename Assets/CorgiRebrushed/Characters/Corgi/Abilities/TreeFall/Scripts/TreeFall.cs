using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events; //Required for audio

public class TreeFall : Abilitiy
{
    
    //Audio
    [FoldoutGroup("Audio")] public UnityEvent OnTreeSpawnActivated;
    //End Audio

    private void OnEnable()
    {
        _abilityEventAsset.OnInvoked.AddListener(PerformTreeFall);
    }

    private void OnDisable()
    {
        _abilityEventAsset.OnInvoked.RemoveListener(PerformTreeFall);
    }

    private void Update()
    {
        GUIManager.instance.TreeAbilityProgress = GetCooldownProgress(_abilitiesData.TreeFallCoolDownDuration);
    }

    [Button]
    private void TestTreeFall()
    {
        PerformTreeFall(ECorgiAbility.Tree);
    }
    private void PerformTreeFall(ECorgiAbility context)
    {
        if (!_isCooldownOver) return;
        if (context != ECorgiAbility.Tree) return;

        SpawnObject(_abilitiesData.SeedPrefab, _spawnPoint.position, Quaternion.identity);
        _nextReadyTime = ResetCooldown(_abilitiesData.TreeFallCoolDownDuration);
        
        //Audio
        OnTreeSpawnActivated?.Invoke();
        //End Audio
    }
    
}
