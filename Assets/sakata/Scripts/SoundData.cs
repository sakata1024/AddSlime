using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName ="ScriptableObject/SoundData")]
public class SoundData : ScriptableObject
{
    public List<AudioClipData> soundList = new List<AudioClipData>();

    public AudioClipData Find(string name)
    {
        return soundList.FirstOrDefault(x => x.name == name);
    }
}

[System.Serializable]
public class AudioClipData
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
}
