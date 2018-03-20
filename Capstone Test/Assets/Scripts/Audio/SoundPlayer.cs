using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Plays custom sounds through the SoundManager
public class SoundPlayer : MonoBehaviour 
{
    public virtual void Play(string sound)
    {
        if (SoundManager.instance != null)
            SoundManager.instance.Play(sound);
        else
            Debug.Log("There is no Sound Manager in the scene!");
    }
}
