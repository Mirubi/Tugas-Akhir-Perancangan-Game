using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    public static ButtonBehavior Instance { get; private set; }

    private static bool s_isLoading = false;

    private GameObject loadingScreenPrefab;
    private AudioData audioData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        loadingScreenPrefab = (GameObject)Resources.Load("LoadingScreen");
        audioData = (AudioData)Resources.Load("AudioData");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        s_isLoading = false;
        SetGeneralBehavior();

        GameObject oldLoadingScreen = GameObject.FindGameObjectWithTag("LoadingScreen");
        if (oldLoadingScreen != null)
        {
            Destroy(oldLoadingScreen);
        }
    }

    void SetGeneralBehavior()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);
        foreach (Button btn in allButtons)
        {
            btn.onClick.RemoveListener(OnAnyButtonClicked);
            btn.onClick.AddListener(OnAnyButtonClicked);
        }
    }

    void OnAnyButtonClicked()
    {
        if (AudioManager.Instance != null && audioData != null)
        {
            AudioClip clip = audioData.audioItems.FirstOrDefault(a => a.audioName == "ButtonClick").audioClip;
            if (clip != null)
            {
                AudioManager.Instance.PlaySFX(clip);
            }
        }
    }

    public void LoadAsyncScene(string sceneName)
    {
        if (s_isLoading)
        {
            return;
        }
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        s_isLoading = true;

        GameObject loadingScreenInstance = null;
        if (loadingScreenPrefab != null)
        {
            loadingScreenInstance = Instantiate(loadingScreenPrefab);
            loadingScreenInstance.tag = "LoadingScreen";
            DontDestroyOnLoad(loadingScreenInstance);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (loadingScreenInstance != null)
        {
            Destroy(loadingScreenInstance);
        }
        s_isLoading = false;
    }

    public void ReloadCurrentScene()
    {
        LoadAsyncScene(SceneManager.GetActiveScene().name);
    }
}