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

    [Button]
    private void TestTreeFall()
    {
        PerformTreeFall(ECorgiAbility.Tree);
    }
    private void PerformTreeFall(ECorgiAbility context)
    {
        if (context != ECorgiAbility.Tree) return;

        SpawnObject(_abilitiesData.SeedPrefab, _spawnPoint.position, Quaternion.identity);
        //Audio
        OnTreeSpawnActivated?.Invoke();
        //End Audio
    }
    
}
