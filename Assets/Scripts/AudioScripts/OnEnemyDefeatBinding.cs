// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — Health.OnDeath aún no expone el evento.
// Descomentar cuando Health.cs exponga el UnityEvent OnDeath.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays a defeat stinger when an enemy dies.
// /// Listens to Health.OnDeath on enemy GameObjects.
// /// Finds all Health components in the scene and subscribes to
// /// enemies only (tag "Enemy").
// /// </summary>
// [AddComponentMenu("Corgi Audio/On Enemy Defeat Binding")]
// public class OnEnemyDefeatBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;   // enemy defeat stinger (e.g., Music/EnemyDefeat)
//
//     private Health[] _enemyHealthComponents;
//
//     private void Awake()
//     {
//         var enemies = GameObject.FindGameObjectsWithTag("Enemy");
//         _enemyHealthComponents = new Health[enemies.Length];
//         for (int i = 0; i < enemies.Length; i++)
//             _enemyHealthComponents[i] = enemies[i].GetComponent<Health>();
//     }
//
//     private void OnEnable()
//     {
//         foreach (var health in _enemyHealthComponents)
//         {
//             if (health != null)
//                 health.OnDeath.AddListener(OnEnemyDefeated);
//         }
//     }
//
//     private void OnDisable()
//     {
//         foreach (var health in _enemyHealthComponents)
//         {
//             if (health != null)
//                 health.OnDeath.RemoveListener(OnEnemyDefeated);
//         }
//     }
//
//     private void OnEnemyDefeated()
//     {
//         if (!_fmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }