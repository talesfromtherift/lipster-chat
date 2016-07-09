using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class VRInitialize : MonoBehaviour 
{
	[Header("On Awake")]
	public bool Recenter = true;

	void Awake () 
	{
		if (Recenter) 
		{
			InputTracking.Recenter ();
		}
	}
}
