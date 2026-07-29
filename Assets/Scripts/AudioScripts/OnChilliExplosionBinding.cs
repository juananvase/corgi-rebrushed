// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — ChilliExplosion.OnChilliExplosionActivated aún no expone el evento.
// Descomentar cuando ChilliExplosion exponga el UnityEvent.
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays audio when the Chilli Explosion ability is activated.
// /// Listens to ChilliExplosion.OnChilliExplosionActivated UnityEvent.
// /// </summary>
// [AddComponentMenu("Corgi Audio/On Chilli Explosion Binding")]
// public class OnChilliExplosionBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;   // chilli explosion sound (e.g., Abilities/ChilliExplosion)
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
//             _chilli.OnChilliExplosionActivated.AddListener(OnExplosion);
//     }
//
//     private void OnDisable()
//     {
//         if (_chilli != null)
//             _chilli.OnChilliExplosionActivated.RemoveListener(OnExplosion);
//     }
//
//     private void OnExplosion()
//     {
//         if (!_fmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }