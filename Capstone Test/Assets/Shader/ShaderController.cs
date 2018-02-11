using UnityEngine;
using System.Collections;

public class ShaderController : MonoBehaviour {

	public string[] tagList;
	public Color[] colorList;
	private GameObject[] shaderList;
	private Shader standardShader;
	private Shader segmentShader;
	public bool isSegShader;


	void Start () {
		standardShader = Shader.Find("Standard");
		segmentShader = Shader.Find ("Custom/SegmentShader");
	}
	
	// Update is called once per frame
	void Update () {
		

		//Normal Shader
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			if (!isSegShader) {
				shaderSwap ();
			}
			if(GetComponent<DepthMap> ().enabled == true) 
				GetComponent<DepthMap> ().enabled = false;
		}
		//Segment Shader
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			if (isSegShader) {
				shaderSwap ();
			}
			if(GetComponent<DepthMap> ().enabled == true) 
				GetComponent<DepthMap> ().enabled = false;
		}
		//DepthShader
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			if (!isSegShader) {
				shaderSwap ();
			}
			if(GetComponent<DepthMap> ().enabled == false) 
				GetComponent<DepthMap> ().enabled = true;
		}
	}

	void shaderSwap() {
		if (tagList.Length > 0 && colorList.Length > 0) {
			if (isSegShader) {
				for (int i = 0; i < tagList.Length; i++) {
					shaderList = GameObject.FindGameObjectsWithTag (tagList[i]);
					foreach (GameObject gameObject in shaderList) {
						if (gameObject.GetComponent<Renderer> () != null) {
							gameObject.GetComponent<Renderer> ().material.shader = segmentShader;
							gameObject.GetComponent<Renderer> ().material.SetColor ("_SegColor", (Vector4)colorList [i]);
						}
					}
				}
				isSegShader = false;
			} else {
				for (int j = 0; j < tagList.Length; j++) {
					shaderList = GameObject.FindGameObjectsWithTag (tagList[j]);
					foreach (GameObject gameObject in shaderList) {
						if (gameObject.GetComponent<Renderer> () != null) {
							gameObject.GetComponent<Renderer> ().material.shader = standardShader;
						}
					}
				}
				isSegShader = true;
			}
		}
	}

}
