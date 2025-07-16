using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioData")]
[System.Serializable]
public class AudioData : ScriptableObject
{

    [System.Serializable]
    public struct AudioItem
    {
        public string audioName;
        public AudioClip audioClip;
    }

    public List<AudioItem> audioItems;
}
