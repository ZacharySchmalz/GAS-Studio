using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureReader : MonoBehaviour 
{

    public delegate void Save();
    public static event Save OnSaveTexture;

    public Camera[] cameras;
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

        for (int i = 0; i < cameras.Length; i++)
        {
            RenderTexture renderTexture = cameras[i].targetTexture;
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
                string cameraLoc = "";
                if (i == 0)
                    cameraLoc = "left";
                else if (i == 1)
                    cameraLoc = "center";
                else if (i == 2)
                    cameraLoc = "right";
                path = string.Format("{0}{1}{2}.png", imgFolderPath, cameraLoc, renderNum);
                File.WriteAllBytes(path, texBytes);
            }
            renderPath = path;
            LogManager.instance.Log(renderPath + ",");
        }

        renderNum++;
        Debug.Log("Saving Texture");

        // event to call when the render texture is saved
        if (OnSaveTexture != null)
        {
            OnSaveTexture();
        }
    }
}
