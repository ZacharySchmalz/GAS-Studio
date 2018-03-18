﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureReader : MonoBehaviour 
{

    public delegate void Save();
    public static event Save OnSaveTexture;

    public RenderTexture renderTexture;
    public int maxFramesToLog;

    private string topLevelPath;
    private string imgFolderPath;
    private int renderNum = 0;
    private string renderPath;

    public string RenderPath
    {
        get { return renderPath; }
    }

    void Awake()
    {
        topLevelPath = Application.dataPath + "/../";
    }

	void Start () 
    {
        if (maxFramesToLog == 0)
            maxFramesToLog = 3600;      // 2 minutes of gameplay at 60fps
	}
	
	void Update () 
    {
        if (renderNum < maxFramesToLog)
            StartCoroutine(SaveTexture());
	}

    IEnumerator SaveTexture()
    {
        yield return new WaitForEndOfFrame();

        int width = renderTexture.width;
        int height = renderTexture.height;

        RenderTexture.active = renderTexture;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        byte[] texBytes = tex.EncodeToPNG();


        string path = "";
        if (Directory.Exists(topLevelPath + "Game_Data"))
        {
            imgFolderPath = topLevelPath + "Game_Data" + "/IMG/";
            Directory.CreateDirectory(imgFolderPath);
            path = string.Format("{0}{1}{2}.png", imgFolderPath, "render", renderNum);
            File.WriteAllBytes(path, texBytes);
        }

        Debug.Log("Saving Texture");
        renderPath = path;
        renderNum++;

        // event to call when the render texture is saved
        if (OnSaveTexture != null)
        {
            LogManager.instance.Log(renderPath + ",");
            OnSaveTexture();
        }
    }
}