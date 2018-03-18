using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Plays custom sounds to test the SoundManager
public class SoundPlayer : MonoBehaviour 
{
    
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.instance.Play("tap");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SoundManager.instance.Play("open");
        }
            
	}

}
