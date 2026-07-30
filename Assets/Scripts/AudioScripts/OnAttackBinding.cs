using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Plays attack sounds for the Corgi's melee combo system.
/// Fires two separate FMOD events per attack step:
/// - BarkAttack: the Corgi's bark/vocalization
/// - Attack-Corgi: the whoosh/impact sound
/// Both receive the "AttackStep" parameter (0=first hit, 1=second, 2=third).
/// </summary>
[AddComponentMenu("Corgi Audio/On Attack Binding")]
public class OnAttackBinding : MonoBehaviour
{
    // ────────────────────────────────────────────────────────────
    // FMOD Event References — assigned in the Inspector
    // ────────────────────────────────────────────────────────────
    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _barkFmodEvent;    // bark/vocalization (e.g., Player/BarkAttack)

    [FoldoutGroup("FMOD")]
    [SerializeField] private EventReference _attackFmodEvent;  // whoosh/impact (e.g., Player/Attack-Corgi)

    // ────────────────────────────────────────────────────────────
    // Cached reference to the melee system
    // ────────────────────────────────────────────────────────────
    private Melee _melee;

    // ────────────────────────────────────────────────────────────
    // Lifecycle: cache the melee reference
    // ────────────────────────────────────────────────────────────
    private void Awake()
    {
        _melee = FindObjectOfType<Melee>();
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: subscribe to attack events
    // ────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (_melee != null)
            _melee.OnAttackPerformed.AddListener(HandleAttack);
    }

    // ────────────────────────────────────────────────────────────
    // Lifecycle: unsubscribe to prevent leaks
    // ────────────────────────────────────────────────────────────
    private void OnDisable()
    {
        if (_melee != null)
            _melee.OnAttackPerformed.RemoveListener(HandleAttack);
    }

    // ────────────────────────────────────────────────────────────
    // Event handler: attack performed — fire both FMOD events
    // Each event gets its own short-lived instance with the
    // AttackStep parameter set before starting. Instances are
    // released immediately after start — FMOD handles the tail.
    // ────────────────────────────────────────────────────────────
    private void HandleAttack(int comboStep)
    {
        // Bark / vocalization
        if (!_barkFmodEvent.IsNull)
        {
            var bark = RuntimeManager.CreateInstance(_barkFmodEvent);
            bark.setParameterByName("AttackStep", comboStep);
            bark.start();
            bark.release();
        }

        // Whoosh / impact
        if (!_attackFmodEvent.IsNull)
        {
            var attack = RuntimeManager.CreateInstance(_attackFmodEvent);
            attack.setParameterByName("AttackStep", comboStep);
            attack.start();
            attack.release();
        }
    }
}