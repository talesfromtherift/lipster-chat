using UnityEngine;
using System.Collections;

public class DontDestroyOnLoadHelper : MonoBehaviour 
{
	static bool created = false;

	void Awake () 
	{
		if (created == false) 
		{
			DontDestroyOnLoad (gameObject);	
			created = true;
		} else 
		{
			DestroyImmediate(gameObject);
		}
	}
}
