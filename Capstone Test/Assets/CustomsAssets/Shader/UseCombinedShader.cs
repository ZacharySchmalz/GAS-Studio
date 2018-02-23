using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCombinedShader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		GetComponent<Camera> ().SetReplacementShader (Shader.Find("Custom/CombinedShader"), "RenderType");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
