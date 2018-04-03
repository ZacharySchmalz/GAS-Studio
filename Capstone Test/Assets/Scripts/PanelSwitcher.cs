using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelSwitcher : MonoBehaviour {

	public GameObject mainPanel, deliveryPanel, racingPanel, instructionsPanel;

	public void ToMainMenu () {
		if (mainPanel.activeSelf == false) {
			mainPanel.SetActive (true);
		}
		if (deliveryPanel.activeSelf == true) {
			deliveryPanel.SetActive (false);
		}
		if (racingPanel.activeSelf == true) {
			racingPanel.SetActive (false);
		}
		if (instructionsPanel.activeSelf == true) {
			instructionsPanel.SetActive (false);
		}
	}

	public void ToDeliveryLevels () {
		if (deliveryPanel.activeSelf == false) {
			deliveryPanel.SetActive (true);
		}
		if (mainPanel.activeSelf == true) {
			mainPanel.SetActive (false);
		}
		if (racingPanel.activeSelf == true) {
			racingPanel.SetActive (false);
		}
		if (instructionsPanel.activeSelf == true) {
			instructionsPanel.SetActive (false);
		}
	}

	public void LoadLvlOne () {
		SceneManager.LoadScene ("doverpointe");
	}

	public void LoadLvlTwo () {
		SceneManager.LoadScene ("NightMap");
	}

	public void LoadTrackOne () {
		SceneManager.LoadScene ("RacingLoop");
	}

	public void LoadTrackTwo () {
		SceneManager.LoadScene ("Racing2");
	}

	public void ToInstructions () {
		if (instructionsPanel.activeSelf == false) {
			instructionsPanel.SetActive (true);
		}
		if (deliveryPanel.activeSelf == true) {
			deliveryPanel.SetActive (false);
		}
		if (racingPanel.activeSelf == true) {
			racingPanel.SetActive (false);
		}
		if (mainPanel.activeSelf == true) {
			mainPanel.SetActive (false);
		}
	}

	public void ToRacingLevels () {
		if (racingPanel.activeSelf == false) {
			racingPanel.SetActive (true);
		}
		if (deliveryPanel.activeSelf == true) {
			deliveryPanel.SetActive (false);
		}
		if (mainPanel.activeSelf == true) {
			mainPanel.SetActive (false);
		}
		if (instructionsPanel.activeSelf == true) {
			instructionsPanel.SetActive (false);
		}
	}
}
