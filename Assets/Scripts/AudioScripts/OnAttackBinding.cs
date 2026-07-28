using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

[AddComponentMenu("Corgi Audio/On Attack Binding")]
public class OnAttackBinding : MonoBehaviour
{
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;

    private Melee _melee;

    private void Awake()
    {
        _melee = FindObjectOfType<Melee>();
    }

    private void OnEnable()
    {
        if (_melee != null)
            _melee.OnAttackPerformed.AddListener(HandleAttack);
    }

    private void OnDisable()
{
    if (_melee != null)
        _melee.OnAttackPerformed.RemoveListener(HandleAttack);
    
    // Clean up any lingering instance
    if (_currentAttackInstance.hasHandle())
    {
        _currentAttackInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _currentAttackInstance.release();
    }
}

    private FMOD.Studio.EventInstance _currentAttackInstance;
    private float _lastAttackTime;

     private void HandleAttack(int comboStep)
    {
        if (_fmodEvent.IsNull) return;

        var instance = RuntimeManager.CreateInstance(_fmodEvent);
        instance.setParameterByName("AttackStep", comboStep);
        instance.start();
        instance.release();
    }

    private void OnDestroy()
    {
        _currentAttackInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _currentAttackInstance.release();
    }


}
