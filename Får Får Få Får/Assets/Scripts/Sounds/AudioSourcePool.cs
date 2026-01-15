using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public static AudioSourcePool Instance;

    public AudioSource AudioSourcePrefab;
    private List<AudioSource> audioSources;
    [Range(0f, 1f)]
    public float currentSfxVolume = 1f;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSources = new List<AudioSource>();
    }

    public AudioSource GetSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying) return source;
        }
        AudioSource newSource = GameObject.Instantiate(AudioSourcePrefab, transform);
        newSource.volume = currentSfxVolume;
        audioSources.Add(newSource);
        return newSource;
    }
    public void SetSfxVolume(float volume)
    {
        currentSfxVolume = volume;

        foreach (AudioSource src in audioSources)
        {
            if (src != null)
                src.volume = currentSfxVolume;
        }
    }

}
