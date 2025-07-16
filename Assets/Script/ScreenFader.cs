using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;

    private CanvasGroup canvasGroup;

    [Header("Default Fade Duration")]
    public float fadeDuration = 0.1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogError("CanvasGroup not found on FadeCanvas!");
                return;
            }

            // Set gelap saat awal, lalu langsung FadeIn
            canvasGroup.alpha = 1f;
            StartCoroutine(FadeIn(fadeDuration));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator FadeOut(float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f; // Pastikan nilai akhir solid
    }

    public IEnumerator FadeIn(float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
        canvasGroup.alpha = 0f; // Pastikan nilai akhir transparan
    }
}
