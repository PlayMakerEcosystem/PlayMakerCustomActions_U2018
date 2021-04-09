// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine.VR;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("XR")]
	[Tooltip("Set Type of VR to the desired VR device and it will be loaded. Note that if a VR device was already loaded, a restart may be forced.")]
	public class XRSettingsLoadDeviceByName : FsmStateAction
	{
		[Tooltip("Type of VR device to load")]
		public FsmString deviceType;

		public override void Reset()
		{
			deviceType = null;
		}

		public override void OnEnter()
		{
			UnityEngine.XR.XRSettings.LoadDeviceByName(deviceType.Value);

			Finish ();
		}

	}
}