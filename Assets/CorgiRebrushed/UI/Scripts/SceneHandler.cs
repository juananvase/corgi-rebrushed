using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
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

    public void Quit()
    {
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }
}
