using UnityEngine;
using DG.Tweening;

public class SmoothPopup : MonoBehaviour
{
    [Header("Animation Settings")]
    public float showDuration = 0.6f;
    public float hideDuration = 0.2f;
    public float popScale = 1.05f; // Sedikit scaling untuk efek "pop"

    private Vector3 originalScale;
    private RectTransform rectTransform;

    void Awake()
    {
        // Get the appropriate transform component
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform != null ? rectTransform.localScale : transform.localScale;

        // Set initial scale to 0
        if (rectTransform != null)
            rectTransform.localScale = Vector3.zero;
        else
            transform.localScale = Vector3.zero;
    }

    void OnEnable()
    {
        PlayShowAnimation();
    }

    public void PlayShowAnimation()
    {
        Transform target = rectTransform != null ? rectTransform : transform;

        // Sequence untuk animasi muncul
        Sequence showSequence = DOTween.Sequence();
        showSequence.Append(target.DOScale(originalScale * popScale, showDuration * 0.7f).SetEase(Ease.OutQuad));
        showSequence.Append(target.DOScale(originalScale, showDuration * 0.3f).SetEase(Ease.InQuad));
        showSequence.Play();
    }

    public void ClosePopup()
    {
        Transform target = rectTransform != null ? rectTransform : transform;

        // Animasi menutup
        target.DOScale(Vector3.zero, hideDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() => gameObject.SetActive(false));
    }

    // Optional: Reset scale ketika dinonaktifkan
    void OnDisable()
    {
        Transform target = rectTransform != null ? rectTransform : transform;
        target.localScale = Vector3.zero;
    }
}