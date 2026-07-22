using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilitiesDataSO", menuName = "Scriptable Objects/AbilitiesDataSO")]
public class AbilitiesDataSO : ScriptableObject
{
    [field: SerializeField, FoldoutGroup("Water Impulse")] public float ImpulseForce { get; private set; }
}
