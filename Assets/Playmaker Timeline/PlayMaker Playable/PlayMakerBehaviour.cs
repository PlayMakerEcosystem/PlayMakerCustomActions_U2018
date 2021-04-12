using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

using HutongGames.PlayMaker;

using HutongGames.PlayMaker.Ecosystem.Utils;

namespace HutongGames.PlayMaker.Ecosystem.Timeline
{
	[Serializable]
	public class PlaymakerBehaviour : PlayableBehaviour
	{

		private PlaymakerClip _clip;

		private bool isPlaying;

		private string _debug ="";

		public void SetContext(GameObject owner,PlayableGraph graph, PlaymakerClip clip)
		{
			_clip = clip;
		}

		#region PlayableBehaviour
		public override void OnBehaviourPause (Playable playable, FrameData info)
		{
			if (isPlaying)
			{
				SendPlayMakerEvent (_clip.OnFinished);
			}

			isPlaying = false;
		}

		public override void OnBehaviourPlay (Playable playable, FrameData info)
		{
			isPlaying = true;
			SendPlayMakerEvent (_clip.OnPlay);
		}

		#endregion



		protected void SendPlayMakerEvent(PlayMakerEvent pmEvent)
		{

			if (_clip.debug )
			{
				_debug = string.Empty;
				if (!Application.isPlaying)
				{
					_debug = "<color=RED>Application must run to send a PlayMaker Event, but the playable proxy at least works</color>\n";

				}

				_debug += "Send " + pmEvent.ToString () + " on " + _clip.eventTarget.ToString ();

				UnityEngine.Debug.Log (_debug);
			}

			if (Application.isPlaying)
			{
				pmEvent.SendEvent (null, _clip.eventTarget,_clip.debug);
			}
		}

	}
}