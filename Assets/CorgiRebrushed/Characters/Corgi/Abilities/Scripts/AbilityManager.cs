using System.Collections.Generic;
using GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events; //Required for audio

public class AbilityManager : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<ECorgiAbility, Abilitiy> _abilities;
    [SerializeField] private ECorgiHabilityEventAsset _onPickupEventAsset;

    //Audio
    [FoldoutGroup("Audio")] public UnityEvent OnAbilityUnlocked;
    //End Audio
    
    private void OnEnable()
    {
        _onPickupEventAsset.OnInvoked.AddListener(UnlockAbility);
    }

    private void OnDisable()
    {
        _onPickupEventAsset.OnInvoked.RemoveListener(UnlockAbility);
    }

    private void UnlockAbility(ECorgiAbility context)
    {
        _abilities[context].enabled = true;

        //Audio — stinger on fability unlock
        _abilities[context].enabled = true;
        OnAbilityUnlocked?.Invoke();
    }
}
