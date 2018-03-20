using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Adapted from Brackeys tutorial on "Introduction to AUDIO in Unity"
public class SoundManager : MonoBehaviour 
{
    public Sound[] sounds;

    public static SoundManager instance;

    void Awake()
    {
        
        // keep the audio manager persistent
        DontDestroyOnLoad(gameObject);
        
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // setup the audio sources to be played
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        
    }
        
    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

}

// Adapted from Brackeys tutorial on "Introduction to AUDIO in Unity"
[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public string name;
    public bool loop;
    [Range(0, 1)]
    public float volume;
    [Range(0, 2)]
    public float pitch = 0.5f;

    [HideInInspector]
    public AudioSource source;
}
