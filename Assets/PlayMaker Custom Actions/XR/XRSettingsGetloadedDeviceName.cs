// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine.XR;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("XR")]
	[Tooltip("Get Type of VR device that is currently in use.")]
	public class XRSettingsGetloadedDeviceName : FsmStateAction
	{

		[Tooltip("Type of VR device currently in use")]
		[UIHint(UIHint.Variable)]
		public FsmString deviceName;

		[Tooltip("No VR Device Event")]
		public FsmEvent noDevice;

		[Tooltip("Is the device is Active")]
		public FsmBool isDeviceActive;
		
		[Tooltip("Event sent if not active")]
		public FsmEvent IsNotActiveEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Stereo rendering mode")]
		[ObjectType(typeof(XRSettings.StereoRenderingMode))]
		public FsmEnum stereoRenderingMode;
		
		
		public override void Reset()
		{
			deviceName = null;

			noDevice = null;
			isDeviceActive = null;
			stereoRenderingMode = null;
			IsNotActiveEvent = null;
		}

		public override void OnEnter()
		{
			deviceName.Value = XRSettings.loadedDeviceName;

			if ( string.IsNullOrEmpty(XRSettings.loadedDeviceName) ) {
				Fsm.Event (noDevice);
			}

			isDeviceActive.Value = XRSettings.isDeviceActive;

			if (!XRSettings.isDeviceActive)
			{
				Fsm.Event (IsNotActiveEvent);
			}
			
			if (stereoRenderingMode != null   )
			{
				stereoRenderingMode.RawValue = XRSettings.stereoRenderingMode;
			}
			
			Finish ();
		}

	}
}