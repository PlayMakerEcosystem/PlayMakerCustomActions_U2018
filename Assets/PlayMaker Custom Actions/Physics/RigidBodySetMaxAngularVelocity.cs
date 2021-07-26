// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Set The maximimum angular velocity of the rigidbody. (Default 7) range { 0, infinity }.")]
	public class RigidBodySetMaxAngularVelocity : ComponentAction<Rigidbody>
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		[Tooltip("The GameObject to set the  maximimum angular velocity of.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The maximimum angular velocity. (Default 7) range { 0, infinity }.")]
		public FsmFloat maxAngularVelocity;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			maxAngularVelocity = 7f;
			everyFrame = false;
		}

		
		public override void OnEnter()
		{
			DoSetMaxAngularVelocity();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}
		
		public override void OnUpdate()
		{
			DoSetMaxAngularVelocity();
		}
		
		void DoSetMaxAngularVelocity()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (!UpdateCache(go))
			{
				return;
			}

			rigidbody.maxAngularVelocity = maxAngularVelocity.Value;
	
		}

	
	}
}