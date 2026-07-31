// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — EnemyController.OnEnemySpotted aún no expone el evento.
// Descomentar cuando EnemyController exponga el UnityEvent.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays a stinger when an enemy spots the player.
// /// Listens to EnemyController.OnEnemySpotted UnityEvent.
// /// Uses FindObjectOfType to bind to the first active enemy controller.
// /// </summary>
// [AddComponentMenu("Corgi Audio/On Enemy Spotted Binding")]
// public class OnEnemySpottedBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;   // enemy spotted stinger (e.g., Music/EnemySpotted)
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
//             _enemy.OnEnemySpotted.AddListener(OnSpotted);
//     }
//
//     private void OnDisable()
//     {
//         if (_enemy != null)
//             _enemy.OnEnemySpotted.RemoveListener(OnSpotted);
//     }
//
//     private void OnSpotted()
//     {
//         if (!_fmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }