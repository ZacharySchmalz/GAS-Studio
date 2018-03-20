using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plays sound from random intervals
public class RandomSoundPlayer : SoundPlayer 
{
    public float interval;

    public string[] soundList;

    private float currentTime;

    void Start()
    {
        currentTime = 0;        
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= interval && soundList.Length > 0)
        {
            int index = Random.Range(0, soundList.Length);
            Debug.Log("Playing " + soundList[index]);
            Play(soundList[index]);
            currentTime = 0;
        }
    }

}
