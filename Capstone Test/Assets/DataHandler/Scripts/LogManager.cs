using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FileManager))]
public class LogManager : MonoBehaviour {

    public static LogManager instance;
    public FileManager fileManager;
    public TextureReader textureReader;

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

    public void LogLine(object msg)
    {
        fileManager.LogLine(msg);
    }

    public void Log(object msg)
    {
        fileManager.Log(msg);
    }
}
