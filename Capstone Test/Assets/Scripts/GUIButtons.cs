using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToMainMenu () {
		SceneManager.LoadScene ("MainMenu");
	}

	public void ToLevelSelect () {
		SceneManager.LoadScene ("LevelSelect");
	}

	public void LoadLvlOne () {
		SceneManager.LoadScene ("doverpointe");
	}

	public void LoadLvlTwo () {
		SceneManager.LoadScene ("LevelTwo");
	}

	public void LoadLvlThree () {
		SceneManager.LoadScene ("LevelThree");
	}

	public void ToInstructions () {
		SceneManager.LoadScene ("Instructions");
	}

	public void ToDataOptions () {
		SceneManager.LoadScene ("DataOptions");
	}

}
