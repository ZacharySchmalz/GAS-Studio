using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {
	public GameObject selector;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float gear = Input.GetAxis ("GearSwitch");
		if (gear == 0) {
			selector.transform.localPosition = new Vector3 (.337f, 0f, 0f);
		} else if (gear > 0) {
			selector.transform.localPosition = new Vector3 (.337f, -0.1f, 0f);
		} else {
			selector.transform.localPosition = new Vector3 (.337f, 0.1f, 0f);
		}
	}
}
