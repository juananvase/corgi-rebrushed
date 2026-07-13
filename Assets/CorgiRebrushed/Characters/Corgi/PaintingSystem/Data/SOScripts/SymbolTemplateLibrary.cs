using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolTemplateLibrary", menuName = "Scriptable Objects/SymbolTemplateLibrary")]
[InlineEditor]
public class SymbolTemplateLibrary : ScriptableObject
{
    [Serializable]
    public class TemplateEntry
    {
        [field: SerializeField] public SymbolType Type { get; private set; }
        [field: SerializeField] public List<Vector2> Points { get; private set; }

        public TemplateEntry(SymbolType type, List<Vector2> points)
        {
            Type = type;
            Points = points;
        }
    }

    [field: SerializeField] public List<TemplateEntry> Templates { get; private set; } = new();
}
