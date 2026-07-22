using Sirenix.OdinInspector;
using UnityEngine;

public class AbilitiyManager : MonoBehaviour
{
    [field: SerializeField][FoldoutGroup("References")] public Transform CharacterObject { get; private set; }
    [field: SerializeField][FoldoutGroup("References")] public AbilitiesDataSO AbilitiesData { get; private set; }
}
