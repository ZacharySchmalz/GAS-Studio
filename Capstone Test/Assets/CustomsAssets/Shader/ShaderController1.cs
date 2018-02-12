﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public struct segmentTags
{
	public string tag;
	public Color color;
}

public class ShaderController1 : MonoBehaviour {

	public float depthDistance = 100;
	public segmentTags[] segmentList;
	private GameObject[] shaderList;
	private Shader standardShader;
	private Shader segmentShader;
	private float originalFarPlane;
	private bool isSegShader;
	private int shaderState = 0;


	void Start () {
		standardShader = Shader.Find("Custom/SegmentShader");
		segmentShader = Shader.Find ("Unlit/SegShader");
		originalFarPlane = GetComponent<Camera> ().farClipPlane;

		for (int i = 0; i < segmentList.Length; i++) {
			shaderList = GameObject.FindGameObjectsWithTag (segmentList[i].tag);
			foreach (GameObject gameObject in shaderList) {
				if (gameObject.GetComponent<Renderer> () != null) {
					gameObject.GetComponent<Renderer> ().material.shader = standardShader;
					gameObject.GetComponent<Renderer> ().material.SetColor ("_SegColor", (Vector4)segmentList[i].color);
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {

		//Normal Shader
		if (Input.GetButtonDown("Normal")) {
			GetComponent<Camera> ().ResetReplacementShader();
			shaderState = 0;
			GetComponent<Camera> ().farClipPlane = originalFarPlane;
			if(GetComponent<DepthMap> ().enabled == true) 
				GetComponent<DepthMap> ().enabled = false;
		}
		//Segment Shader
		if (Input.GetButtonDown("Segment")) {
			GetComponent<Camera> ().SetReplacementShader (segmentShader, "RenderType");
			shaderState = 1;
			GetComponent<Camera> ().farClipPlane = originalFarPlane;
			if(GetComponent<DepthMap> ().enabled == true) 
				GetComponent<DepthMap> ().enabled = false;
		}
		//DepthShader
		if (Input.GetButtonDown("Depth")) {
			GetComponent<Camera> ().ResetReplacementShader();
			shaderState = 2;
			GetComponent<Camera> ().farClipPlane = depthDistance;
			if(GetComponent<DepthMap> ().enabled == false) 
				GetComponent<DepthMap> ().enabled = true;
		}

		if (shaderState == 2) {
			GetComponent<Camera> ().farClipPlane = depthDistance;
		}
	}
}