using Sirenix.OdinInspector;
using UnityEngine;

public abstract class AbilityInvokeable : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")] protected AbilitiesDataSO _abilitiesData;
    public GameObject Owner { protected get; set; }

    [Button("Print Owner")]
    private void PrintOwner()
    {
        Debug.Log(Owner.gameObject.name);
    }
}
