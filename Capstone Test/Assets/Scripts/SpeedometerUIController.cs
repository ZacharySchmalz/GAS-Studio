using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedometerUIController : MonoBehaviour {

    public GameObject ticker;
    public Text speedText; 
    public GameObject carController;

	void Start () 
    {
		
	}
	
	void Update () 
    {
        float currentSpeed = carController.GetComponent<CarControlCS>().CurrentSpeedLog;
        if (Input.GetAxis("Accel") > 0)
        {
            ticker.transform.localEulerAngles = new Vector3 (0.0f, 0.0f, 90 - currentSpeed);
        }
        else
        {
            ticker.transform.localEulerAngles = new Vector3 (0.0f, 0.0f, 90 - currentSpeed);
        }

        speedText.text = currentSpeed.ToString ("f1") + " kmph"; 

	}
}
