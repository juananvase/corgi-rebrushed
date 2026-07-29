// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — AbilityManager.OnFirstAbilityUnlocked aún no expone el evento.
// Descomentar cuando AbilityManager exponga el UnityEvent.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays a stinger when the player unlocks their first ability.
// /// Listens to AbilityManager.OnFirstAbilityUnlocked UnityEvent.
// /// </summary>
// [AddComponentMenu("Corgi Audio/On First Ability Binding")]
// public class OnFirstAbilityBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;   // ability unlock stinger (e.g., Music/AbilityUnlocked)
//
//     private AbilityManager _manager;
//
//     private void Awake()
//     {
//         _manager = FindObjectOfType<AbilityManager>();
//     }
//
//     private void OnEnable()
//     {
//         if (_manager != null)
//             _manager.OnFirstAbilityUnlocked.AddListener(OnFirstAbility);
//     }
//
//     private void OnDisable()
//     {
//         if (_manager != null)
//             _manager.OnFirstAbilityUnlocked.RemoveListener(OnFirstAbility);
//     }
//
//     private void OnFirstAbility()
//     {
//         if (!_fmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }