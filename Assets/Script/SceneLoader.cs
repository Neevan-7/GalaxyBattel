using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static string targetScene;

    public static void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Target scene name is empty!");
            return;
        }

        targetScene = sceneName;

        Debug.Log("TARGET SCENE SAVED: " + targetScene);

        SceneManager.LoadScene("LoadingScene");
    }

    public static string GetTargetScene()
    {
        Debug.Log("GET TARGET SCENE: " + targetScene);
        return targetScene;
    }
}