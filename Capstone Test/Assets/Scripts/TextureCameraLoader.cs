using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TextureCameraLoader : MonoBehaviour {

    public Camera[] cameras;
    public NetworkIdentity identity;

	// Use this for initialization
	void Start ()
    {
        if (!identity)
            return;

        if (!identity.isLocalPlayer)
            return;

        LogManager.instance.textureReader.cameras = cameras;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
