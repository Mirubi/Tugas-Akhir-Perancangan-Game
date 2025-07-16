using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button exitButton;
    private AudioData audioData;
    private PlayerCombat player;
    private bool isGameOver = false;

    void Awake()
    {
        audioData = Resources.Load<AudioData>("AudioData");
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        player = FindObjectOfType<PlayerCombat>();

        if (AudioManager.Instance != null)
        {
            AudioClip clip = audioData.audioItems.FirstOrDefault(_clip => _clip.audioName == "InGameMusic").audioClip;
            if (clip != null)
            {
                AudioManager.Instance.PlayBGM(clip);
            }
        }
        exitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (player != null && player.GetCurrentHealth() <= 0 && !isGameOver)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (AudioManager.Instance != null)
        {
            AudioClip clip = audioData.audioItems.FirstOrDefault(_clip => _clip.audioName == "GameOver").audioClip;
            if (clip != null)
            {
                AudioManager.Instance.PlaySFX(clip);
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        ButtonBehavior.Instance.LoadAsyncScene("MainMenu");
    }
}