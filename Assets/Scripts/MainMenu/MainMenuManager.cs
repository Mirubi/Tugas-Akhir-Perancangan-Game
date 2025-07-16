using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    AudioData audioData;

    void Awake()
    {
        audioData = Resources.Load<AudioData>("AudioData");
    }

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioClip clip = audioData.audioItems.FirstOrDefault(_clip => _clip.audioName == "MainMenu").audioClip;
            AudioManager.Instance.PlayBGM(clip);
        }

        playButton.onClick.AddListener(PlayGame);
        exitButton.onClick.AddListener(Quit);
    }
    void PlayGame()
    {
        ButtonBehavior.Instance.LoadAsyncScene("Level");
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
