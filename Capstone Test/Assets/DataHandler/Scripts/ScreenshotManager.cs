using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(FileManager))]
public class ScreenshotManager : MonoBehaviour {

    public int timeBetweenShots;
    public string fileName;
    public const int maxImages = 5000;
    public bool isTakingScreenshot;

    public FileManager fileManager;
    private DirectoryInfo directoryToSave;
    private string directoryName = "Game_Screenshots";

    // Use this for initialization
    void Start ()
    {
        fileManager = GetComponent<FileManager>();
        directoryToSave = fileManager.CreateDirectory(fileManager.TopLevelPath, directoryName);

        StartCoroutine(TakeScreenShot());
    }

    // Update is called once per frame
    void Update ()
    {
	}

    IEnumerator TakeScreenShot()
    {
        while (Application.isPlaying && isTakingScreenshot)
        {
            
            int count = 0;
            string tempFileName = fileName;


            while (fileManager.CheckForFile(directoryToSave.FullName, tempFileName + ".png"))
            {
                count++;
                tempFileName = fileName + string.Format("_{0}", count);
            }

            if (count < maxImages)
            {
                Debug.Log("Saving " + tempFileName);
                ScreenCapture.CaptureScreenshot(Path.Combine(directoryToSave.FullName, tempFileName + ".png"));
            }
            else
            {
                Debug.Log("You've exceeded the amount of screenshots!");
            }

            yield return new WaitForSeconds(timeBetweenShots);
        }
        
    }
}
