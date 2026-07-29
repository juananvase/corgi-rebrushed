using System;
using System.Collections;
using GameEvents;
using PrimeTween;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class HitFeedback : MonoBehaviour
{
    
    [SerializeField, FoldoutGroup("References")]  protected HitFeedbackDataSO _hitFeedbackData;
    [SerializeField, FoldoutGroup("References")]  private Renderer _renderer;
    //[SerializeField, FoldoutGroup("References")] private TransformEventAsset _onHitCamaraShake;
    

    private Color _originalColor;
    private Coroutine _flashCoroutine;
    
    private void Start()
    {
        _originalColor =  _renderer.material.color;
    }

    public virtual void PerformHitFeedback(DamageInfo damageInfo)
    {
        PerformFlash();
        PerformPause();
        PerformCameraShake(GameManager.instance.PlayerCameraCinemachineImpulseSource);
    }
    
    [Button("CameraShake Test")]
    private void PerformCameraShake(CinemachineImpulseSource impulseSource)
    {
        Tween.Custom(useUnscaledTime: true, startValue: _hitFeedbackData.CameraShakeForce, endValue: 0f, duration: _hitFeedbackData.CameraShakeDuration, onValueChange: force =>
        {
            CinemachineImpulseManager.Instance.IgnoreTimeScale = true;
            impulseSource.GenerateImpulse(force * 0.1f);
        });
    }

    [Button("Flash Test")]
    private void PerformFlash()
    {
        if (_flashCoroutine != null) StopCoroutine(_flashCoroutine);
        else StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        _renderer.material.color = _hitFeedbackData.FlashColor;
        yield return Tween.Delay(_hitFeedbackData.FlashDuration).ToYieldInstruction();
        _renderer.material.color = _originalColor;
    }
    
    [Button("Pause Test")]
    private void PerformPause()
    {
        GameTimeManager.instance.ModifyTimeScaleForDuration(_hitFeedbackData.PauseDuration);
    }
    
    
}
