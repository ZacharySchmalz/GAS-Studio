using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FileType
{
    Text,
    CSV,
    None
};
public enum Destination
{
    Default,
    Custom
};

// The File Manager can edit one file at a time
// Use more instances of file manager to work with more than one different type of file 
public class FileManager : MonoBehaviour
{
    [Header("Destination Info")]
    public Destination destinationType;
    [Tooltip("The directory that the files will go into. Default as /Assets/StreamingAssets, doesn't work right now lmao")]
    public string DirectoryName;
    [Tooltip("Overwrites the current file name if the same name is found. Otherwise append n at the end of current file name where n = [0, maxFileCount]")]
    public bool overwrite;

    [Header("File Info")]
    public FileType type;
    public string currentFileName;

    [Header("Debug")]
    public int maxFileCount;    // if set to 0, default to 500
    public bool isLog;
    public bool writeTime;

    private int dupNum;
    private string defaultFilePath;
    private string currentFilePath;
    private string streamingAssetsPath;
    private string topLevelPath;

    // /Assets/
    public string DefaultFilePath
    {
        get { return defaultFilePath; }
    }
        
    // /Assets/StreamingAssets
    public string StreamingAssetsPath
    {
        get { return streamingAssetsPath; }
    }

    // Where the Assets Folder is located, not inside
    public string TopLevelPath
    {
        get { return topLevelPath; }
    }

    public string CurrentFilePath
    {
        get { return currentFilePath; }
    }

    void Start()
    {
        if (maxFileCount == 0)
            maxFileCount = 500;

        //defaultFilePath = Application.dataPath;
        defaultFilePath = "Game_Data/";
        if (!Application.isEditor)
            defaultFilePath = Application.dataPath;

        topLevelPath = Directory.GetParent(defaultFilePath).FullName;
        Directory.CreateDirectory(topLevelPath + "/Game_Data");

        if (destinationType == Destination.Default)
            currentFilePath = defaultFilePath + "/StreamingAssets";
        else if (!Application.isEditor)
            currentFilePath = topLevelPath + "/Game_Data/";
        else
            currentFilePath = topLevelPath;

        Debug.Log("Current File Path: " + currentFilePath);
        streamingAssetsPath = defaultFilePath + "/StreamingAssets";

        if (isLog)
        {
            string tempFilePath = Path.Combine(currentFilePath, currentFileName + GetTypeExtensionString(type));
            if (!overwrite)
            {
                while (File.Exists(tempFilePath))
                {
                    tempFilePath = Path.Combine(currentFilePath, currentFileName + dupNum + GetTypeExtensionString(type));
                    dupNum++;
                }
            }
            currentFilePath = tempFilePath;

            using (StreamWriter sw = File.CreateText(currentFilePath))
            {
                if (writeTime) sw.WriteLine(DateTime.Now.ToString());
            }

            Debug.Log("Writing to " + currentFilePath);
            LogLine("left" + "," + "center" + "," + "right" + "," + "throttle" + "," + "reverse" + "," + "steering" + "," + "speed");
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
                // uses a ternary operator to check if adding time for every log
                sw.Write(string.Format("{0}{1}", (writeTime) ? (DateTime.Now.ToString("H:mm:ss") + " ") : "", obj.ToString()));
            }
        }
        else
        {
            Debug.Log("File does not exist");
        }
    }

    public void LogLine(object obj)
    {
        if (File.Exists(currentFilePath))
        {
            using (StreamWriter sw = File.AppendText(currentFilePath))
            {
                // uses a ternary operator to check if adding time for every log
                sw.WriteLine(string.Format("{0}{1}", (writeTime) ? (DateTime.Now.ToString("H:mm:ss") + " ") : "", obj.ToString()));
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
            case FileType.CSV:
                return ".csv";
            default:
                return "";
        }
    }

}
