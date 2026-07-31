using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using UnityEngine.Events; //Required for audio

public class WaterJump : Abilitiy
{
    [SerializeField] [FoldoutGroup("References")] private Rigidbody _rigidbody;
    [SerializeField, BoxGroup("Events")] private UnityEvent OnWaterImpulse;
    
    //Audio
     [FoldoutGroup("Audio")] public UnityEvent OnWaterJetActivated;
    // End Audio

    private void OnEnable()
    {
        _abilityEventAsset.OnInvoked.AddListener(PerformWaterImpulse);
    }

    private void OnDisable()
    {
        _abilityEventAsset.OnInvoked.RemoveListener(PerformWaterImpulse);
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GUIManager.instance.WaterAbilityProgress = GetCooldownProgress(_abilitiesData.WaterJumpCoolDownDuration);
    }

    private void PerformWaterImpulse(ECorgiAbility context)
    {
        Debug.Log(_isCooldownOver);
        if (!_isCooldownOver) return;
        if (context != ECorgiAbility.Water) return;
        
        ApplyDamage(_abilitiesData.WaterJumpAttackBoxHalfExtents, _abilitiesData.WaterJumpAttackOffset, _abilitiesData.WaterImpulseDamage, gameObject, EDamageType.WaterJump);
        ApplyImpulseForce();
        OnWaterImpulse.Invoke();
        PlayWaterJetVfx();
        _nextReadyTime = ResetCooldown(_abilitiesData.WaterJumpCoolDownDuration);
        
        //Audio
        OnWaterJetActivated?.Invoke();
        // End Audio
    }
    
    private void ApplyImpulseForce()
    {
        _rigidbody.AddForce(transform.up * _abilitiesData.WaterJumpImpulseForce, ForceMode.Impulse);
    }
    

    private void PlayWaterJetVfx()
    {
        if (_abilitiesData.WaterCascadeVfxPrefab == null) return;

        Vector3 spawnPosition = _spawnPoint.position + _abilitiesData.VfxOffset;
        VisualEffect vfxInstance = Instantiate(_abilitiesData.WaterCascadeVfxPrefab, spawnPosition, Quaternion.identity);
        vfxInstance.Play();

        Transform vfxTransform = vfxInstance.transform;
        Tween.Scale(vfxTransform, _abilitiesData.WaterCascadeInScaleTweenSettings)
            .Chain(Tween.Scale(vfxTransform, _abilitiesData.WaterCascadeOutScaleTweenSettings))
            .OnComplete(() => StopAndDestroyVfx(vfxInstance));
    }

    private void StopAndDestroyVfx(VisualEffect vfxInstance)
    {
        vfxInstance.Stop();
        Destroy(vfxInstance.transform.gameObject);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 boxCenter = transform.position + transform.TransformDirection(_abilitiesData.WaterJumpAttackOffset);
        
        // Match matrix to character rotation so gizmo turns when player turns
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _abilitiesData.WaterJumpAttackBoxHalfExtents * 2f);
    }
#endif
    
}
