
//using UnityEngine;

//public class SceneLoadButton : MonoBehaviour
//{
//    [SerializeField] private string targetSceneName;

//    public void LoadTargetScene()
//    {
//        SceneLoader.LoadScene(targetSceneName);
//    }
//}


using UnityEngine;

public class SceneLoadButton : MonoBehaviour
{
    [SerializeField] private string targetSceneName;

    public void LoadTargetScene()
    {
        Debug.Log("Launch Mission clicked!");

        Debug.Log("Target Scene: " + targetSceneName);

        SceneLoader.LoadScene(targetSceneName);
    }
}
