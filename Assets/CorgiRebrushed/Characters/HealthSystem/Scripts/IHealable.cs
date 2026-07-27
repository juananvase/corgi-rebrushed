using UnityEngine;

public interface IHealable
{
    public bool IsAlive { get; }
    public void Heal(HealInfo healInfo);
}
