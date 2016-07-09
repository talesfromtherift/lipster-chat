/*
 * Copyright (c) 2015-2016 Peter Koch <peterept@gmail.com>
 * 
 * Lipster Lobby
 * 
 * Purpose: Update a list of pre-defined rooms for the player to join, handle the join requests by loading the correct Unity level.
 */
using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.UI;

namespace Lipster
{
	public class LipsterNetworkedLobby : Photon.MonoBehaviour
	{
		// Pre-defined rooms
		[System.Serializable]
		public class RoomDefinition 
		{
			public string name;
			public int maxPlayers;
			public string levelName;
		}
		public RoomDefinition[] rooms;

		[Header("Rooms List UI")]
		public Transform RoomsPanel;
		public GameObject RoomButton;
	

		private RoomDefinition FindRoom(string name)
		{
			foreach (RoomDefinition room in rooms) {
				if (room.name == name) {
					return room;
				}
			}
			return null;
		}

		public void JoinRoom(string roomName)
		{
			Debug.LogFormat("Lobby.JoinRoom({0})", roomName);
			RoomDefinition room = FindRoom (roomName);
			if (room != null) 
			{
				// make sure we set out player name
				PhotonNetwork.player.name = LipsterPlayerSettings.PlayerName;

				RoomOptions roomOptions = new RoomOptions () { maxPlayers = (byte)room.maxPlayers };
				PhotonNetwork.JoinOrCreateRoom (roomName, roomOptions, null);
			}
		}


		#region Photon	

		// refresh the lobby ui
		public virtual void OnReceivedRoomListUpdate()
		{
			Debug.LogFormat("Lobby.OnReceivedRoomListUpdate()  Room Count = {0}", PhotonNetwork.GetRoomList().Length);

			// Clear the current room list UI
			foreach (Transform child in RoomsPanel) {
				GameObject.Destroy(child.gameObject);
			}
			
			// Add each room
			foreach (RoomDefinition r in rooms) 
			{
				// find the matching photon room (if any)
				RoomInfo photonRoom = null;
				foreach (RoomInfo game in PhotonNetwork.GetRoomList()) 
				{
					if (game.name == r.name)
					{
						photonRoom = game;
						break;
					}
				}

				// Add a UI row for each room
				GameObject roomRow = GameObject.Instantiate (RoomButton);
				roomRow.transform.SetParent (RoomsPanel, false);
				LipsterLobbyMenuRoomRow row = roomRow.GetComponent<LipsterLobbyMenuRoomRow>();
				row.RoomName = r.name;
				row.MaxPlayers = r.maxPlayers;
				row.PlayersCount = photonRoom != null ? photonRoom.playerCount : 0;
				string name = r.name;
				row.onClick.AddListener(() => { JoinRoom(name);});
			}

		}

		// load the desired level
		public void OnJoinedRoom()
		{
			Debug.LogFormat("OnJoinedRoom() Room = {0} [{1}/{2}]", PhotonNetwork.room.name, PhotonNetwork.room.playerCount, PhotonNetwork.room.maxPlayers);
			RoomDefinition room = FindRoom (PhotonNetwork.room.name);
			if (room != null) 
			{
				PhotonNetwork.isMessageQueueRunning = false;
				Fader.Instance.FadeOut(() =>
                {
					PhotonNetwork.LoadLevel (room.levelName);
				});
			}
		}

		#endregion

	}
}