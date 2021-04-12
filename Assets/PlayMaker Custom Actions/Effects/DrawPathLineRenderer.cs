// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// Based on DrawLine.cs http://hutonggames.com/playmakerforum/index.php?topic=3943.0

using UnityEngine;

namespace HutongGames.PlayMaker.Actions {
	[ActionCategory( ActionCategory.Effects )]
	[Tooltip( "Draws a path using an array of positions using LineRenderer." )]
	public class DrawPathLineRenderer : ComponentAction<LineRenderer>
	{

		[Tooltip("The GameObject with the LineRenderer component ")]
		[RequiredField]
		[CheckForComponent(typeof(LineRenderer))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Path definition")]
		[ArrayEditor(VariableType.Vector3)]
		public FsmArray Points;

		[Tooltip( "Updates positions every frame" )]
		public bool everyFrame;
	
		
		public override void Reset ()
		{
			gameObject = null;	
			Points = null;
			everyFrame = false;
		}

		public override void OnEnter () {	
			
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject))) return;
			
			DoUpdatePositions();
			
			if( !everyFrame ) {
				Finish();
			}
		}

		public override void OnUpdate () {
		
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject))) return;
			
			DoUpdatePositions();
		}
		
		void DoUpdatePositions ()
		{

			this.cachedComponent.positionCount = Points.Length;
			for (int i = 0; i < Points.Length; i++)
			{
				this.cachedComponent.SetPosition(i,(Vector3)Points.Values[i]);
			}

		}
	}
}