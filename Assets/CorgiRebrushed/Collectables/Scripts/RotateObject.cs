using PrimeTween;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] Vector3 _targetRotationGain;
    [SerializeField] TweenSettings _rotatingTweenSettings;

    private void OnEnable()
    {
        FirstRotation();
    }

    private void FirstRotation()
    {
        Vector3 targetRotation = transform.localEulerAngles + _targetRotationGain;
        Tween.LocalEulerAngles(transform, startValue:transform.localEulerAngles ,endValue: targetRotation, _rotatingTweenSettings);
    }
}
