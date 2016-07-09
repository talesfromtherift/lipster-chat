using UnityEngine;
using System.Collections;

public class VoiceChatPacketPhotonTypeRegisterer : MonoBehaviour 
{
	void Awake () 
	{
		VoiceChatPacketPhotonType.Register ();
	}	
}
