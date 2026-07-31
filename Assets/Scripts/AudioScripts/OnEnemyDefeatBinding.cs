using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays a defeat stinger when an enemy dies.
/// Listens to Health.OnDeath on all EnemyController GameObjects in the scene.
/// Finds enemies via EnemyController component instead of tags.
/// </summary>
[AddComponentMenu("Corgi Audio/On Enemy Defeat Binding")]
public class OnEnemyDefeatBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // enemy defeat stinger (e.g., Music/EnemyDefeat)

    // ────────────────────────────────────────────────────────────
    // All enemy Health components found at startup
    // ────────────────────────────────────────────────────────────
    private Health[] _enemyHealthComponents;

    // ────────────────────────────────────────────────────────────
    // Lifecycle: find all EnemyControllers and cache their Health
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        var controllers = FindObjectsOfType<EnemyController>();
        _enemyHealthComponents = new Health[controllers.Length];
        for (int i = 0; i < controllers.Length; i++)
            _enemyHealthComponents[i] = controllers[i].GetComponent<Health>();
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to every enemy's death event
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        foreach (var health in _enemyHealthComponents)
        {
            if (health != null)
                health.OnDeath.AddListener(OnEnemyDefeated);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe from all enemies to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        foreach (var health in _enemyHealthComponents)
        {
            if (health != null)
                health.OnDeath.RemoveListener(OnEnemyDefeated);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: an enemy was defeated — play the stinger
    // ────────────────────────────────────────────────────────────
    private void OnEnemyDefeated()
    {
        if (!_fmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_fmodEvent);
    }
}