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
    private int _lastComboStep = -1;

    private void HandleAttack(int comboStep)
    {
        // Ignore clicks outside the 3-attack combo
        if (comboStep < 0 || comboStep > 2 || _fmodEvent.IsNull) return;

        // Ignore repeated calls for the same combo step (rapid clicks)
        if (comboStep == _lastComboStep) return;
        _lastComboStep = comboStep;

        // Create instance to ensure parameter is set before the sound triggers
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
