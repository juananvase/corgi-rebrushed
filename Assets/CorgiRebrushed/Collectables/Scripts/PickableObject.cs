using System;
using GameEvents;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private ECorgiHabilityEventAsset _onPickupEventAsset;
    [SerializeField] private ECorgiAbility _EcorgiAbility;
    [SerializeField] private string[] _hitLayers;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _hitLayers.Length; i++)
        {
            int hitLayerIndex = LayerMask.NameToLayer(_hitLayers[i]);
            if (other.gameObject.layer == hitLayerIndex)
            {
                _onPickupEventAsset.Invoke(_EcorgiAbility);
                Destroy(this.gameObject);
            }
        }
    }
}
