/*
 * Copyright (c) 2015-2016 Peter Koch <peterept@gmail.com>
 * 
 * Network Room Exit Handler
 * 
 * Purpose: For room scenes, automatically return to lobby when the player leaves the networked room
 */
using UnityEngine;
using System.Collections;

public class NetworkRoomExitHandler : MonoBehaviour 
{
	public string LobbyLevelName = "Lobby";

	#region Photon

	public void OnLeftRoom()
	{
		LoadLobby ();
	}
	
	public void OnDisconnectedFromPhoton()
	{
		LoadLobby ();
	}

	void LoadLobby()
	{
       Application.LoadLevel (LobbyLevelName);
	}

	#endregion

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}
}
