// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// original action : https://hutonggames.com/playmakerforum/index.php?topic=22222.msg97434#msg97434
using UnityEngine;


namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.Application)]
	[Tooltip("This action lets you read a value from the systems clipboard and store it in a string.")]
	public class ReadFromClipboard : FsmStateAction
	{
		[Tooltip("String variable that will receive the clipboard content.")]
		[UIHint(UIHint.Variable)]
		public FsmString String; 

		[Tooltip("Repeat every frame while the state is active.")]
    	public bool everyFrame;
        
		public override void OnEnter()
		{
			GetClipboard();

			if (!everyFrame) {
				Finish();
			}
		}

		public override void OnUpdate()
		{
			GetClipboard();
		}

		void GetClipboard() {
			String.Value = GUIUtility.systemCopyBuffer;
		}
	}
}
