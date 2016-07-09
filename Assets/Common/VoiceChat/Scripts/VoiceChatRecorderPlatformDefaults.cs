using UnityEngine;
using System.Collections;
using VoiceChat;

[RequireComponent(typeof(VoiceChatRecorder))]

public class VoiceChatRecorderPlatformDefaults : MonoBehaviour 
{
	void Awake()
	{
		VoiceChatRecorder recorder = GetComponent<VoiceChatRecorder>();
#if UNITY_ANDROID
		// Assume always have headphones to chat
		recorder.transmitToggled = true;
#else
#endif
	}
}
