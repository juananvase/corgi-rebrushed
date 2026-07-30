using System;
using GameEvents;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")] private CharacterDataSO _characterData;
    [SerializeField, FoldoutGroup("References")] private Transform _characterObject;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _characterData.HitLayers.Length; i++)
        {
            int hitLayerIndex = LayerMask.NameToLayer(_characterData.HitLayers[i]);
            if (other.gameObject.layer == hitLayerIndex)
            {
                if (other.transform.root.gameObject.TryGetComponent(out IDamageable target))
                {
                    target.Damaged(new DamageInfo(_characterData.Damage, other.gameObject, this.gameObject, _characterObject.gameObject, EDamageType.Brush));
                }
            }
        }
    }
}
