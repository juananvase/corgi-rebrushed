using UnityEngine;
using Sirenix.OdinInspector;

public class Health : MonoBehaviour
{
    [SerializeField, BoxGroup("stats")] private float _currentHealth = 100f;
    [SerializeField, BoxGroup("stats")] private float _maxHealth = 100f;

    [BoxGroup("Debug"), ShowInInspector] float Percentage => _currentHealth / _maxHealth;
    [BoxGroup("Debug"), ShowInInspector] bool IsAlive => _currentHealth >= 1f;

    public void Damage(DamageInfo damageInfo)
    {
        if (!IsAlive) return;
        if (damageInfo.Amount < 1f) return;
        _currentHealth -= damageInfo.Amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
    }
    [ContextMenu("Damage Test 10%"), Button("Damage Test 10%")]
    public void DamageTest()
    {
        DamageInfo damageInfo = new DamageInfo(_maxHealth * 0.1f, gameObject, gameObject, gameObject, DamageType.Normal);
        Damage(damageInfo);
    }
}

public class DamageInfo
{
    public DamageInfo(float amount, GameObject victim, GameObject source, GameObject instigator, DamageType damageType)
    {
        Amount = amount;
        Victim = victim;
        source = source;
        instigator = instigator;
        DamageType = damageType;
    }
    
    public float Amount {  get; set; }
    public GameObject Victim {  get; set; }
    public GameObject Source {  get; set; }
    public GameObject Instigator {  get; set; }
    public DamageType DamageType {  get; set; }
}

public enum DamageType
{
    Normal,
    Fire,
    Water,
}
