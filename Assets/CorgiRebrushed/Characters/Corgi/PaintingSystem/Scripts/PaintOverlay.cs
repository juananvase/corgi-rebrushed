using GameEvents;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PaintOverlay : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] private Image _overlayImage;
    [SerializeField][FoldoutGroup("Data")] private PaintingDataSO _data;
    [SerializeField] private ECorgiHabilityEventAsset mievento;

    [Button("EventoPrueba")]

    private void LLamarEvento()
    {
        mievento.Invoke(ECorgiHability.Water);
    }
    

    private void Awake()
    {
        Color c = _data.OverlayColor;
        c.a = 0f;
        _overlayImage.color = c;
    }

    public void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        TweenOverlayAlpha(_data.OverlayColor.a);
    }

    public void Hide()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        TweenOverlayAlpha(0f);
    }

    private void TweenOverlayAlpha(float targetAlpha)
    {
        Tween.Custom(
            startValue: _overlayImage.color.a,
            endValue: targetAlpha,
            duration: _data.OverlayFadeDuration,
            onValueChange: alpha =>
            {
                Color c = _overlayImage.color;
                c.a = alpha;
                _overlayImage.color = c;
            }
        );
    }
}
