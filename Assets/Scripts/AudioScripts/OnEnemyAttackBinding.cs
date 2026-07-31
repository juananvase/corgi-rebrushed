// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — EnemyController.OnEnemyAttack aún no expone el evento.
// Descomentar cuando EnemyController exponga el UnityEvent.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays audio when an enemy attacks the player.
// /// Listens to EnemyController.OnEnemyAttack UnityEvent.
// /// </summary>
// [AddComponentMenu("Corgi Audio/On Enemy Attack Binding")]
// public class OnEnemyAttackBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;   // enemy attack sound (e.g., Enemy/Attack)
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
//             _enemy.OnEnemyAttack.AddListener(OnAttack);
//     }
//
//     private void OnDisable()
//     {
//         if (_enemy != null)
//             _enemy.OnEnemyAttack.RemoveListener(OnAttack);
//     }
//
//     private void OnAttack()
//     {
//         if (!_fmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }