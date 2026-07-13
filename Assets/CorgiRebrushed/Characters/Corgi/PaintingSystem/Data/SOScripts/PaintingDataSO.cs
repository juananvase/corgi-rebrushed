using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PaintingDataSO", menuName = "Scriptable Objects/PaintingDataSO")]
[InlineEditor]
public class PaintingDataSO : ScriptableObject
{
    [Header("Overlay")]
    [field: SerializeField] public Color OverlayColor { get; private set; } = new Color(0f, 0f, 0f, 0.7f);
    [field: SerializeField] public float OverlayFadeDuration { get; private set; } = 0.2f;

    [Header("Brush")]
    [field: SerializeField] public Color BrushColor { get; private set; } = Color.white;
    [field: SerializeField] public int BrushSize { get; private set; } = 5;
}
