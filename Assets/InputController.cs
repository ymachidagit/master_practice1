using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputController : MonoBehaviour
{
  private SteamVR_Action_Vector2 TrackPad = SteamVR_Actions.default_myTrackPad;
	private SteamVR_Action_Boolean TrackPadPress = SteamVR_Actions.default_myTrackPadPress;
	private SteamVR_Action_Single squeeze = SteamVR_Actions.default_Squeeze;
  public Vector2 posRight;
	public bool isPressedRight;
	public float squeezeRight;

	void Start()
	{
			
	}
	void Update()
	{
		posRight = TrackPad.GetLastAxis(SteamVR_Input_Sources.RightHand);
		isPressedRight = TrackPadPress.GetState(SteamVR_Input_Sources.RightHand);
		squeezeRight = squeeze.GetLastAxis(SteamVR_Input_Sources.RightHand);
		// Debug.Log(posRight.x + " " + posRight.y);
		// Debug.Log("isPressedRight" + isPressedRight);
	}
}
