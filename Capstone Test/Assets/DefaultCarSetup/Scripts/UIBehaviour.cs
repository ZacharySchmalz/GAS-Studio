using UnityEngine;
using System.Collections;

public class UIBehaviour : MonoBehaviour {
	public GameObject wheel;
	public string wheelAxis;

	public GameObject gear;
	public string gearAxis;

	public GameObject brake;
	private Vector3 brakePosition;
	public string brakeAxis;

	public GameObject accel;
	private Vector3 accelPosition;
	public string accelAxis;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		float rotationAngle = Input.GetAxis (wheelAxis) * -450f;
		wheel.transform.eulerAngles = new Vector3 (15f, 0f, rotationAngle);

		float gearVal = Input.GetAxis ("GearSwitch");
		if (gearVal == 0) {
			gear.transform.localPosition = new Vector3 (.337f, 0f, 0f);
		} else if (gearVal > 0) {
			gear.transform.localPosition = new Vector3 (.337f, -0.1f, 0f);
		} else {
			gear.transform.localPosition = new Vector3 (.337f, 0.1f, 0f);
		}

		float newYScale = (Input.GetAxis(brakeAxis) + 1f)/5f * .2f;
		brake.transform.localScale = new Vector3(brake.transform.localScale.x, newYScale, brake.transform.localScale.z);
		brake.transform.localPosition = new Vector3(brakePosition.x, brakePosition.y + newYScale/2, brakePosition.z);

		float newYScale2 = (Input.GetAxis(accelAxis) + 1f)/5f * .2f;
		accel.transform.localScale = new Vector3(accel.transform.localScale.x, newYScale2, accel.transform.localScale.z);
		accel.transform.localPosition = new Vector3(accelPosition.x, accelPosition.y + newYScale2/2, accelPosition.z);
	}
}
