using System;
using System.Collections;
using GameEvents;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterVFXController : MonoBehaviour
{
    [SerializeField][FoldoutGroup("RunVFX")] private ForcesBasedCharacterMovementController _movement;
    [SerializeField][FoldoutGroup("RunVFX")] private ParticleSystem _runParticles;
    
    [SerializeField][FoldoutGroup("Melee VFX")] private Melee _melee;
    [SerializeField][FoldoutGroup("Melee VFX")] private ParticleSystem _meleeSlashVFX;
    [SerializeField][FoldoutGroup("Melee VFX")] private Transform _meleeSlashTransform;
    [SerializeField][FoldoutGroup("Melee VFX")] private Transform[] _meleeSlashVFXTransforms;
    
    [SerializeField] [FoldoutGroup("Chilli VXF")] private AbilitiesDataSO _abilitiesData;
    [SerializeField][FoldoutGroup("Chilli VXF")] private ParticleSystem _fireParticles;
    [SerializeField][FoldoutGroup("Chilli VXF")] private ParticleSystem _smokeParticles;
    [SerializeField] [FoldoutGroup("Chilli VXF")] private TransformEventAsset _onStartChilliState;
    
    [SerializeField][FoldoutGroup("Water VXF")] private GameObject _waterParticlesObjectPrefab;
    
    private ParticleSystem.EmissionModule _runParticlesEmission; 
    private ParticleSystem.EmissionModule _fireParticlesEmission; 
    private ParticleSystem.EmissionModule _smokeParticlesEmission; 
    private Coroutine _chilliStateCoroutine;
    private Coroutine _waterSplashCoroutine;
    
    
    private void OnEnable()
    {
        _onStartChilliState.OnInvoked.AddListener(StartChilliState);
    }

    private void OnDisable()
    {
        _onStartChilliState.OnInvoked.RemoveListener(StartChilliState);
    }
    
    private void Start()
    {
        _runParticlesEmission = _runParticles.emission;
        _fireParticlesEmission = _fireParticles.emission;
        _smokeParticlesEmission = _smokeParticles.emission;
        
        _fireParticles.Stop();
        _smokeParticles.Stop();
        
    }

    private void Update()
    {
        SetRunParticles();
    }

    public void PlayWaterParticles()
    {
        if (_waterSplashCoroutine != null)
        {
            StopCoroutine(_waterSplashCoroutine);
            _waterSplashCoroutine = StartCoroutine(WaterSplashRoutine());
        }
        else _waterSplashCoroutine = StartCoroutine(WaterSplashRoutine());
    }

    private IEnumerator WaterSplashRoutine()
    {
        GameObject waterSplash = Instantiate(_waterParticlesObjectPrefab, transform.position + new Vector3(0,-1,0), Quaternion.identity);
        if(waterSplash.TryGetComponent(out ParticleSystem waterSplashParticles)) waterSplashParticles.Play();
        
        yield return Tween.Delay(waterSplashParticles.main.duration).ToYieldInstruction();
        
        Destroy(waterSplash);
        
    }

    private void SetMeleeSlashTransform()
    {
        int value = Mathf.Clamp(_melee.Count, 0, 2);
        _meleeSlashTransform = _meleeSlashVFXTransforms[value];
    }

    public void PerformMeleeSlashVFX()
    {
        SetMeleeSlashTransform();
        _meleeSlashVFX.Play();
    }

    private void SetRunParticles()
    {
        if(_movement.IsGrounded && _runParticlesEmission.enabled) return;
        
        if (!_movement.IsGrounded) _runParticlesEmission.enabled = false;
        else _runParticlesEmission.enabled = true;
    }
    
    public void StartChilliState(Transform context)
    {
        if(context != transform) return;

        if (_chilliStateCoroutine != null)
        {
            StopCoroutine(_chilliStateCoroutine);
            _chilliStateCoroutine = StartCoroutine(ChilliStateRoutine());
        }
        else _chilliStateCoroutine = StartCoroutine(ChilliStateRoutine());
    }

    private IEnumerator ChilliStateRoutine()
    {
        _fireParticles.Play();
        _fireParticlesEmission.enabled = true;
        yield return Tween.Delay(_abilitiesData.ChilliSateDuration).ToYieldInstruction();
        
        _fireParticlesEmission.enabled = false;
        _fireParticles.Stop();
        
        _smokeParticles.Play();
        _smokeParticlesEmission.enabled = true;
        yield return Tween.Delay(0.5f).ToYieldInstruction();
        
        _smokeParticles.Stop();
        _smokeParticlesEmission.enabled = false;
        
    }
}
