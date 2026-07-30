using System;
using System.Collections;
using PrimeTween;
using UnityEngine;

public class Seed : AbilityInvokeable
{
    [SerializeField] private Vector3 _spawnOffset;
    [SerializeField] private string[] _hitLayers;
    [SerializeField] private ParticleSystem _dirtParticles;
    
    private Coroutine _spawnTreeCoroutine;
    private ParticleSystem.EmissionModule _dirtParticlesEmission;
    
    private bool _hasSpawnTree = false;

    private void OnEnable()
    {
        _dirtParticles.Stop();
        _dirtParticlesEmission = _dirtParticles.emission;
        
        _hasSpawnTree = false;
        
    }

    private void SpawnTree()
    {
        if(_hasSpawnTree) return;
        _hasSpawnTree = true;
        
        GameObject treeObject = Instantiate(_abilitiesData.TreePrefab, transform.position + _spawnOffset, Quaternion.identity);
        
        if (TryGetComponent(out AbilityInvokeable tree))
        {
            tree.Owner = Owner;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        for (int i = 0; i < _hitLayers.Length; i++)
        {
            int hitLayerIndex = LayerMask.NameToLayer(_hitLayers[i]);
            if (other.gameObject.layer == hitLayerIndex)
            {
                if (_spawnTreeCoroutine != null)
                {
                    StopCoroutine(_spawnTreeCoroutine);
                    _spawnTreeCoroutine = StartCoroutine(SpawnTreeRoutine());
                }
                else _spawnTreeCoroutine = StartCoroutine(SpawnTreeRoutine());
                
                break;
            }
        }
    }

    private IEnumerator SpawnTreeRoutine()
    {
        _dirtParticles.Play();
        _dirtParticlesEmission.enabled = true;
        
        SpawnTree();
        
        yield return Tween.Delay(1f).ToYieldInstruction();
        
        _dirtParticles.Stop();
        _dirtParticlesEmission.enabled = false;
        
        Destroy(gameObject);
    }
}
