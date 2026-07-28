using System;
using UnityEngine;

public class Seed : AbilityInvokeable
{
    [SerializeField] private Vector3 _spawnOffset;
    [SerializeField] private string[] _hitLayers;

    private void SpawnTree()
    {
        GameObject treeObject = Instantiate(_abilitiesData.TreePrefab, transform.position + _spawnOffset, Quaternion.identity);
        
        if (TryGetComponent(out AbilityInvokeable tree))
        {
            tree.Owner = gameObject;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        for (int i = 0; i < _hitLayers.Length; i++)
        {
            int hitLayerIndex = LayerMask.NameToLayer(_hitLayers[i]);
            if (other.gameObject.layer == hitLayerIndex)
            {
                SpawnTree();
                Destroy(gameObject);
                break;
            }
        }
    }
}
