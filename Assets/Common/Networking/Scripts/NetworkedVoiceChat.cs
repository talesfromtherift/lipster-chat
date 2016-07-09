/*
 * Copyright (c) 2015-2016 Peter Koch <peterept@gmail.com>
 * 
 * Networked Voice Chat
 * 
 * Purpose: Synchronise VOIP over the network
 */
using UnityEngine;
using System;
using System.Collections;
using VoiceChat;
using VoiceChat.Networking.Legacy;

public class NetworkedVoiceChat : Photon.MonoBehaviour 
{
	public VoiceChatPlayer player;
	public AudioSource audioSource;
	public bool AudioSourceIsMicrophoneForlocalPlayer = true;
	
	private int seq = 0;
	
	void Start() 
	{
		if (photonView.isMine) 
		{
			VoiceChatRecorder.Instance.NetworkId = PhotonNetwork.player.ID;
			VoiceChatRecorder.Instance.NewSample += HandleNewSample;
			VoiceChatRecorder.Instance.StartRecording ();

			// if we are the local player - we want to directly route microphone input to our player 
			if (AudioSourceIsMicrophoneForlocalPlayer)
			{
				audioSource.clip = VoiceChatRecorder.Instance.clip;
				audioSource.Play();
			}
		}
	}
	
	void Destroy()
	{
		if (photonView.isMine) 
		{
			VoiceChatRecorder.Instance.NewSample -= HandleNewSample;
			VoiceChatRecorder.Instance.StopRecording();
		}
	}
	
	void HandleNewSample(VoiceChatPacket obj)
	{
		photonView.RPC("VoiceChat", PhotonTargets.Others, ++seq, obj);
	}
	
	[PunRPC]
	void VoiceChat(int seq, VoiceChatPacket obj)
	{
		if (seq <= this.seq) return;
		if (!audioSource.enabled) return;
		
		this.seq = seq;
		player.OnNewSample(obj);
	}
}
