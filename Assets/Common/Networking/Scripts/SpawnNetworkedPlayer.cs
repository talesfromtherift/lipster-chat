/*
 * Copyright (c) 2015-2016 Peter Koch <peterept@gmail.com>
 * 
 * Spawn Networked Player
 * 
 * Purpose: When joining a room, this component can spawn a new player at a spawn spot
 */
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]

public class SpawnNetworkedPlayer : Photon.MonoBehaviour 
{
	public string PlayerPrefabName = "";
	public Transform[] SpawnSpots;

	private PhotonView ScenePhotonView;

	#region Mono

	public virtual void Awake()
	{
		ScenePhotonView = this.GetComponent<PhotonView>();
	}

	void Start()
	{
		if (PhotonNetwork.room != null) 
		{
			OnJoinedRoom();
		}
	}

	#endregion


	#region Photon

	public void OnJoinedRoom()
	{
		// master client must spawn themselves
		if (PhotonNetwork.isMasterClient)
		{
			Spawn(PhotonNetwork.player);
		}
	}
	
	public virtual void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		// master client will spawn all other players when they join the room
		if (PhotonNetwork.isMasterClient)
		{
			Spawn(newPlayer);
		}
	}	

	#endregion


	// find the next available spawn spot for this player
	int AssignSpawnSpotToPlayer(PhotonPlayer player)
	{
		// Track players spots in a room property (so any new Master Client can access it)
		int[] spawnSpotPlayers = (int[])PhotonNetwork.room.customProperties ["SpawnSpotPlayers"];
		
		// Find an empty spawn point and allocate the player to it
		if (spawnSpotPlayers == null)
		{
			spawnSpotPlayers = new int[SpawnSpots.Length];
		}
		
		// remove any stale players
		for (int i = 0; i < spawnSpotPlayers.Length; i++) {
			int playerId = spawnSpotPlayers[i];
			bool found = false;
			foreach (PhotonPlayer p in PhotonNetwork.playerList)
			{
				if (p.ID == playerId)
				{
					found = true;
					break;
				}
			}
			if (found == false)
			{
				spawnSpotPlayers[i] = 0;
			}
		}
		
		// find an empty (or invalid player) spawn spot
		int spawnSpotIndex = 0;
		for (int i = 0; i < spawnSpotPlayers.Length; i++) {
			if (spawnSpotPlayers[i] == 0)
			{
				spawnSpotIndex = i;
				break;
			}
		}
		
		// set it to the player
		spawnSpotPlayers[spawnSpotIndex] = player.ID;
		ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable () { { "SpawnSpotPlayers", spawnSpotPlayers } };
		PhotonNetwork.room.SetCustomProperties (roomProps);
		
	//	string s = "";
	//	foreach (int n in spawnSpotPlayers) {
	//		s += " " + n;
	//	}
	//	Debug.Log (s);
		
		return spawnSpotIndex;
	}
	
	public void Spawn(PhotonPlayer newPlayer)
	{
		int spawnSpotIndex = AssignSpawnSpotToPlayer(newPlayer);
		ScenePhotonView.RPC("OnSpawn", newPlayer, SpawnSpots[spawnSpotIndex].position, SpawnSpots[spawnSpotIndex].rotation);
	}
	
	[PunRPC]
	public void OnSpawn(Vector3 position, Quaternion rotation)
	{
		Debug.Log ("Spawn ME!");
		PhotonNetwork.Instantiate(PlayerPrefabName, position, rotation, 0);
	}
}
