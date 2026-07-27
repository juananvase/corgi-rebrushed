using UnityEngine;

public interface IDamageable
{
    public bool IsAlive { get; }
    public void Damaged(DamageInfo damageInfo);
}
