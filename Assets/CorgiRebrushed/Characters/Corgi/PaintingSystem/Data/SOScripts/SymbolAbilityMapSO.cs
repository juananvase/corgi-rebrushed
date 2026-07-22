using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolAbilityMapSO", menuName = "Scriptable Objects/SymbolAbilityMapSO")]
[InlineEditor]
public class SymbolAbilityMapSO : ScriptableObject
{
    [Serializable]
    public struct Entry
    {
        [field: SerializeField] public SymbolType Symbol { get; private set; }
        [field: SerializeField] public ECorgiHability Hability { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }

        public Entry(SymbolType symbol, ECorgiHability hability, Color color)
        {
            Symbol = symbol;
            Hability = hability;
            Color = color;
        }
    }

    [field: SerializeField] public List<Entry> Mappings { get; private set; } = new();

    public bool TryGetEntry(SymbolType symbol, out Entry entry)
    {
        foreach (var candidate in Mappings)
        {
            if (candidate.Symbol == symbol)
            {
                entry = candidate;
                return true;
            }
        }

        entry = default;
        return false;
    }
}
