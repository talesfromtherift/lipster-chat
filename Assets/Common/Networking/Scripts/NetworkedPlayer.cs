/*
 * Copyright (c) 2015-2016 Peter Koch <peterept@gmail.com>
 * 
 * Networked Player
 * 
 * Purpose: Synchronise players body and head transform over the network 
 */
using UnityEngine;
using System.Collections;

public class NetworkedPlayer : Photon.MonoBehaviour  
{
	public Transform PlayersHead;

	[Header("Local Player Components")]
	public Camera LocalPlayerCamera;
	public AudioListener LocalPlayerAudioListener;

	[Header("Local Player")]
	public string LocalPlayerGameObjectName = "Player";
	public Component[] LocalPlayerComponentsToEnable;
	public Component[] LocalPlayerComponentsToDisable;
	public GameObject[] LocalPlayerGameObjectsToEnable;

	// Networked Data
	private int networkSequence = 0;
	private Vector3 correctPlayerPos = Vector3.zero; 
	private Quaternion correctPlayerRot = Quaternion.identity; 
	private Vector3 correctPlayerHeadPos = Vector3.zero; 
	private Quaternion correctPlayerHeadRot = Quaternion.identity; 


	#region MonoBehaviour

	void Awake ()
	{
		// if I am the local player, enable things like camera and controls, etc
		if (photonView.isMine) 
		{
			name = LocalPlayerGameObjectName;
			foreach (GameObject g in LocalPlayerGameObjectsToEnable)
			{
				g.SetActive(true);
			}
			foreach (Behaviour b in LocalPlayerComponentsToEnable)
			{
				b.enabled = true;
			}
			foreach (Behaviour b in LocalPlayerComponentsToDisable)
			{
				b.enabled = false;
			}
		}
	}

	void Update()
	{
		if (!photonView.isMine && networkSequence > 0)
		{
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 10);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 10);
			PlayersHead.position = Vector3.Lerp(PlayersHead.position, this.correctPlayerHeadPos, Time.deltaTime * 10);
			PlayersHead.rotation = Quaternion.Lerp(PlayersHead.rotation, this.correctPlayerHeadRot, Time.deltaTime * 10);		
		}
	}

	#endregion


	#region Photon

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(++networkSequence);
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(PlayersHead.position);
			stream.SendNext( PlayersHead.rotation);
		}
		else
		{
			int sequence = (int)stream.ReceiveNext();
			if (sequence > networkSequence)
			{
				networkSequence = sequence;
				this.correctPlayerPos = (Vector3)stream.ReceiveNext();
				this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
				this.correctPlayerHeadPos = (Vector3)stream.ReceiveNext();
				this.correctPlayerHeadRot = (Quaternion)stream.ReceiveNext();
			}
		}
	}

	#endregion
}
