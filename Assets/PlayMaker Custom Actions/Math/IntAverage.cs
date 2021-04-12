// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Gets the average of all listed int variables.")]
	public class IntAverage : FsmStateAction
	{
		[RequiredField]
		public FsmInt[] intArray;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			intArray = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoAverage();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoAverage();
		}

		void DoAverage()
		{
			int i = 0;
			float average = new float();
			while (i < intArray.Length)
			{
				average += intArray[i].Value;
				i++;
			}

			storeResult.Value = average / (intArray.Length);
		}
	}
}