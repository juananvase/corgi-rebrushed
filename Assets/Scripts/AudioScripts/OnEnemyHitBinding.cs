// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — Health.OnDamaged en enemigos necesita binding separado.
// Descomentar cuando se quiera activar audio de enemigo recibiendo daño.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays audio when an enemy receives damage.
// /// Listens to Health.OnDamaged on all enemy GameObjects (tag "Enemy").
// /// Covers MeowingHurt and MeowingHiss/Hit-Cat SFX.
// /// </summary>
// [AddComponentMenu("Corgi Audio/On Enemy Hit Binding")]
// public class OnEnemyHitBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;   // enemy hit sound (e.g., Enemy/Hit)
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
//                 health.OnDamaged.AddListener(OnEnemyHit);
//         }
//     }
//
//     private void OnDisable()
//     {
//         foreach (var health in _enemyHealthComponents)
//         {
//             if (health != null)
//                 health.OnDamaged.RemoveListener(OnEnemyHit);
//         }
//     }
//
//     private void OnEnemyHit(DamageInfo info)
//     {
//         if (!_fmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }