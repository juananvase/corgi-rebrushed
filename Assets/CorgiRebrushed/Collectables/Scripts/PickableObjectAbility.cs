using System;
using GameEvents;
using UnityEngine;

public class PickableObjectAbility : Pickable
{
    [SerializeField] private ECorgiHabilityEventAsset _onPickupEventAsset;
    [SerializeField] private ECorgiAbility _EcorgiAbility;

    protected override void OnCollected(GameObject other)
    {
        base.OnCollected(other);
        _onPickupEventAsset.Invoke(_EcorgiAbility);
        Destroy(this.gameObject);
    }
}
