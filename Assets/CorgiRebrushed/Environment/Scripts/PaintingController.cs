using System;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaintingController : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Portal")] private int _treatAmountRequire;
    [SerializeField, FoldoutGroup("Portal")] private string[] _hitLayers;
    [SerializeField, FoldoutGroup("Portal")] private int _nextSceneIndex;
    [SerializeField, FoldoutGroup("Portal")] private ShakeSettings _shakeSettings;

    private void OnCollisionEnter(Collision other)
    {
        for (int i = 0; i < _hitLayers.Length; i++)
        {
            int hitLayerIndex = LayerMask.NameToLayer(_hitLayers[i]);
            if (other.gameObject.layer == hitLayerIndex)
            {
                if (_treatAmountRequire <= GameManager.instance.TreatCount)
                {
                    GameManager.instance.LoadSceneByIndex(_nextSceneIndex);
                }
                else
                {
                    Tween.ShakeLocalPosition(transform, _shakeSettings);
                }
            }
        }
    }
}
