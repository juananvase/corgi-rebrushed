using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;

public class HitFeedback : MonoBehaviour
{
    [SerializeField]  private Renderer _renderer;
    [SerializeField]  private Color _flashColor;
    [SerializeField]  private float _flashDuration;
    [SerializeField]  private Rigidbody _rigidbody;
    [SerializeField]  private float _pushBackForce;

    private Color _originalColor;
    private Coroutine _flashCoroutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _originalColor =  _renderer.material.color;
    }

    public void PerformCharacterHitFeedback(DamageInfo damageInfo)
    {
        PerformFlash();
        PerformPushBack(damageInfo.Instigator.transform, damageInfo.Victim.transform);
    }

    [Button("PushBack Test")]
    private void PerformPushBack(Transform instigator, Transform victim)
    {
        if(_rigidbody == null) return;
        
        Vector3 direction = (victim.position - instigator.position).normalized;
        _rigidbody.AddForce(direction * _pushBackForce, ForceMode.Impulse);
        
    }

    [Button("Flash Test")]
    private void PerformFlash()
    {
        if (_flashCoroutine != null) StopCoroutine(_flashCoroutine);
        else StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        _renderer.material.color = _flashColor;
        yield return Tween.Delay(_flashDuration).ToYieldInstruction();
        _renderer.material.color = _originalColor;
    }
    
}
