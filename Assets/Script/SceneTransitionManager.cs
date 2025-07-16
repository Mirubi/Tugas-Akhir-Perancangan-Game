using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Cinemachine;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    private string nextSpawnPoint = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithSpawn(string sceneName, string spawnPointName)
    {
        nextSpawnPoint = spawnPointName;
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // 1. Fade Out
        if (RoomFadeManager.instance != null)
            yield return RoomFadeManager.instance.FadeOut(RoomFadeManager.instance.defaultFadeDuration);

        // 2. Pindah Scene
        SceneManager.LoadScene(sceneName);
        // -> setelah scene load, akan lanjut ke OnSceneLoaded()
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(nextSpawnPoint)) return;

        // 3. Temukan spawn point & player
        GameObject spawn = GameObject.Find(nextSpawnPoint);
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (spawn != null && player != null)
        {
            // 4. Pindahkan posisi player
            player.transform.position = spawn.transform.position;

            // 5. Update posisi kamera (pakai Cinemachine)
            CinemachineVirtualCamera cam = GameObject.Find("Player Camera")?.GetComponent<CinemachineVirtualCamera>();
            if (cam != null)
            {
                cam.Follow = player.transform;
                cam.OnTargetObjectWarped(player.transform, player.transform.position);
                cam.ForceCameraPosition(player.transform.position, Quaternion.identity);
            }

            Debug.Log("Scene Loaded");
            Debug.Log("Player position after teleport: " + player.transform.position);
        }

        // 6. Fade In setelah pindah scene
        if (RoomFadeManager.instance != null)
            RoomFadeManager.instance.StartCoroutine(RoomFadeManager.instance.FadeIn(RoomFadeManager.instance.defaultFadeDuration));

        nextSpawnPoint = ""; // reset
    }
}
