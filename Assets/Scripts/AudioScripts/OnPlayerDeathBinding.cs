// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — Health.OnDeath aún no expone el evento.
// Descomentar cuando Health.cs exponga el UnityEvent OnDeath.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays a death stinger when the player dies.
// /// Listens to Health.OnDeath on the player's Health component.
// /// Uses FindObjectOfType filtered by tag "Player".
// /// </summary>
// [AddComponentMenu("Corgi Audio/On Player Death Binding")]
// public class OnPlayerDeathBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;   // player death stinger (e.g., Player/Death)
//
//     private Health _playerHealth;
//
//     private void Awake()
//     {
//         var player = GameObject.FindWithTag("Player");
//         if (player != null)
//             _playerHealth = player.GetComponent<Health>();
//     }
//
//     private void OnEnable()
//     {
//         if (_playerHealth != null)
//             _playerHealth.OnDeath.AddListener(OnPlayerDeath);
//     }
//
//     private void OnDisable()
//     {
//         if (_playerHealth != null)
//             _playerHealth.OnDeath.RemoveListener(OnPlayerDeath);
//     }
//
//     private void OnPlayerDeath()
//     {
//         if (!_fmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }