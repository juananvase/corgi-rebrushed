// ──────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — PaintingModeController.OnSymbolConfirmed/Failed ya no exponen los eventos en la rama actual.
// Restaurar cuando PaintingModeController vuelva a exponer los eventos.
// ──────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
// 
// [AddComponentMenu("Corgi Audio/On Symbol Confirmed Binding")]
// public class OnSymbolConfirmedBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _confirmedFmodEvent;
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _failedFmodEvent;
// 
//     private PaintingModeController _controller;
// 
//     private void Awake()
//     {
//         _controller = FindObjectOfType<PaintingModeController>();
//     }
// 
//     private void OnEnable()
//     {
//         if (_controller != null)
//         {
//             _controller.OnSymbolConfirmed.AddListener(OnConfirmed);
//             _controller.OnSymbolFailed.AddListener(OnFailed);
//         }
//     }
// 
//     private void OnDisable()
//     {
//         if (_controller != null)
//         {
//             _controller.OnSymbolConfirmed.RemoveListener(OnConfirmed);
//             _controller.OnSymbolFailed.RemoveListener(OnFailed);
//         }
//     }
// 
//     private void OnConfirmed()
//     {
//         if (!_confirmedFmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_confirmedFmodEvent);
//     }
// 
//     private void OnFailed()
//     {
//         if (!_failedFmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_failedFmodEvent);
//     }
// }
