using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// function used to unload a scene after an animation event
public class UnloadSceneAsyncUnit : MonoBehaviour 
{

    public void Unload()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
