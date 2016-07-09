using UnityEngine;
using System.Collections;

[RequireComponent(typeof(OVRLipSyncContextTextureFlip))]
public class SeetupOVRLipSyncContextTextureFlip : MonoBehaviour 
{
	public Renderer MouthRenderer;

	void Awake() 
	{
		GetComponent<OVRLipSyncContextTextureFlip> ().material = MouthRenderer.material;
	}
}
