using System;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] private InputActionAsset _inputActions;
    [SerializeField][FoldoutGroup("References")] private ThirdPersonCameraDataSO _characterCameraData;
    
    [SerializeField][FoldoutGroup("References")] private CinemachineCamera _camera;
    [SerializeField][FoldoutGroup("References")] private CinemachineOrbitalFollow _orbitalFollow;
    [SerializeField][FoldoutGroup("References")] private CinemachineInputAxisController _inputAxisController;
    
    private InputAction _zoomAction;

    private Vector2 _scrollDelta;

    private float _targetZoom;
    private float _currentZoom;

    private void OnEnable()
    {
        _inputActions.FindActionMap("CameraControl").Enable();
    }
    
    private void OnDisable()
    {
        _inputActions.FindActionMap("CameraControl").Disable();
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _camera = GetComponent<CinemachineCamera>();
        _orbitalFollow = GetComponentInChildren<CinemachineOrbitalFollow>();
        _inputAxisController = GetComponentInChildren<CinemachineInputAxisController>();
        
        _zoomAction = InputSystem.actions.FindAction("Zoom");
        _zoomAction.performed += HandleMouseScroll;

        _targetZoom = _currentZoom = _orbitalFollow.Radius;
        
        SetCameraSensitivity(_characterCameraData.SensitivityInX, _characterCameraData.SensitivityInY);
    }

    private void Update()
    {
        //ZoomInOut();
    }

    private void SetCameraSensitivity(float x, float y)
    {
        foreach (var c in _inputAxisController.Controllers)
        {
            if (c.Name == "Look Orbit X")
                c.Input.Gain = x;

            if (c.Name == "Look Orbit Y")
                c.Input.Gain = y*-1;
        }
    }
    
    private void HandleMouseScroll(InputAction.CallbackContext context)
    {
        _scrollDelta = context.ReadValue<Vector2>();
    }
    
    private void ZoomInOut()
    {
        if (_scrollDelta.y != 0f && _orbitalFollow)
        {
            _targetZoom = Mathf.Clamp(_orbitalFollow.Radius - _scrollDelta.y * _characterCameraData.ZoomSpeed, _characterCameraData.MinDistance, _characterCameraData.MaxDistance);
            _scrollDelta = Vector2.zero;
        }
        
        _currentZoom = Mathf.Lerp(_currentZoom, _targetZoom, Time.deltaTime * _characterCameraData.ZoomLerpSpeed);
        _orbitalFollow.Radius = _currentZoom;
    }
}
