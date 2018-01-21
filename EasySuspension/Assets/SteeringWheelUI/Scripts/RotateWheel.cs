using UnityEngine;
using System.Collections;

public class RotateWheel : MonoBehaviour {
	// Use this for initialization
	public string AxisName;

	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		float rotationAngle = Input.GetAxis (AxisName) * -450f;
		this.gameObject.transform.eulerAngles = new Vector3 (15f, 0f, rotationAngle);
	}
}
