// ──────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — ForcesBasedCharacterMovementController.OnFootstep ya no expone el evento en la rama actual.
// Restaurar cuando el controlador de movimiento vuelva a exponer los eventos.
// ──────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
// 
// [AddComponentMenu("Corgi Audio/On Footstep Binding")]
// public class OnFootstepBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;
// 
//     private ForcesBasedCharacterMovementController _controller;
// 
//     private void Awake()
//     {
//         _controller = FindObjectOfType<ForcesBasedCharacterMovementController>();
//     }
// 
//     private void OnEnable()
//     {
//         if (_controller != null)
//             _controller.OnFootstep.AddListener(OnFootstepRaised);
//     }
// 
//     private void OnDisable()
//     {
//         if (_controller != null)
//             _controller.OnFootstep.RemoveListener(OnFootstepRaised);
//     }
// 
//     private void OnFootstepRaised(float speed)
//     {
//         if (_fmodEvent.IsNull) return;
//         RuntimeManager.StudioSystem.setParameterByName("PlayerSpeed", speed);
//         RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }
