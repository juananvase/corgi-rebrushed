using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "HitFeedbackDataSO", menuName = "Scriptable Objects/HitFeedbackDataSO")]
[InlineEditor]
public class HitFeedbackDataSO : ScriptableObject
{
    [field: SerializeField, FoldoutGroup("Hit Flash")] public Color FlashColor { get; private set; }
    [field: SerializeField, FoldoutGroup("Hit Flash")] public float FlashDuration { get; private set; }
    
    [field: SerializeField, FoldoutGroup("Hit Pause")] public float PauseDuration { get; private set; }
    
    [field: SerializeField, FoldoutGroup("Push Back")] public float PushBackForce { get; private set; }
    
}
