using UnityEngine;
using System.Collections;

public class GraphBar : MonoBehaviour {
	public string AxisName;
	private Vector3 position;
	// Use this for initialization
	void Start () {
		position = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		float newYScale = (Input.GetAxis (AxisName) + 1f)/5f * .2f;
		this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, newYScale, this.gameObject.transform.localScale.z);
		this.gameObject.transform.localPosition = new Vector3(position.x, position.y + newYScale/2, position.z);
	}
}
