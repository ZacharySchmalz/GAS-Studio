using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

public enum LoggerType
{
    Generic,
    Transform,
    None
}
public class Logger : MonoBehaviour {

    public bool logGameObjectName = true;
    public LoggerType type;

    protected string gameObjectName = "";

	// Use this for initialization
	public virtual void Start () {
        if (logGameObjectName)
            gameObjectName = gameObject.name;
	}
	
	public virtual void Update () 
    {
        LogValues();
	}

    public virtual void LogValues()
    {

    }
}
