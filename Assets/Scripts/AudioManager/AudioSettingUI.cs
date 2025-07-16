using UnityEngine;
using UnityEngine.UI;

public class AudioSettingUI : MonoBehaviour
{

    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    private AudioSettingData audioSettingData;

    [Header("On Off Toggle")]
    [SerializeField] private Button toggleBgm;

    [SerializeField] private GameObject handlerButton;
    [SerializeField] private Vector3 offButtonPosition;
    bool isBGMMuted = false;

    private Vector3 onButtonPosition;

    void Start()
    {
        audioSettingData = (AudioSettingData)Resources.Load("AudioSettingData");
        bgmSlider.value = audioSettingData.bgmVolume;
        sfxSlider.value = audioSettingData.sfxVolume;
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);

        isBGMMuted = audioSettingData.isBgmMuted;

        onButtonPosition = -offButtonPosition;
        toggleBgm.onClick.AddListener(ToggleBgm);

        isBGMMuted = audioSettingData.isBgmMuted;
        UpdateToggleUI();
        SetBgmVolume(audioSettingData.bgmVolume);
        SetSfxVolume(audioSettingData.sfxVolume);
    }

    void ToggleBgm()
    {
        isBGMMuted = !isBGMMuted;
        audioSettingData.isBgmMuted = isBGMMuted;
        if (isBGMMuted)
        {
            AudioManager.Instance.SetBGMVolume(0f);
        }
        else
        {
            AudioManager.Instance.SetBGMVolume(audioSettingData.bgmVolume);
        }
        UpdateToggleUI();

    }

    void UpdateToggleUI()
    {
        if (isBGMMuted)
        {
            handlerButton.GetComponent<RectTransform>().anchoredPosition = offButtonPosition;
        }
        else
        {
            handlerButton.GetComponent<RectTransform>().anchoredPosition = onButtonPosition;
        }
    }

    void SetBgmVolume(float arg0)
    {
        audioSettingData.bgmVolume = bgmSlider.value;
        if (m_AudioSource != null)
        {
            m_AudioSource.volume = audioSettingData.bgmVolume;
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(audioSettingData.bgmVolume);
        }
    }

    void SetSfxVolume(float arg0)
    {
        audioSettingData.sfxVolume = sfxSlider.value;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(audioSettingData.sfxVolume);
        }
    }
}
