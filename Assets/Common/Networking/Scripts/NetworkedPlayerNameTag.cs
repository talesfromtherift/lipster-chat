using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class NetworkedPlayerNameTag : Photon.MonoBehaviour 
{
	public Transform nameTagTransform;
	public Text nameTagText;
	public Transform playersHeadTransform;

	public string LocalPlayerName = "Player";

	Transform _localPlayer;
	Transform LocalPlayer
	{
		get 
		{
			if (_localPlayer == null)
			{
				GameObject player = GameObject.Find (LocalPlayerName);
				if (player != null) {
					_localPlayer = player.transform;
				}
			}
			return _localPlayer;
		}
	}

	void Start () 
	{
		nameTagText.text = photonView.owner.name;	
	}


	void Update () 
	{
		// The name tag always straight up and faces the local player
		Transform FaceTowards = LocalPlayer; 
		if (FaceTowards) 
		{
			// if we are the local player, make it face straight forwards
			if (FaceTowards.gameObject == gameObject)
			{
				Vector3 rot = playersHeadTransform.rotation.eulerAngles;
				nameTagTransform.transform.rotation = Quaternion.Euler(new Vector3(0, rot.y, 0)) *  Quaternion.Euler(new Vector3(0, 180, 0));
			}
			else
			{
				Vector3 pos = new Vector3 (FaceTowards.position.x, transform.position.y, FaceTowards.position.z);
				Vector3 dir = pos - transform.position;
				nameTagTransform.transform.rotation = Quaternion.LookRotation (-dir, Vector3.up);
			}
		}
	}

}
