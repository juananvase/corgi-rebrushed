using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbilityInvokeable : MonoBehaviour, IDamageable
{
    [SerializeField, FoldoutGroup("References")] protected AbilitiesDataSO _abilitiesData;
    [SerializeField, BoxGroup("Events")]private UnityEvent<DamageInfo> OnDamaged;
    public GameObject Owner { protected get; set; }
    public bool IsAlive { get; }

    [Button("Print Owner")]
    private void PrintOwner()
    {
        Debug.Log(Owner.gameObject.name);
    }
    public virtual void Damaged(DamageInfo damageInfo)
    {
        OnDamaged.Invoke(damageInfo);
    }
}
