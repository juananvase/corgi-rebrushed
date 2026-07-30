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
    
    private void Awake()
    {
        InitiateSinglenton();
    }
    
}
