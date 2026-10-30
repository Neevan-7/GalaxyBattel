
//using System.Collections;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class LoadingScreenManager : MonoBehaviour
//{
//    [Header("UI References")]
//    [SerializeField] private Slider progressBar;
//    [SerializeField] private TMP_Text percentageText;
//    [SerializeField] private TMP_Text statusText;
//    [SerializeField] private TMP_Text tipText;

//    [Header("Loading Settings")]
//    [SerializeField] private float minimumDisplayTime = 3.5f;
//    [SerializeField] private float fillSpeed = 0.25f;

//    private float displayedProgress = 0f;

//    private readonly string[] tips =
//    {
//        "Destroy enemy ships to earn rewards!",
//        "Upgrade your ship before harder waves.",
//        "Watch out for boss ships in later levels!",
//        "Collect power-ups to improve your firepower!"
//    };

//    private void Start()
//    {
//        Debug.LogError("========== LOADING SCENE MANAGER RUNNING ==========");
//        Debug.Log("LOADING SCREEN STARTED!");

//        // Get selected gameplay scene
//        string sceneName = SceneLoader.GetTargetScene();

//        Debug.Log("TARGET GAMEPLAY SCENE: " + sceneName);

//        // Validate UI references
//        if (progressBar == null ||
//            percentageText == null ||
//            statusText == null)
//        {
//            Debug.LogError("Loading UI references are missing!");
//            return;
//        }

//        // Initialize progress bar
//        progressBar.minValue = 0f;
//        progressBar.maxValue = 1f;
//        progressBar.value = 0f;

//        percentageText.text = "0%";

//        if (tipText != null)
//        {
//            tipText.text =
//                tips[Random.Range(0, tips.Length)];
//        }

//        // Validate target scene
//        if (string.IsNullOrEmpty(sceneName))
//        {
//            statusText.text = "No target scene selected!";
//            Debug.LogError("Target scene is empty!");
//            return;
//        }

//        StartCoroutine(LoadTargetScene(sceneName));
//    }

//    //private IEnumerator LoadTargetScene(string sceneName)
//    //{
//    //    float startTime = Time.unscaledTime;

//    //    statusText.text = "Initializing spaceship...";

//    //    AsyncOperation operation =
//    //        SceneManager.LoadSceneAsync(sceneName);

//    //    if (operation == null)
//    //    {
//    //        statusText.text = "Unable to load level!";
//    //        Debug.LogError("Scene loading operation failed!");
//    //        yield break;
//    //    }

//    //    operation.allowSceneActivation = false;

//    //    Debug.Log("ASYNC SCENE LOADING STARTED: " + sceneName);

//    //    // Load scene and smoothly update progress
//    //    while (operation.progress < 0.9f)
//    //    {
//    //        float actualProgress =
//    //            Mathf.Clamp01(operation.progress / 0.9f);

//    //        displayedProgress = Mathf.MoveTowards(
//    //            displayedProgress,
//    //            actualProgress,
//    //            fillSpeed * Time.unscaledDeltaTime
//    //        );

//    //        UpdateUI(displayedProgress);

//    //        statusText.text = "Loading galaxy...";

//    //        yield return null;
//    //    }

//    //    // Wait for minimum display duration
//    //    while (Time.unscaledTime - startTime <
//    //           minimumDisplayTime)
//    //    {
//    //        displayedProgress = Mathf.MoveTowards(
//    //            displayedProgress,
//    //            0.9f,
//    //            fillSpeed * Time.unscaledDeltaTime
//    //        );

//    //        UpdateUI(displayedProgress);

//    //        statusText.text = "Preparing weapons systems...";

//    //        yield return null;
//    //    }

//    //    // Complete progress animation
//    //    while (displayedProgress < 1f)
//    //    {
//    //        displayedProgress = Mathf.MoveTowards(
//    //            displayedProgress,
//    //            1f,
//    //            fillSpeed * Time.unscaledDeltaTime
//    //        );

//    //        UpdateUI(displayedProgress);

//    //        yield return null;
//    //    }

//    //    statusText.text = "Ready for launch!";

//    //    yield return new WaitForSecondsRealtime(0.3f);

//    //    Debug.Log("LOADING COMPLETE - ACTIVATING GAMEPLAY");

//    //    operation.allowSceneActivation = true;
//    //}

//    private IEnumerator LoadTargetScene(string sceneName)
//    {
//        float startTime = Time.unscaledTime;

//        // Initial status
//        statusText.text = "Initializing spaceship...";
//        displayedProgress = 0f;
//        UpdateUI(displayedProgress);

//        // Validate target scene
//        if (string.IsNullOrEmpty(sceneName))
//        {
//            statusText.text = "Unable to load level!";
//            Debug.LogError("Target scene name is empty!");
//            yield break;
//        }

//        // Start asynchronous scene loading
//        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

//        if (operation == null)
//        {
//            statusText.text = "Unable to load level!";
//            Debug.LogError("Scene loading operation failed!");
//            yield break;
//        }

