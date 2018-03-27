using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour {

	float acceleration = 0.0f;

	public Text speedText;

	void Start () {
		
	}
	
	void Update () {
        
//		if (Input.GetKey ("up")) {
//			acceleration += 2.0f;
//		}
//
//		acceleration -= 1.0f;
//
//		if (acceleration < 0) {
//			acceleration = 0.0f;
//		}
//
//		if (acceleration > 250) {
//			acceleration = 250.0f;
//		}
//
//		this.transform.localEulerAngles = new Vector3 (0.0f, 0.0f, 90 - acceleration);

		//speedText.text = acceleration.ToString ("f1") + " kmph";
	}		
}
