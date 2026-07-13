using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SymbolRecognizer : MonoBehaviour
{
    [SerializeField][FoldoutGroup("Data")] private SymbolTemplateLibrary _templateLibrary;
    [SerializeField][FoldoutGroup("Data")] private float _minConfidence = 0.8f;

    [ShowInInspector][FoldoutGroup("Testing")] private List<Vector2> _lastStroke = new();
    [ShowInInspector][FoldoutGroup("Testing")] private SymbolType _templateTypeToRecord;

    public SymbolType Recognize(List<Vector2> points)
    {
        _lastStroke = points;

        if (points == null || points.Count < 2 || _templateLibrary == null)
        {
            Debug.Log("[SymbolRecognizer] Stroke received: sin suficientes puntos o sin template library → SymbolType.Unknown");
            return SymbolType.Unknown;
        }

        var candidate = OneDollarRecognizer.Normalize(points);
        var templates = _templateLibrary.Templates.Select(t => (t.Type, t.Points));
        var (type, score) = OneDollarRecognizer.Recognize(candidate, templates);

        if (score < _minConfidence)
        {
            Debug.Log($"[SymbolRecognizer] Stroke received: {points.Count} points → mejor match {type} (score {score:F2}, bajo el umbral) → SymbolType.Unknown");
            return SymbolType.Unknown;
        }

        Debug.Log($"[SymbolRecognizer] Stroke received: {points.Count} points → SymbolType.{type} (score {score:F2})");
        return type;
    }

    [Button("Test Recognize")]
    private void TestRecognize() => Recognize(_lastStroke);

    [Button("Save As Template")]
    private void SaveAsTemplate()
    {
        if (_lastStroke == null || _lastStroke.Count < 2)
        {
            Debug.LogWarning("[SymbolRecognizer] No hay un trazo válido para guardar. Dibujá un símbolo primero.");
            return;
        }
        if (_templateLibrary == null)
        {
            Debug.LogWarning("[SymbolRecognizer] No hay un SymbolTemplateLibrary asignado.");
            return;
        }

        var normalized = OneDollarRecognizer.Normalize(_lastStroke);
        _templateLibrary.Templates.Add(new SymbolTemplateLibrary.TemplateEntry(_templateTypeToRecord, normalized));

#if UNITY_EDITOR
        EditorUtility.SetDirty(_templateLibrary);
        AssetDatabase.SaveAssets();
#endif

        Debug.Log($"[SymbolRecognizer] Plantilla guardada para {_templateTypeToRecord} ({_templateLibrary.Templates.Count} en total).");
    }
}
