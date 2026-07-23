using System.Collections;
using GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaintingModeController : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] private PaintOverlay _overlay;
    [SerializeField][FoldoutGroup("References")] private PaintCanvas _canvas;
    [SerializeField][FoldoutGroup("References")] private SymbolRecognizer _recognizer;
    [SerializeField][FoldoutGroup("References")] private ECorgiHabilityEventAsset _corgiHabilityEvent;
    [SerializeField][FoldoutGroup("References")] private SymbolAbilityMapSO _symbolAbilityMap;

    [SerializeField][FoldoutGroup("Data")] private float _recognizedStrokeHoldDuration = 1f;

    [ShowInInspector][FoldoutGroup("Testing")] private bool _isPainting;
    [ShowInInspector][FoldoutGroup("Testing")] private bool _isStroking;

    private ThirdPersonCameraController _cameraController;

    private void Awake()
    {
        _cameraController = FindAnyObjectByType<ThirdPersonCameraController>();
    }

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
            EnterPaintMode();
        else if (Mouse.current.rightButton.wasReleasedThisFrame)
            ExitPaintMode();

        if (_isPainting)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
                BeginStroke();
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
                PauseStroke();
        }
    }

    private void EnterPaintMode()
    {
        _isPainting = true;
        if (_cameraController != null) _cameraController.enabled = false;
        _overlay.Show();
        _canvas.PrepareSession();
    }

    private void BeginStroke()
    {
        _isStroking = true;
        _canvas.BeginStroke();
    }

    private void PauseStroke()
    {
        _isStroking = false;
        _canvas.PauseStroke();
    }

    private void ExitPaintMode()
    {
        _isPainting = false;
        _isStroking = false;
        if (_cameraController != null) _cameraController.enabled = true;
        var stroke = _canvas.EndSession();
        _overlay.Hide();

        var symbol = _recognizer.Recognize(stroke);

        if (_symbolAbilityMap != null && _symbolAbilityMap.TryGetEntry(symbol, out var entry))
        {
            _canvas.RecolorStroke(entry.Color);
            StartCoroutine(ResolveRecognizedSymbol(entry.Hability));
        }
        else
        {
            _canvas.FadeOutAndClear();
        }
    }

    private IEnumerator ResolveRecognizedSymbol(ECorgiHability hability)
    {
        _corgiHabilityEvent.Invoke(hability);
        yield return new WaitForSeconds(_recognizedStrokeHoldDuration);
        _canvas.FadeOutAndClear();
    }
}
