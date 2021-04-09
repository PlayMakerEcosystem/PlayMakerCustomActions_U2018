// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine.XR;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("XR")]
	[Tooltip("Controls the actual size of eye textures as a multiplier of the device's default resolution.A value of 1.0 will use the default eye texture resolution specified by the XR device. Values less than 1.0 will use lower resolution eye textures, which may improve performance at the expense of a less sharp image. Values greater than 1.0 will use higher resolution eye textures, resulting in a potentially sharper image at a cost to performance and increased memory usage.")]
	public class XRSettingsGetEyeTextureResolutionScale : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The actual size of eye textures as a multiplier of the device's default resolution")]
		[UIHint(UIHint.Variable)]
		public FsmFloat EyeTextureResolutionScale;

		public override void Reset()
		{
			EyeTextureResolutionScale = null;
		}

		public override void OnEnter()
		{
			EyeTextureResolutionScale.Value = XRSettings.eyeTextureResolutionScale;

			Finish ();
		}

	}
}