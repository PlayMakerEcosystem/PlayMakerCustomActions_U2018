// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine.XR;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("XR")]
	[Tooltip("Center tracking on the current pose.")]
	public class XRInputTrackingRecenter : FsmStateAction
	{
		public override void OnEnter()
		{
			InputTracking.Recenter();
			Finish();		
		}

	}
}