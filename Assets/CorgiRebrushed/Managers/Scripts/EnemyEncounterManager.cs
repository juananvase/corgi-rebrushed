using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyEncounterManager : MonoBehaviour
{
    public static EnemyEncounterManager instance { get; private set; }
    private void InitiateSinglenton()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }

    [field: SerializeField, FoldoutGroup("References")] public Transform PlayerTransform { get; private set; }
    
    private void Awake()
    {
        InitiateSinglenton();
    }
    
}
