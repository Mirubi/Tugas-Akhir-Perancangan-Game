using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomFadeManager : MonoBehaviour
{
    public static RoomFadeManager instance;
    private CanvasGroup canvasGroup;

    // ✅ Tambahkan variabel ini
    public float defaultFadeDuration = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            canvasGroup = GetComponent<CanvasGroup>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (canvasGroup != null && canvasGroup.alpha > 0f)
            StartCoroutine(FadeIn(defaultFadeDuration));
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
        canvasGroup.alpha = 1f;
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
        canvasGroup.alpha = 0f;
    }
}
