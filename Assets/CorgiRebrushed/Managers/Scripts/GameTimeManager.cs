using System;
using System.Collections;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager instance { get; private set; }
    private void InitiateSinglenton()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }
    
    [SerializeField, FoldoutGroup("References")] private InputActionAsset _inputActions;
    [SerializeField, FoldoutGroup("PaintMode")] private float _slowTimeScale = 0.5f;

    private InputAction _enterPaintModeAction;
    private bool _isTimeScaleBeingUsed = false;
    private Coroutine _modifyTimeScaleCoroutine;
    
    private void Awake()
    {
        InitiateSinglenton();
        _enterPaintModeAction = _inputActions.FindAction("EnterPaintMode");
    }
    
    protected virtual void OnEnable()
    {
        _inputActions.FindActionMap("Player").Enable();
    }
    
    protected virtual void OnDisable()
    {
        _inputActions.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        SlowDownTimeOnPaintMode();
    }

    private void SlowDownTimeOnPaintMode()
    {
        if (_enterPaintModeAction.IsPressed())
        {
            Time.timeScale = _slowTimeScale;
        }
        else if(!_isTimeScaleBeingUsed)
        {
            Time.timeScale = 1;
        }
    }

    public void ModifyTimeScaleForDuration(float  duration, float amount = 0f)
    {
        if (_modifyTimeScaleCoroutine != null) StopCoroutine(_modifyTimeScaleCoroutine);
        else StartCoroutine(ModifyTimeScaleRoutine(duration,  amount));
    }

    private IEnumerator ModifyTimeScaleRoutine(float  duration, float amount)
    {
        Debug.Log(Time.timeScale);
        _isTimeScaleBeingUsed  = true;
        Time.timeScale = amount;
        yield return Tween.Delay(duration).ToYieldInstruction();
        _isTimeScaleBeingUsed  = false;
        Time.timeScale = 1f;
    }
    
}
