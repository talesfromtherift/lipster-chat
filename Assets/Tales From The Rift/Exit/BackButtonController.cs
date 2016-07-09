using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BackButtonController : MonoBehaviour 
{
	public UnityEvent onBack;
	public UnityEvent onExit;

	float longPressTimeExpiry = float.MaxValue;
	bool longPressTriggered = false;

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			// Long-press: he user presses the button down and holds it for > 0.75 seconds. A longpress should always open the Universal Menu.
			longPressTimeExpiry = Time.realtimeSinceStartup + 0.75f;
			longPressTriggered = false;
		}

		if (Input.GetKey(KeyCode.Escape)) 
		{
			if (longPressTriggered == false && Time.realtimeSinceStartup > longPressTimeExpiry) {
				Debug.Log ("onExit Pressed");
				longPressTriggered = true;
				onExit.Invoke ();
			}
		}

		if (Input.GetKeyUp (KeyCode.Escape)) 
		{
			if (longPressTriggered == false)
			{
				Debug.Log ("onBack Pressed");
				onBack.Invoke();
			}
		}
	}
}
