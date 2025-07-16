using UnityEngine;


[CreateAssetMenu(fileName = "AudioSettingData", menuName = "AudioSettingData", order = 0)]
[System.Serializable]
public class AudioSettingData : ScriptableObject
{
    [Range(0f, 1f)]
    public float bgmVolume;
    public bool isBgmMuted;
    [Range(0f, 1f)]
    public float sfxVolume;
    public bool isSfxMuted;
}

