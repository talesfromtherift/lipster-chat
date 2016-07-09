using UnityEngine;
using System.Collections;

public class LipsterPlayerSettings
{
	static string _playerName;
	public static string PlayerName
	{
		get 
		{
			if (string.IsNullOrEmpty(_playerName))
			{
				_playerName = PlayerPrefs.GetString("player_name", "");
			}
			return _playerName;
		}
		
		set
		{
			_playerName = value;
			PlayerPrefs.SetString("player_name", _playerName);
			PlayerPrefs.Save();
		}
	}

	public static bool HasPlayerName()
	{
		return !string.IsNullOrEmpty (PlayerName);
	}

}
