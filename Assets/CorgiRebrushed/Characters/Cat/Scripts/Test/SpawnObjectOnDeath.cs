using System;
using UnityEngine;

public class SpawnObjectOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject _gameObjectToSpawn;
    [SerializeField] private Vector3 _spawnOffset;

    private void OnDestroy()
    {
        if(_gameObjectToSpawn == null) return;
        Instantiate(_gameObjectToSpawn, transform.position + _spawnOffset, Quaternion.identity);
    }
}
