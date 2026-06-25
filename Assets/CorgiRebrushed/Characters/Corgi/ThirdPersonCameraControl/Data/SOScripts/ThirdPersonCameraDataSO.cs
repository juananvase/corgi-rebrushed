using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ThirdPersonCameraDataSO", menuName = "Scriptable Objects/ThirdPersonCameraDataSO")]
[InlineEditor]
public class ThirdPersonCameraDataSO : ScriptableObject
{
    [Header("Camera Movement Variables")]
    [field: SerializeField] public float SensitivityInX { get; private set; } = 10f;
    [field: SerializeField] public float SensitivityInY { get; private set; } = 10f;
    
    [Header("Zoom Variables")]
    [field: SerializeField] public float ZoomSpeed { get; private set; } = 2f;
    [field: SerializeField] public float ZoomLerpSpeed { get; private set; } =10f;
    [field: SerializeField] public float MaxDistance { get; private set; } =3f;
    [field: SerializeField] public float MinDistance { get; private set; } =15f;
}
