using System;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Click,
    Play,
    Coin,
    Error,
    // ³ ò.ä.
}


[CreateAssetMenu(fileName = "SoundConfig", menuName = "Configs/SoundConfig")]
public class SoundConfig : ScriptableObject
{
    [Serializable]
    public class SoundEntry
    {
        public SoundType type;
        public AudioClip clip;
    }

    public List<SoundEntry> sounds = new List<SoundEntry>();

    private Dictionary<SoundType, AudioClip> soundMap;

    public AudioClip GetClip(SoundType type)
    {
        if (soundMap == null) Init();

        soundMap.TryGetValue(type, out var clip);
        return clip;
    }

    private void Init()
    {
        soundMap = new Dictionary<SoundType, AudioClip>();
        foreach (var entry in sounds)
            soundMap[entry.type] = entry.clip;
    }
}
