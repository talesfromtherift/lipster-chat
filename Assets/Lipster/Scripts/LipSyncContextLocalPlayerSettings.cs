using UnityEngine;
using System.Collections;

[RequireComponent(typeof(OVRLipSyncContext))]
public class LipSyncContextLocalPlayerSettings : MonoBehaviour 
{
	public bool audioMute = true;

	void Start () 
	{
		// Local Player should always be muted
		GetComponent<OVRLipSyncContext> ().audioMute = audioMute;
	}

	void Update()
	{
	}
}
