using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Sets the game object this script is attached to if it is not the local player
public class NetworkObjectDisabler : MonoBehaviour 
{
    public NetworkIdentity identity;
	// Use this for initialization
	void Start () {
        
        if (!identity.isLocalPlayer)
        {
            this.gameObject.SetActive(false);
            return;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
