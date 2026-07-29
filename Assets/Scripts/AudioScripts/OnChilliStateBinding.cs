// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — ChilliExplosion.OnChilliStateEnter/Exit aún no exponen los eventos.
// Descomentar cuando ChilliExplosion exponga los UnityEvents.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays stingers when the Chilli speed-boost state starts and ends.
// /// Listens to ChilliExplosion.OnChilliStateEnter and OnChilliStateExit UnityEvents.
// /// </summary>
// [AddComponentMenu("Corgi Audio/On Chilli State Binding")]
// public class OnChilliStateBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _enterFmodEvent;   // chilli state enter stinger (e.g., Abilities/ChilliStateEnter)
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _exitFmodEvent;    // chilli state exit stinger (e.g., Abilities/ChilliStateExit)
//
//     private ChilliExplosion _chilli;
//
//     private void Awake()
//     {
//         _chilli = FindObjectOfType<ChilliExplosion>();
//     }
//
//     private void OnEnable()
//     {
//         if (_chilli != null)
//         {
//             _chilli.OnChilliStateEnter.AddListener(OnEnter);
//             _chilli.OnChilliStateExit.AddListener(OnExit);
//         }
//     }
//
//     private void OnDisable()
//     {
//         if (_chilli != null)
//         {
//             _chilli.OnChilliStateEnter.RemoveListener(OnEnter);
//             _chilli.OnChilliStateExit.RemoveListener(OnExit);
//         }
//     }
//
//     private void OnEnter()
//     {
//         if (!_enterFmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_enterFmodEvent);
//     }
//
//     private void OnExit()
//     {
//         if (!_exitFmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_exitFmodEvent);
//     }
// }