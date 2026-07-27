using System.Collections.Generic;
using GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;

public class AbilityManager : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<ECorgiAbility, Abilitiy> _abilities;
    [SerializeField] private ECorgiHabilityEventAsset _onPickupEventAsset;
    
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
    }
}
