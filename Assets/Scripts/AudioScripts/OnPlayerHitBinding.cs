// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — Health.OnDamaged en el jugador necesita binding separado.
// Descomentar cuando se quiera activar audio de jugador recibiendo daño.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays audio when the player receives damage.
// /// Listens to Health.OnDamaged on the player GameObject (tag "Player").
// /// Covers HitReceived-Corgi and BarkHurt SFX.
// /// </summary>
// [AddComponentMenu("Corgi Audio/On Player Hit Binding")]
// public class OnPlayerHitBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;   // player hit sound (e.g., Player/HitReceived)
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
//             _playerHealth.OnDamaged.AddListener(OnPlayerHit);
//     }
//
//     private void OnDisable()
//     {
//         if (_playerHealth != null)
//             _playerHealth.OnDamaged.RemoveListener(OnPlayerHit);
//     }
//
//     private void OnPlayerHit(DamageInfo info)
//     {
//         if (!_fmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }