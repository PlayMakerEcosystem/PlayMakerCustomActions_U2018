// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.
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

	#pragma warning disable CS0168

	[ActionCategory("Timeline")]
	[Tooltip("Set Generic Binding for a PlayableDirector")]
	public class  SetPlayableDirectorGenericBinding : FsmStateAction
	{
		[CheckForComponent(typeof(PlayableDirector))]
		[Tooltip("The game object to hold the unity timeline components.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The Track Index, leave to none to pick up the first or default")]
		public FsmInt trackIndex;

		[Tooltip("The Track Name, leave to none to pick up the first or default")]
		public FsmString trackName;

		[Tooltip("The game object To Bind to the PlayableDirector Timeline")]
		public FsmOwnerDefault bindingObject;

		[Tooltip("EVent Sent if Track is not found")]
		public FsmEvent failureEvent;


	
		PlayableDirector timeline;
		TimelineAsset timelineAsset;
		PlayableBinding _playableBinding;

		public override void Reset()
		{
			gameObject = null;
			bindingObject = null;
			trackIndex = new FsmInt() {UseVariable=true};
			trackName = new FsmString() {UseVariable=true};
			failureEvent = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			timeline = go.GetComponent<PlayableDirector>();

			timelineAsset = (TimelineAsset)timeline.playableAsset;

			timelineAction();
		}

		void timelineAction()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null || timeline == null || timelineAsset == null)
			{
				return;
			}

			try{
			if (trackIndex.IsNone && trackName.IsNone) {
				_playableBinding = timelineAsset.outputs.FirstOrDefault ();
			} else {
				
				if (!trackIndex.IsNone && (trackIndex.Value > 0 || trackIndex.Value  < timelineAsset.outputs.Count())) {
					_playableBinding =	timelineAsset.outputs.ElementAt<PlayableBinding> (trackIndex.Value);
				}

				if (!trackName.IsNone && !string.IsNullOrEmpty (trackName.Value)) {
					_playableBinding =	timelineAsset.outputs.First (c => c.streamName == trackName.Value);
				}
			}
			}catch(Exception e) {
				
					Fsm.Event (failureEvent);
					return;
			}

			timeline.SetGenericBinding(_playableBinding.sourceObject,Fsm.GetOwnerDefaultTarget (bindingObject));

		}

	}
}