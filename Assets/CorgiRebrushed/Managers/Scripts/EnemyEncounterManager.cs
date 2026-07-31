using System;
using System.Collections.Generic;
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
    public List<Encounter> ActiveEncounters { get; private set; } = new List<Encounter>();
    
    private void Awake()
    {
        InitiateSinglenton();
    }

    private void Update()
    {
        if (ActiveEncounters.Count == 0)
        {
            //All Encounters done
        }
    }
    
    public void EncounterRegister(Encounter target)
    {
        ActiveEncounters.Add(target);
    }
    
    public void EncounterDeregister(Encounter target)
    {
        ActiveEncounters.Remove(target);
    }
    
}
