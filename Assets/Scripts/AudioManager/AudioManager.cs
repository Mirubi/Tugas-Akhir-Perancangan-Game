using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM Settings")]
    public AudioSource bgmSource;
    [SerializeField] private AudioSettingData audioSettingData;
    private float bgmVolume = 0.5f;
    [SerializeField] private float fadeDuration = 1.5f;
    private Coroutine fadeCoroutine;
    bool isMuteBackgroundMusic;

    [Header("SFX Settings")]
    [SerializeField] private int initialSfxSources = 3;
    private float sfxVolume = 0.7f;
    private List<AudioSource> sfxSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }

        bgmVolume = audioSettingData.bgmVolume;
        sfxVolume = audioSettingData.sfxVolume;
    }

    private void InitializeAudioSources()
    {
        // BGM Source Setup
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.volume = 0f; // Start with volume 0 for fade in

        // SFX Sources Setup
        for (int i = 0; i < initialSfxSources; i++)
        {
            CreateNewSfxSource();
        }
    }

    private AudioSource CreateNewSfxSource()
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.volume = sfxVolume;
        sfxSources.Add(newSource);
        return newSource;
    }

    #region BGM Controls
    public void PlayBGM(AudioClip bgmClip, bool withFade = true)
    {
        if (bgmSource.clip == bgmClip && bgmSource.isPlaying) return;

        // Stop any ongoing fade
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        bgmSource.clip = bgmClip;
        bgmSource.Play();

        if (withFade)
        {
            fadeCoroutine = StartCoroutine(FadeInBGM());
        }
        else
        {
            bgmSource.volume = bgmVolume;
        }
    }

    private IEnumerator FadeInBGM()
    {
        float currentTime = 0;
        float startVolume = 0;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, bgmVolume, currentTime / fadeDuration);
            yield return null;
        }
    }

    public void StopBGM(bool withFade = true)
    {
        if (withFade)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeOutBGM());
        }
        else
        {
            bgmSource.Stop();
        }
    }

    public void ToggleBackgroundMusic()
    {
        isMuteBackgroundMusic = !isMuteBackgroundMusic;
        if (isMuteBackgroundMusic)
        {
            bgmSource.Stop();
        }
        else
        {
            bgmSource.Play();
        }
    }

    private IEnumerator FadeOutBGM()
    {
        float currentTime = 0;
        float startVolume = bgmSource.volume;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeDuration);
            yield return null;
        }

        bgmSource.Stop();
    }

    public void SetBGMVolume(float newVolume)
    {
        bgmVolume = Mathf.Clamp01(newVolume);
        bgmSource.volume = bgmVolume;
    }
    #endregion

    #region SFX Controls
    public void PlaySFX(AudioClip sfxClip)
    {
        AudioSource availableSource = GetAvailableSfxSource();
        availableSource.PlayOneShot(sfxClip);
    }

    public void StopAllSFX()
    {
        foreach (AudioSource source in sfxSources)
        {
            source.Stop();
        }
    }

    public void SetSFXVolume(float newVolume)
    {
        sfxVolume = Mathf.Clamp01(newVolume);
        foreach (AudioSource source in sfxSources)
        {
            source.volume = sfxVolume;
        }
    }

    private AudioSource GetAvailableSfxSource()
    {
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying) return source;
        }
        return CreateNewSfxSource();
    }
    #endregion
}