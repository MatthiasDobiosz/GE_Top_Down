using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

/**
    Manages Audio within the Game
    https://www.youtube.com/watch?v=6OT43pvUyfY
*/
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else 
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("Background Music");
    }

    public void PlayDelayed(string name, float delay)
    {
        StartCoroutine(PlayDelayedCoroutine(name, delay));
    }

    private IEnumerator PlayDelayedCoroutine(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        Play(name);
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void Cancel(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public void ChangePitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.pitch = pitch;
    }

    public void ChangeVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.volume = volume;
    }
}
