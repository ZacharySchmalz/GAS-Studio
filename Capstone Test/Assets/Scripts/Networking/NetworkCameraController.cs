using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkCameraController : NetworkBehaviour 
{

    public GameObject camera;
	// Use this for initialization
	void Start () 
    {
        if (isLocalPlayer)
            camera = GameObject.FindGameObjectWithTag("MainCamera");

        if (camera != null)
        {
            camera.transform.parent = this.transform;
            camera.transform.localPosition = Vector3.zero;
            camera.transform.localRotation = Quaternion.identity;
            camera.transform.localPosition += new Vector3(0, 10f, -15f);
            camera.transform.Rotate(new Vector3(30, 0, 0));
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
