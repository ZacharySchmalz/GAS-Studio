using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedometerUIController : MonoBehaviour {

    public GameObject ticker;
    public Text speedText;
    public Image steeringWheel;
    public Image gearSelector;
    public float gearSize;
    private Vector3 gearPosition;

    public GameObject carController;
    private CarControlCS controlScript;


	void Start () 
    {
        gearPosition = gearSelector.transform.localPosition;
        controlScript = carController.GetComponent<CarControlCS>();

    }
	
	void Update () 
    {
        float currentSpeed = controlScript.CurrentSpeedLog;
        if (Input.GetAxis("Accel") > 0)
        {
            ticker.transform.localEulerAngles = new Vector3 (0.0f, 0.0f, 90 - currentSpeed);
        }
        else
        {
            ticker.transform.localEulerAngles = new Vector3 (0.0f, 0.0f, 90 - currentSpeed);
        }

        speedText.text = currentSpeed.ToString ("f1") + " kmph";

        float currentSteeringAngle = carController.GetComponent<CarControlCS>().Wheel;
        steeringWheel.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -170 * currentSteeringAngle);

        float gearVal = controlScript.Gear;
        gearSelector.transform.localPosition = gearPosition + Vector3.up * (gearSize * gearVal);
    }
}
