using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "AudioClipGroup")]
public class AudioClipGroup : ScriptableObject
{
    [Range(0f, 2f)]
    public float VolumeMin = 1.05f;
    [Range(0f, 2f)]
    public float VolumeMax = 0.95f;
    [Range(0f, 2f)]
    public float PitchMin = 1f;
    [Range(0f, 2f)]
    public float PitchMax = 1f;
    [Range(0f, 2f)]
    public float Cooldown = 0.1f;
    public bool Loop = false;
    public List<AudioClip> Clips;

    private float timestamp;

    public AudioMixerGroup AudioMixerGroup;
    private void OnEnable()
    {
        timestamp = 0;
    }

    public void Play()
    {
        Play(AudioSourcePool.Instance.GetSource());
    }

    public void Play(AudioSource source)
    {
        if (timestamp > Time.time) return;
        timestamp = Time.time + Cooldown;

        source.outputAudioMixerGroup = AudioMixerGroup;

        float baseVolume = 1f;
        if (AudioSourcePool.Instance != null)
            baseVolume = AudioSourcePool.Instance.currentSfxVolume;

        source.volume = baseVolume * Random.Range(VolumeMin, VolumeMax);
        source.pitch = Random.Range(PitchMin, PitchMax);
        source.clip = Clips[Random.Range(0, Clips.Count)];
        source.loop = Loop;
        source.Play();
    }
    
}
