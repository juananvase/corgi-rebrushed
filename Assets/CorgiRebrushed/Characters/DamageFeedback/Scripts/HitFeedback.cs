using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;

public class HitFeedback : MonoBehaviour
{
    
    [SerializeField, FoldoutGroup("References")]  protected HitFeedbackDataSO _hitFeedbackData;
    [SerializeField, FoldoutGroup("References")]  private Renderer _renderer;
    

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
