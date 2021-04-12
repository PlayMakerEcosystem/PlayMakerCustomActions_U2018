// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HutongGames.PlayMaker.Actions
{

	#pragma warning disable 168

	[ActionCategory("Timeline")]
	[Tooltip("Set a Playable  speed")]
	public class SetPlayableSpeed : FsmStateAction
	{
		[CheckForComponent(typeof(PlayableDirector))]
		[Tooltip("The game object to hold the unity timeline components.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The Track Index of the playbale")]
		public FsmInt trackIndex;

		[Tooltip("The  speed of the playable")]
		public FsmFloat speed;

		[Tooltip("Event Sent if Track is not found")]
		public FsmEvent failureEvent;

		[Tooltip("repeat every frame")]
		public bool everyFrame;


	
		PlayableDirector timeline;

		public override void Reset()
		{
			
			gameObject = null;
			trackIndex = null;
			everyFrame = false;
			speed = null;

			failureEvent = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			timeline = go.GetComponent<PlayableDirector>();

			execute();

			if (!everyFrame)
			{
				Finish ();
			}
		}


		public override void OnUpdate()
		{
			execute();
		}

		void execute()
		{
			if (timeline == null)
			{
				return;
			}

			try{

                timeline.playableGraph.GetRootPlayable(trackIndex.Value).SetSpeed(Math.Max(0,speed.Value));
			}catch(Exception e) {
				
					Fsm.Event (failureEvent);
					return;
			}


		}

	}
}