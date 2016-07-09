/*
 * Copyright (c) 2015-2016 Peter Koch <peterept@gmail.com>
 * 
 * Lipster Lobby Menu
 * 
 * Purpose: Update UI in the lobby
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TalesFromTheRift;

namespace Lipster
{
	public class LipsterLobbyMenu : MonoBehaviour 
	{
		public Text InstructionsText;
		public GameObject RoomsPanel;
		public GameObject AvatarPanel;
		public Text PlayersNameText;
		public InputField PlayersNameInputField;

		void Start () 
		{
			// Instructions are slightly different on GearVR 
			#if UNITY_ANDROID
			InstructionsText.text = InstructionsText.text.Replace("ESC", "Back");
			#else
			InstructionsText.text += "\nHold SPACE while talking, or T to toggle talking mode";
			#endif

			// If player has no name, go to settings
			bool showSettings = !LipsterPlayerSettings.HasPlayerName ();
			ShowRoom (!showSettings);
		}		


		void ShowRoom(bool Menu)
		{
			if (Menu) 
			{
				PlayersNameText.text = string.Format ("Welcome {0}", LipsterPlayerSettings.PlayerName); 
			} 
			else 
			{
				PlayersNameInputField.text =  LipsterPlayerSettings.PlayerName;
			}
			RoomsPanel.SetActive(Menu);
			AvatarPanel.SetActive(!Menu);
		}

		public void ShowRoomMenu()
		{
			Fader.Instance.FadeOutAndIn (() =>
			{
				ShowRoom (true);
			});

		}

		public void ShowAvartarMenu()
		{
			Fader.Instance.FadeOutAndIn (() =>
	         {
				ShowRoom (false);
			});
		}

		public void SavePlayerName()
		{
			string name = CanvasKeyboard.CurrentInputTarget ().GetComponent<InputField> ().text;
			if (!string.IsNullOrEmpty (name)) 
			{
				// Save players name
				LipsterPlayerSettings.PlayerName = name;

				// Back to room menu
				ShowRoomMenu();
			}
		}

	}

}