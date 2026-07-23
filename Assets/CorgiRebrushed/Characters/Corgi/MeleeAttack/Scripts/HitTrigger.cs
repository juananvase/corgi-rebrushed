using System;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    [SerializeField] private string[] _hitLayers;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _hitLayers.Length; i++)
        {
            int hitLayerIndex = LayerMask.NameToLayer(_hitLayers[i]);
            if (other.gameObject.layer == hitLayerIndex)
            {
                if (other.TryGetComponent<SpawnObjectOnDeath>(out SpawnObjectOnDeath enemy))
                {
                    Destroy(enemy.gameObject);
                }
            }
        }
    }
}
