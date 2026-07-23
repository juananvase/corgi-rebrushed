using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class Melee : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")] private InputActionAsset _inputActions;
    [field: SerializeField, FoldoutGroup("References")] private Animator _animator;
    
    [field: SerializeField, FoldoutGroup("Melee")] private float cooldownDuration = 3f;
    
    private int _attackInt;
    private int _attackTrigger;
    
    private float _nextReadyTime;
    private bool _isCooldownOver => Time.time >= _nextReadyTime;
    
    [ShowInInspector] private int _count;

    private InputAction _attackAction;
    private InputAction _enterPaintModeAction;
    
    private void OnEnable()
    {
        _inputActions.FindActionMap("Player").Enable();
    }
    
    private void OnDisable()
    {
        _inputActions.FindActionMap("Player").Disable();
    }
    
    private void Awake()
    {
        SetAnimationHashes();
        _attackAction = InputSystem.actions.FindAction("Attack");
        _enterPaintModeAction = InputSystem.actions.FindAction("EnterPaintMode");
    }

    private void Update()
    {
        
    }

    private void Start()
    {
        _attackAction.performed += PerfomrAttack;
    }

    private void SetAnimationHashes()
    {
        _attackTrigger = Animator.StringToHash("Attack");
        _attackInt = Animator.StringToHash("AttackInt");
    }
    
    private void PerfomrAttack(InputAction.CallbackContext context)
    {
        if(_enterPaintModeAction.IsInProgress()) return;
        
        _animator.SetTrigger(_attackTrigger);
        
        if (_isCooldownOver)
        {
            ResetCount();
        }
    }

    public void OnAttackAnimationEnded()
    {
        _count++;
        _animator.SetInteger(_attackInt, _count);
        
        ResetCooldown();
        _animator.ResetTrigger(_attackTrigger);
    }
    
    private void ResetCooldown()
    {
        _nextReadyTime = Time.time + cooldownDuration;
    }

    public void ResetCount()
    {
        _count = 0;
        _animator.SetInteger(_attackInt, _count);
    }
    
}
