using System;
using UnityEngine;

public class SpawnObjectOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject _gameObjectToSpawn;
    [SerializeField] private Vector3 _spawnOffset;

    private void OnDestroy()
    {
        Instantiate(_gameObjectToSpawn, _spawnOffset, Quaternion.identity);
    }
}
