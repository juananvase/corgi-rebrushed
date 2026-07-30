using System;
using System.Collections;
using GameEvents;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChilliExplosion : Abilitiy
{
    [SerializeField] [FoldoutGroup("References")] private ForcesBasedCharacterMovementController _movement;
    [SerializeField] [FoldoutGroup("References")] private SkinnedMeshRenderer _bodySkinMesh;
    [SerializeField] [FoldoutGroup("References")] private TransformEventAsset _onStartChilliState;
    private Coroutine _chilliStateCoroutine;
    
    private float _nextReadyTime;
    private bool _isCooldownOver => Time.time >= _nextReadyTime;
    
    private void OnEnable()
    {
        _abilityEventAsset.OnInvoked.AddListener(PerformChilliExplosion);
        _onStartChilliState.OnInvoked.AddListener(StartChilliState);
    }

    private void OnDisable()
    {
        _abilityEventAsset.OnInvoked.RemoveListener(PerformChilliExplosion);
        _onStartChilliState.OnInvoked.RemoveListener(StartChilliState);
    }
    
    private void PerformChilliExplosion(ECorgiAbility context)
    {
        if (!_isCooldownOver) return;
        if (context != ECorgiAbility.Fire) return;
        SpawnObject(_abilitiesData.ChilliPrefab, _spawnPoint.position, Quaternion.identity);
        _nextReadyTime = ResetCooldown(_abilitiesData.ChilliExplosionCoolDownDuration);
    }
    
    [Button("Test Chilli State")]
    public void StartChilliState(Transform context)
    {
        if(context != transform) return;

        if (_chilliStateCoroutine != null)
        {
            StopCoroutine(_chilliStateCoroutine);
            _chilliStateCoroutine = null;
            _chilliStateCoroutine = StartCoroutine(ChilliStateRoutine());
        }
        else _chilliStateCoroutine = StartCoroutine(ChilliStateRoutine());
    }

    private IEnumerator ChilliStateRoutine()
    {
        _movement.CurrentAcceleration = _abilitiesData.ChilliAcceleration;
        _movement.CurrentMaxSpeed = _abilitiesData.ChilliMaxSpeed;
        _bodySkinMesh.material = _abilitiesData.ChilliCorgiMaterial;
        
        yield return Tween.Delay(_abilitiesData.ChilliSateDuration).ToYieldInstruction();

        _movement.CurrentAcceleration = _movement.CharacterData.Acceleration;
        _movement.CurrentMaxSpeed = _movement.CharacterData.MaxSpeed;
        _bodySkinMesh.material = _abilitiesData.CommonCorgiMaterial;
    }
    
}
