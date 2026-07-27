using UnityEngine;
using Sirenix.OdinInspector;

public class Health : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField, BoxGroup("stats")] private float _currentHealth = 100f;
    [SerializeField, BoxGroup("stats")] private float _maxHealth = 100f;

    [BoxGroup("Debug"), ShowInInspector] public float Percentage => _currentHealth / _maxHealth;
    [BoxGroup("Debug"), ShowInInspector] public bool IsAlive => _currentHealth >= 1f;

    public void Damaged(DamageInfo damageInfo)
    {
        if (!IsAlive) return;
        if (damageInfo.Amount < 1f) return;
        _currentHealth -= damageInfo.Amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
    }
    [ContextMenu("Damage Test 10%"), Button("Damage Test 10%")]
    public void DamageTest()
    {
        DamageInfo damageInfo = new DamageInfo(_maxHealth * 0.1f, gameObject, gameObject, gameObject, EDamageType.Normal);
        Damaged(damageInfo);
    }
    
    public void Heal(HealInfo healInfo)
    {
        if (!IsAlive) return;
        if (healInfo.Amount < 1f) return;
        _currentHealth += healInfo.Amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
    }
    [ContextMenu("Heal Test 10%"), Button("Heal Test 10%")]
    public void HealTest()
    {
        HealInfo healInfo = new HealInfo(_maxHealth * 0.1f, gameObject, gameObject, gameObject, EHealType.Normal);
        Heal(healInfo);
    }
}

public class DamageInfo
{
    public DamageInfo(float amount, GameObject victim, GameObject source, GameObject instigator, EDamageType eDamageType)
    {
        Amount = amount;
        Victim = victim;
        Source = source;
        Instigator = instigator;
        EDamageType = eDamageType;
    }
    
    public float Amount {  get; set; }
    public GameObject Victim {  get; set; }
    public GameObject Source {  get; set; }
    public GameObject Instigator {  get; set; }
    public EDamageType EDamageType {  get; set; }
}

public class HealInfo
{
    public HealInfo(float amount, GameObject target, GameObject source, GameObject instigator, EHealType eHealType)
    {
        Amount = amount;
        Target = target;
        Source = source;
        Instigator = instigator;
        EHealType = eHealType;
    }

    public float Amount {  get; set; }
    public GameObject Target {  get; set; }
    public GameObject Source {  get; set; }
    public GameObject Instigator {  get; set; }
    public EHealType EHealType {  get; set; }
}

public enum EDamageType
{
    Normal,
    Brush,
    WaterJump,
    Tree,
    Chilli,
    Scratch
}

public enum EHealType
{
    Normal
}
