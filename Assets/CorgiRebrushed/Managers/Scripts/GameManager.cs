using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    
    private void InitiateSinglenton()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }
    
    [field: SerializeField, FoldoutGroup("References")] public Transform PlayerTransform { get; private set; }
    [field: SerializeField, FoldoutGroup("References")] public CinemachineImpulseSource PlayerCameraCinemachineImpulseSource { get; private set; }
    
    private void Awake()
    {
        InitiateSinglenton();
    }
}
