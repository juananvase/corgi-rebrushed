using System;
using UnityEngine;

public class Seed : MonoBehaviour
{
    [SerializeField] private AbilitiesDataSO _abilitiesData;
    [SerializeField] private Vector3 _spawnOffset;
    [SerializeField] private string[] _hitLayers;

    private void OnCollisionEnter(Collision other)
    {
        for (int i = 0; i < _hitLayers.Length; i++)
        {
            int hitLayerIndex = LayerMask.NameToLayer(_hitLayers[i]);
            if (other.gameObject.layer == hitLayerIndex)
            {
                Instantiate(_abilitiesData.TreePrefab, transform.position + _spawnOffset, Quaternion.identity);
                Destroy(gameObject);
                break;
            }
        }
    }
}
