using System;
using PrimeTween;
using UnityEngine;

public class FloatObjectet : MonoBehaviour
{
    [SerializeField] TweenSettings _floatingTweenSettings;

    private void OnEnable()
    {
        Tween.LocalPosition(transform, startValue: Vector3.zero, endValue: new Vector3(0f, 0.1f, 0f), _floatingTweenSettings);
    }
}
