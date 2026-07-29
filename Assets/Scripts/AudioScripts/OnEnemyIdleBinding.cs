// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — EnemyController.OnEnemyIdle aún no expone el evento.
// Descomentar cuando EnemyController exponga el UnityEvent.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays idle/ambient sounds for enemies (e.g., cat meowing, hissing).
// /// Listens to EnemyController.OnEnemyIdle UnityEvent.
// /// Covers MeowingHiss and MeowingAngry SFX.
// /// </summary>
// [AddComponentMenu("Corgi Audio/On Enemy Idle Binding")]
// public class OnEnemyIdleBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;   // enemy idle sound (e.g., Enemy/Idle)
//
//     private EnemyController _enemy;
//
//     private void Awake()
//     {
//         _enemy = FindObjectOfType<EnemyController>();
//     }
//
//     private void OnEnable()
//     {
//         if (_enemy != null)
//             _enemy.OnEnemyIdle.AddListener(OnIdle);
//     }
//
//     private void OnDisable()
//     {
//         if (_enemy != null)
//             _enemy.OnEnemyIdle.RemoveListener(OnIdle);
//     }
//
//     private void OnIdle()
//     {
//         if (!_fmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }