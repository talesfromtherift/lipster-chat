/*
 * Copyright (c) 2015-2016 Peter Koch <peterept@gmail.com>
 * 
 * Lipster Lobby Room Row
 * 
 * Purpose: The row button which lists the room name and provides a join button
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Lipster
{
	public class LipsterLobbyMenuRoomRow : MonoBehaviour 
	{
		string roomName;
		public string RoomName {
			get { return roomName; }
			set { roomName = value;
				  Refresh (); }
		}
		int maxPlayers;
		public int MaxPlayers {
			get { return maxPlayers; }
			set { maxPlayers = value;
				Refresh (); }
		}
		int playersCount;
		public int PlayersCount {
			get { return playersCount; }
			set { playersCount = value;
				Refresh (); }
		}

		public UnityEvent onClick;
		public Text NameText;
		public Button JoinButton;
		public Text JoinButtonText;

		public void Refresh()
		{
			NameText.text = string.Format ("{0} [{1}/{2}]", roomName, playersCount, maxPlayers);
			if (playersCount == maxPlayers)
			{
				JoinButtonText.text = "Full";
				JoinButton.interactable = false;

			}
			else
			{
				JoinButtonText.text = "Join";
				JoinButton.interactable = true;
			}
		}

		public void OnJoin()
		{
			onClick.Invoke ();
		}

	}
}