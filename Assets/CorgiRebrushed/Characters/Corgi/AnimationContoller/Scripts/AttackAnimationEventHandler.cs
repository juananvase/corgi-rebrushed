using UnityEngine;
using UnityEngine.Events;

public class AttackAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent _onFinishAttackAnimation;
    [SerializeField] private UnityEvent _onFinishFinalAttackAnimation;
    [SerializeField] private UnityEvent _onHitEnable;
    [SerializeField] private UnityEvent _onHitDisable;
    [SerializeField] private UnityEvent _onSpinHitEnable;
    [SerializeField] private UnityEvent _onSpinHitDisable;

    public void FinishAttackAnimation()
    {
        _onFinishAttackAnimation.Invoke();
    }

    private void FinishFinalAttackAnimation()
    {
        _onFinishFinalAttackAnimation.Invoke();
    }

    public void HitEnable()
    {
        _onHitEnable.Invoke();
    }

    public void HitDisable()
    {
        _onHitDisable.Invoke();
    }
    
    public void SpinHitEnable()
    {
        _onSpinHitEnable.Invoke();
    }

    public void SpinHitDisable()
    {
        _onSpinHitDisable.Invoke();
    }
}
