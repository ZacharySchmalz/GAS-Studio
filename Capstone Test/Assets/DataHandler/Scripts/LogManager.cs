using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FileManager))]
public class LogManager : MonoBehaviour {

    public static LogManager instance;
    public FileManager fileManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void Log(object msg)
    {
        fileManager.Log(msg);
    }
}
