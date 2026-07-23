using UnityEngine;
using UnityEngine.Events;

public class AttackAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent _onFinishAttackAnimation;
    [SerializeField] private UnityEvent _onFinishFinalAttackAnimation;
    [SerializeField] private UnityEvent _onHitEnable;
    [SerializeField] private UnityEvent _onHitDisable;

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
}
