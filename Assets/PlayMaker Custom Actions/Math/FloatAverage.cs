// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Gets the average of all listed float variables.")]
	public class FloatAverage : FsmStateAction
	{
		[RequiredField]
		public FsmFloat[] floatArray;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			floatArray = null;
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
			while (i < floatArray.Length)
			{
				average += floatArray[i].Value;
				i++;
			}

			storeResult.Value = average / (floatArray.Length);
		}
	}
}
