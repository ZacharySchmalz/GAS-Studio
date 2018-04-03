using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetworkUseCombinedShader : MonoBehaviour 
{
    public NetworkIdentity identity;
	// Use this for initialization
	void Start () {
        if (!identity.isLocalPlayer)
            return;
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		GetComponent<Camera> ().SetReplacementShader (Shader.Find("Custom/CombinedShader"), "RenderType");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
