// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// original action : https://hutonggames.com/playmakerforum/index.php?topic=22222.msg97434#msg97434

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.Application)]
	[Tooltip("This action lets you write a string value into the devices clipboard.")]
	public class WriteToClipboard : FsmStateAction
	{

		[Tooltip("String value that is written into the Clipboard")]
		public FsmString String;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			SetClipboard();
					if (!everyFrame) {
					Finish();
			}			
		}
		public override void OnUpdate()
		{
			SetClipboard();
		}

		void SetClipboard() {
			GUIUtility.systemCopyBuffer = String.Value;	
		}
	}
}
