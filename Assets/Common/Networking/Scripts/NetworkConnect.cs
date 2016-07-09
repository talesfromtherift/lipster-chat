/*
 * Copyright (c) 2015-2016 Peter Koch <peterept@gmail.com>
 * 
 * Network Connect
 * 
 * Purpose: Automatically connect to Photon 
 */
using UnityEngine;
using System.Collections;

public class NetworkConnect : MonoBehaviour 
{
	public bool AutoConnect = true;
	public string VersionNumber = "1.0";

	#region Mono
	
	void Awake() 
	{
		// Connect to photon - if we are not already
		if (AutoConnect && PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated)
		{
			Debug.Log ("PhotonNetwork.ConnectUsingSettings()");
			PhotonNetwork.ConnectUsingSettings(VersionNumber);
		}
	}
	
	#endregion
}
