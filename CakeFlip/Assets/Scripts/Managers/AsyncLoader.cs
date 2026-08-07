using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private string currentLevelToLoad;
    [SerializeField] private GameObject loadingScreenPrefab;
    [SerializeField] private LoadingScreen loadingScreen;

    public LoadingScreen LoadingScreen
    {
        get
        {
            if (loadingScreen == null)
            {
                loadingScreen = Instantiate(loadingScreenPrefab, GameManager.Instance.Canvas.transform).GetComponent<LoadingScreen>();
            }
            return loadingScreen;
        }

        set => loadingScreen = value;
    }

    public void LoadLevelAsync(string levelName)
    {
        currentLevelToLoad = levelName;
        StartCoroutine(nameof(LoadAsync));
    }

    /// <summary>
    /// Load level and update progress bar. TODO maybe some minimum value so we fake a loading screen? nah
    /// </summary>
    /// <returns></returns>

    private IEnumerator LoadAsync()
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(currentLevelToLoad);
        LoadingScreen.gameObject.SetActive(true);

        while (!loadingOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress / .9f);
            LoadingScreen.Slider.value = progress;
            LoadingScreen.ProgressText.text = $"{progress:P2}";
            yield return null;
        }

        LoadingScreen.gameObject.SetActive(false);
    }
}
