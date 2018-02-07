using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FileType
{
    Text,
    None
};

// The File Manager can edit one file at a time
public class FileManager : MonoBehaviour
{
    public FileType type;
    public string currentFileName;
    public bool isLog;

    private string defaultFilePath;
    private string currentFilePath;
    private string streamingAssetsPath;
    private string topLevelPath;

    // Inside Assets Folder; 
    public string DefaultFilePath
    {
        get { return defaultFilePath; }
    }

    public string CurrentFilePath
    {
        get { return currentFilePath; }
    }

    // Inside Assets > StreamingAssets Folder
    public string StreamingAssetsPath
    {
        get { return streamingAssetsPath; }
    }

    // Where the Assets Folder is located
    public string TopLevelPath
    {
        get { return topLevelPath; }
    }

    void Start()
    {

        defaultFilePath = Application.dataPath;
        topLevelPath = Directory.GetParent(defaultFilePath).FullName;
        Debug.Log("Default File Path: " + defaultFilePath);
        currentFilePath = defaultFilePath + "/StreamingAssets";
        streamingAssetsPath = defaultFilePath + "/StreamingAssets";

        if (isLog)
        {
            currentFilePath = Path.Combine(currentFilePath, currentFileName + GetTypeExtensionString(type));
            using (StreamWriter sw = File.CreateText(currentFilePath))
            {
                sw.WriteLine(DateTime.Now.ToString());
            }
        }

    }

    public void UpdateFilePath(string name)
    {
        currentFilePath = Path.Combine(defaultFilePath, name);
    }

    public void WriteLineToFile(string message, bool overwrite = false)
    {
        if (string.IsNullOrEmpty(currentFileName))
        {
            currentFileName = "data_" + DateTime.Today.Month + "_" + DateTime.Today.Year;
            UpdateFilePath(currentFileName);
        }
    }

    public void Log(object obj)
    {
        if (File.Exists(currentFilePath))
        {
            using (StreamWriter sw = File.AppendText(currentFilePath))
            {
                Debug.Log("Logging to file");
                sw.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString("H:mm:ss"), obj.ToString()));
            }
        }
        else
        {
            Debug.Log("File does not exist");
        }
    }

    public bool CheckForFile(string path, string fileName)
    {
        string tempPath = Path.Combine(path, fileName);
        if (File.Exists(tempPath))
        {
            return true;
        }

        return false;
    }

    public string ReadFile(string name)
    {
        string output = "";

        string temppath = Path.Combine(defaultFilePath + "/StreamingAssets", name);
        using (StreamReader sr = new StreamReader(temppath))
        {
            // read until the end of the file
            while (sr.Peek() >= 0)
            {
                output += sr.ReadLine();
            }

            return output;
        }
    }

    public DirectoryInfo CreateDirectory(string path, string directoryName)
    {
        string tempPath = Path.Combine(path, directoryName);

        if (Directory.Exists(tempPath))
        {
            Debug.Log(string.Format("{0} already exists!", tempPath));
        }

        return Directory.CreateDirectory(tempPath);
    }

    public void DeleteFile(string filepath)
    {
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
    }

    private string GetTypeExtensionString(FileType type)
    {
        switch (type)
        {
            case FileType.Text:
                return ".txt";
            default:
                return "";
        }
    }

}
