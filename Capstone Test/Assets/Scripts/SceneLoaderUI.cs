using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderUI : MonoBehaviour {

    public Animator animatedImage;
    public Canvas levelCanvas;
	// Use this for initialization
	void Start () 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;	
	}
	
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded Scene");
        StartCoroutine(UnloadSceneWait());
    }

    IEnumerator UnloadSceneWait()
    {
        yield return new WaitForSeconds(1.0f);
        levelCanvas.gameObject.SetActive(false);
        animatedImage.Play("SceneTransitionOff");
    }
}
