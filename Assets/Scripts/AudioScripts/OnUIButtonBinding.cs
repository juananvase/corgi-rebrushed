// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — El sistema de UI aún no existe en el proyecto.
// Descomentar cuando se implemente un UIManager o sistema de menús.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays UI sounds for button interactions.
// /// Covers ButtonClick and ButtonHover SFX.
// /// Requires a UIManager or button event system to be implemented.
// /// </summary>
// [AddComponentMenu("Corgi Audio/On UI Button Binding")]
// public class OnUIButtonBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _clickFmodEvent;   // button click sound (e.g., UI/ButtonClick)
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _hoverFmodEvent;   // button hover sound (e.g., UI/ButtonHover)
//
//     // TODO: Subscribe to UI events when UIManager is implemented.
//     // Expected pattern:
//     //   UIManager.OnButtonClicked.AddListener(OnClick);
//     //   UIManager.OnButtonHovered.AddListener(OnHover);
//
//     private void OnClick()
//     {
//         if (!_clickFmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_clickFmodEvent);
//     }
//
//     private void OnHover()
//     {
//         if (!_hoverFmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_hoverFmodEvent);
//     }
// }