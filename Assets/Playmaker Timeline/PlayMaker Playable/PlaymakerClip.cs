using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

using HutongGames.PlayMaker;

using HutongGames.PlayMaker.Ecosystem.Utils;

namespace HutongGames.PlayMaker.Ecosystem.Timeline
{
	[Serializable]
	public class PlaymakerClip : PlayableAsset, ITimelineClipAsset
	{
		private PlaymakerBehaviour template = new PlaymakerBehaviour ();



		public PlayMakerTimelineEventTarget eventTarget = new PlayMakerTimelineEventTarget(false);

		[EventTargetVariable("eventTarget")]
		//[ShowOptions]
		public PlayMakerEvent OnPlay;


		[EventTargetVariable("eventTarget")]
		//[ShowOptions]
		public PlayMakerEvent OnFinished;

		public bool debug;


		#region ITimelineClipAsset Interface
	    public ClipCaps clipCaps
	    {
	        get { return ClipCaps.None; }
	    }

		#endregion

	    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
	    {
			var playable = ScriptPlayable<PlaymakerBehaviour>.Create (graph,template);
			var _t =	playable.GetBehaviour ();

			this.eventTarget.SetOwner (owner);
			this.eventTarget.Resolve (graph.GetResolver ());

			_t.SetContext (owner, graph,this);
		
	        return playable;
	    }
	}
}
