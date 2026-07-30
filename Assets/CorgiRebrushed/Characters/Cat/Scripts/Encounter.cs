using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Encounter : SerializedMonoBehaviour
{
    [OdinSerialize] public List<EnemyController> ActiveEnemies { get; private set; } = new List<EnemyController>();
    [SerializeField] private UnityEvent OnFinishEncounter;


    private void Start()
    {
        EnemyEncounterManager.instance.EncounterRegister(this);
    }

    private void Update()
    {
        if (ActiveEnemies.Count == 0)
        {
            FinishEncounter();
        }
    }

    private void FinishEncounter()
    {
        EnemyEncounterManager.instance.EncounterDeregister(this);
        OnFinishEncounter.Invoke();
        //TODO Change to explorationMusic
    }

    public void EnemyRegister(EnemyController target)
    {
        ActiveEnemies.Add(target);
    }
    
    public void EnemyDeregister(EnemyController target)
    {
        ActiveEnemies.Remove(target);
    }
}
