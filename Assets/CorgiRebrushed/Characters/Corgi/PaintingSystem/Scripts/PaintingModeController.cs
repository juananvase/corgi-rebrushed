using System.Collections;
using GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events; //Reqquired for Audio

public class PaintingModeController : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] private InputActionAsset _inputActions;
    [SerializeField][FoldoutGroup("References")] private PaintOverlay _overlay;
    [SerializeField][FoldoutGroup("References")] private PaintCanvas _canvas;
    [SerializeField][FoldoutGroup("References")] private SymbolRecognizer _recognizer;
    [SerializeField][FoldoutGroup("References")] private ECorgiHabilityEventAsset _corgiHabilityEvent;
    [SerializeField][FoldoutGroup("References")] private SymbolAbilityMapSO _symbolAbilityMap;

    [SerializeField][FoldoutGroup("Data")] private float _recognizedStrokeHoldDuration = 1f;

    [ShowInInspector][FoldoutGroup("Testing")] private bool _isPainting;
    [ShowInInspector][FoldoutGroup("Testing")] private bool _isStroking;
    
    // Audio
    [FoldoutGroup("Audio")] public UnityEvent OnPaintingModeEnter;
    [FoldoutGroup("Audio")] public UnityEvent OnPaintingModeExit;
    [FoldoutGroup("Audio")] public UnityEvent OnSymbolConfirmed;
    [FoldoutGroup("Audio")] public UnityEvent OnSymbolFailed;
    // End Audio

    private ThirdPersonCameraController _cameraController;
    private InputAction _enterPaintModeAction;
    private InputAction _paintAction;

    private void Awake()
    {
        _cameraController = FindAnyObjectByType<ThirdPersonCameraController>();
        _enterPaintModeAction = InputSystem.actions.FindAction("EnterPaintMode");
        _paintAction = InputSystem.actions.FindAction("Paint");
    }

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && _enterPaintModeAction.inProgress)
            EnterPaintMode();
        else if (Mouse.current.rightButton.wasReleasedThisFrame && !_enterPaintModeAction.inProgress)
            ExitPaintMode();
        
        if (_isPainting)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && _paintAction.inProgress)
                BeginStroke();
            else if (Mouse.current.leftButton.wasReleasedThisFrame && !_paintAction.inProgress)
                PauseStroke();
        }
    }

    private void EnterPaintMode()
    {
        _isPainting = true;
        if (_cameraController != null) _cameraController.enabled = false;
        _overlay.Show();
        _canvas.PrepareSession();
        // Audio - 
        OnPaintingModeEnter?.Invoke();
        // End Audio
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
       //Audio
       OnPaintingModeExit?.Invoke(); // Audio — before state changes to still have context
       // End Audio
        _isPainting = false;
        _isStroking = false;
        if (_cameraController != null) _cameraController.enabled = true;
        var stroke = _canvas.EndSession();
        _overlay.Hide();

        var symbol = _recognizer.Recognize(stroke);

        if (_symbolAbilityMap != null && _symbolAbilityMap.TryGetEntry(symbol, out var entry))
        {
            _canvas.RecolorStroke(entry.Color);
            StartCoroutine(ResolveRecognizedSymbol(entry.Ability));
        }
        else
        {
            // Audio
            OnSymbolFailed?.Invoke();
            // End Audio
            _canvas.FadeOutAndClear();
        }
    }

    private IEnumerator ResolveRecognizedSymbol(ECorgiAbility ability)
    {
        // Audio
        OnSymbolConfirmed?.Invoke();
        // End Audio
        _corgiHabilityEvent.Invoke(ability);
        yield return new WaitForSeconds(_recognizedStrokeHoldDuration);
        _canvas.FadeOutAndClear();
    }
}
