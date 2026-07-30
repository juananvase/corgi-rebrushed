using GameEvents;
using UnityEngine;

public class PickableTreat : Pickable
{
    [SerializeField] private float _healAmount;
    [SerializeField] private IntEventAsset _onTreatCollected;

    protected override void OnCollected(GameObject other)
    {
        base.OnCollected(other);
        
        Debug.Log(other.name + " collected");
        if (other.transform.root.TryGetComponent(out IHealable health))
        {
            health.Heal(new HealInfo(_healAmount, other, gameObject, gameObject, EHealType.Treat));
            _onTreatCollected.Invoke(1);
            Destroy(gameObject);
        }
    }
}
