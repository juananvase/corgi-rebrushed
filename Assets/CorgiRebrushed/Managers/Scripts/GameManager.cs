using System.Collections;
using GameEvents;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [field: SerializeField, FoldoutGroup("GameEvents")] private IntEventAsset _onTreatCollected;
    
    public static int CurrentLevelIndex = 0;

    [ShowInInspector, FoldoutGroup("Test")] public int TreatCount { get; private set; } = 0;
    
    private void Awake()
    {
        InitiateSinglenton();

        if (SceneManager.GetActiveScene().buildIndex > 4) return;
        CurrentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }
    
    private void OnEnable()
    {
        _onTreatCollected.OnInvoked.AddListener(UpdateTreatCount);
    }

    private void OnDisable()
    {
        _onTreatCollected.OnInvoked.RemoveListener(UpdateTreatCount);
    }

    private void UpdateTreatCount(int context)
    {
        TreatCount++;
    }

    public void Respawn()
    {
        LoadSceneByIndex(CurrentLevelIndex);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(LoadSceneRoutine(sceneIndex));
    }

    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        // 1. Begin loading the scene asynchronously in the background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // 2. Wait until the asynchronous scene loading is completely done
        while (!asyncLoad.isDone)
        {
            // Optional: Read asyncLoad.progress here to update a slider loading bar (0.0 to 1.0)
            // float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            
            // Wait for the next frame before checking the completion status again
            yield return null; 
        }
    }
}
