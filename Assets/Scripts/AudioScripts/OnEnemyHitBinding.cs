using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays audio when an enemy receives damage.
/// Listens to Health.OnDamaged on all EnemyController GameObjects in the scene.
/// Finds enemies via EnemyController component instead of tags.
/// </summary>
[AddComponentMenu("Corgi Audio/On Enemy Hit Binding")]
public class OnEnemyHitBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event Reference — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _fmodEvent;   // enemy hit sound (e.g., Enemy/Hit-Cat)

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
    // Lifecycle: subscribe to every enemy's damage event
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        foreach (var health in _enemyHealthComponents)
        {
            if (health != null)
                health.OnDamaged.AddListener(OnEnemyHit);
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
                health.OnDamaged.RemoveListener(OnEnemyHit);
        }
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: enemy took damage — play the hit sound
    // ────────────────────────────────────────────────────────────
    private void OnEnemyHit(DamageInfo info)
    {
        // PlayOneShot — short hit reaction that doesn't need parameter updates.
        if (!_fmodEvent.IsNull)
            RuntimeManager.PlayOneShot(_fmodEvent);
    }
}