//        // Prevent automatic scene activation
//        operation.allowSceneActivation = false;

//        Debug.Log("ASYNC SCENE LOADING STARTED: " + sceneName);
//        Debug.Log("Initial loading progress: " + operation.progress);

//        // STEP 1: Load scene and smoothly update progress
//        while (operation.progress < 0.9f)
//        {
//            float actualProgress = Mathf.Clamp01(
//                operation.progress / 0.9f
//            );

//            displayedProgress = Mathf.MoveTowards(
//                displayedProgress,
//                actualProgress,
//                fillSpeed * Time.unscaledDeltaTime
//            );

//            UpdateUI(displayedProgress);

//            statusText.text = "Loading galaxy...";

//            if (Time.frameCount % 30 == 0)
//            {
//                Debug.Log(
//                    "Loading progress: " +
//                    (operation.progress * 100f / 0.9f).ToString("F1") + "%"
//                );
//            }


//            yield return null;
//        }

//        Debug.Log("GAMEPLAY SCENE READY!");

//        // STEP 2: Wait for minimum display duration
//        while (Time.unscaledTime - startTime < minimumDisplayTime)
//        {
//            displayedProgress = Mathf.MoveTowards(
//                displayedProgress,
//                0.9f,
//                fillSpeed * Time.unscaledDeltaTime
//            );

//            UpdateUI(displayedProgress);

//            statusText.text = "Preparing weapons systems...";

//            yield return null;
//        }

//        // STEP 3: Complete progress animation
//        while (displayedProgress < 1f)
//        {
//            displayedProgress = Mathf.MoveTowards(
//                displayedProgress,
//                1f,
//                fillSpeed * Time.unscaledDeltaTime
//            );

//            UpdateUI(displayedProgress);

//            yield return null;
//        }

//        // STEP 4: Final loading message
//        statusText.text = "Ready for launch!";

//        Debug.Log("LOADING COMPLETE - ACTIVATING GAMEPLAY");

//        yield return new WaitForSecondsRealtime(0.3f);

//        // STEP 5: Activate gameplay scene
//        Debug.Log("ACTIVATING GAMEPLAY SCENE!");

//        operation.allowSceneActivation = true;
//    }

//    private void UpdateUI(float progress)
//    {
//        progressBar.value = progress;

//        percentageText.text =
//            Mathf.RoundToInt(progress * 100f) + "%";
//    }
//}





using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text percentageText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text tipText;

    [Header("Loading Settings")]
    [SerializeField] private float minimumDisplayTime = 3.5f;
    [SerializeField] private float fillSpeed = 0.25f;

    private float displayedProgress = 0f;

    private readonly string[] tips =
    {
        "Destroy enemy ships to earn rewards!",
        "Upgrade your ship before harder waves.",
        "Watch out for boss ships in later levels!",
        "Collect power-ups to improve your firepower!"
    };

    private void Start()
    {
        if (progressBar == null ||
            percentageText == null ||
            statusText == null)
        {
            Debug.LogError("Loading UI references are missing!");
            return;
        }

        progressBar.minValue = 0f;
        progressBar.maxValue = 1f;
        progressBar.value = 0f;

        if (tipText != null)
            tipText.text = tips[Random.Range(0, tips.Length)];

        string sceneName = SceneLoader.GetTargetScene();

        if (string.IsNullOrEmpty(sceneName))
        {
            statusText.text = "No target scene selected!";
            return;
        }

        StartCoroutine(LoadTargetScene(sceneName));
    }

    private IEnumerator LoadTargetScene(string sceneName)
    {
        float startTime = Time.unscaledTime;

        statusText.text = "Initializing spaceship...";

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        if (operation == null)
        {
            statusText.text = "Unable to load level!";
            yield break;
        }

        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float actualProgress =
                Mathf.Clamp01(operation.progress / 0.9f);

            displayedProgress = Mathf.MoveTowards(
                displayedProgress,
                actualProgress,
                fillSpeed * Time.unscaledDeltaTime
            );

            UpdateUI(displayedProgress);

            statusText.text = "Loading galaxy...";
            yield return null;
        }

        // Wait until minimum display time has passed.
        while (Time.unscaledTime - startTime <
               minimumDisplayTime)
        {
            displayedProgress = Mathf.MoveTowards(
                displayedProgress,
                0.9f,
                fillSpeed * Time.unscaledDeltaTime
            );

            UpdateUI(displayedProgress);
            yield return null;
        }

        // Smoothly complete the progress bar.
        while (displayedProgress < 1f)
        {
            displayedProgress = Mathf.MoveTowards(
                displayedProgress,
                1f,
                fillSpeed * Time.unscaledDeltaTime
            );

            UpdateUI(displayedProgress);
            yield return null;
        }

        statusText.text = "Ready for launch!";

        yield return new WaitForSecondsRealtime(0.3f);

        operation.allowSceneActivation = true;
    }

    private void UpdateUI(float progress)
    {
        progressBar.value = progress;

        percentageText.text =
            Mathf.RoundToInt(progress * 100f) + "%";
    }
}
