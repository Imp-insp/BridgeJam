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
    
    public void SetVolume(string name, float volume)
    {
        // Encontra o som no array pelo nome
        Sound s = Array.Find(sounds, sound => sound.name == name);
        // Se não encontrar o som, exibe um aviso no console e encerra a função
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        // Define o volume do AudioSource, garantindo que o valor esteja entre 0 e 1
        s.source.volume = Mathf.Clamp(volume, 0f, 1f);
    }
    
    public void Stop(string name)
    {
        // Encontra o som no array pelo nome
        Sound s = Array.Find(sounds, sound => sound.name == name);
        // Se não encontrar o som, exibe um aviso no console e encerra a função
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        // Para o som completamente
        s.source.Stop();
    }
    
    public bool IsPlaying(string name)
    {
        // Encontra o som no array pelo nome
        Sound s = Array.Find(sounds, sound => sound.name == name);
        // Se não encontrar o som, exibe um aviso no console e retorna false
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return false;
        }
        // Retorna se o AudioSource está tocando
        return s.source.isPlaying;
    }
}