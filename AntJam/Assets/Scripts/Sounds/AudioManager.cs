using System;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;


    private void Awake()
    {
        #region Singleton
        if (Instance == null) { Instance = this; }
        else {Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        #endregion

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }
    public void Play(string name)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        if ( s == null )
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Play();
    }
    public void PlayOnce(string name)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        if ( s == null )
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        if (s.source.isPlaying) return;
        s.source.Play();
    }
    public void PlayOnce(string name, float volumeOffSet)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        if ( s == null )
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        
        s.source.volume = volumeOffSet;
        if (s.source.isPlaying) return;
        s.source.Play();
    }
    public void Pause(string name)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        if ( s == null )
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Pause();
    }
}
