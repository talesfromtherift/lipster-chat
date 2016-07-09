/*
 * Copyright (c) 2015-2016 Peter Koch <peterept@gmail.com>
 * 
 * Lipster Analytics
 * 
 * Purpose: Record some analytics for user activity in any scene
 */
using UnityEngine;
using System.Collections;

public class LipsterAnalytics : Photon.MonoBehaviour 
{
	#region Photon

	public void OnJoinedRoom()
	{
		MixpanelLite.Mixpanel.Instance.Track("JoinedRoom", new Hashtable() {
			{"Room", PhotonNetwork.room.name},
			{"PlayersName", PhotonNetwork.player.name},
			{"PlayersCount", PhotonNetwork.room.playerCount}
		});
	}

	public void OnLeftRoom()
	{
		MixpanelLite.Mixpanel.Instance.Track ("leftRoom");
	}
	#endregion

}
