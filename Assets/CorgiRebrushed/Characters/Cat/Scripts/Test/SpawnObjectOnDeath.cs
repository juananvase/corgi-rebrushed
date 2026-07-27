using System;
using UnityEngine;

public class SpawnObjectOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject _gameObjectToSpawn;
    [SerializeField] private Vector3 _spawnOffset;

    public void SpawnObject()
    {
        Instantiate(_gameObjectToSpawn, transform.position + _spawnOffset, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
