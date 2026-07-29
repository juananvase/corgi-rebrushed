// ──────────────────────────────────────────────────────────────
// DESACTIVADO TEMPORALMENTE — No se ha identificado el sistema de spawn correspondiente.
// Descomentar cuando se defina qué entidad spawnea (jugador, enemigo, coleccionable).
// ──────────────────────────────────────────────────────────────
// using FMODUnity;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// /// <summary>
// /// Plays audio when an entity spawns in the scene.
// /// TODO: Define the source system — could be player respawn, enemy spawn,
// /// or collectible/ability object spawn. Subscribe to the appropriate event.
// /// </summary>
// [AddComponentMenu("Corgi Audio/On Spawn Binding")]
// public class OnSpawnBinding : MonoBehaviour
// {
//     [FoldoutGroup("FMOD")]
//     [SerializeField] private EventReference _fmodEvent;   // spawn sound (e.g., Player/Spawn)
//
//     // TODO: Subscribe to spawn events when the spawning system is identified.
//     // Expected pattern:
//     //   SpawnManager.OnEntitySpawned.AddListener(OnSpawn);
//
//     private void OnSpawn()
//     {
//         if (!_fmodEvent.IsNull)
//             RuntimeManager.PlayOneShot(_fmodEvent);
//     }
// }