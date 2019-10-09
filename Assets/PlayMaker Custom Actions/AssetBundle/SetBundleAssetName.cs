// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("AssetBundle")]
	[Tooltip("Sets the bundle asset name and variant for a given object")]
	public class SetBundleAssetName : FsmStateAction
	{
		public FsmObject target;
		
		[Tooltip("or path")]
		public FsmString path;

		public FsmString assetBundleName;

		public FsmString assetBundleVariant;

		[ActionSection("Results")] 
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Error message if there was an error during the download.")]
		public FsmString errorString;
		
		[Tooltip("Event to send when the bundle has finished loading (progress = 1).")]
		public FsmEvent doneEvent;
		
		[Tooltip("Event to send if there was an error.")]
		public FsmEvent errorEvent;

		public override void Reset()
		{
			target = null;
			path = new FsmString{UseVariable = true};
			assetBundleName = null;
			assetBundleVariant = null;
			doneEvent = null;
			errorEvent = null;
			errorString = null;
		}

		public override void OnEnter()
		{
			string _path = "";
			
			if (target.Value != null)
			{
				#if UNITY_EDITOR
				_path = AssetDatabase.GetAssetPath(target.Value);
				#endif
			}
			else if (!path.IsNone)
			{
				_path = path.Value;
			}
			
			if (string.IsNullOrEmpty(path.Value))
			{
				errorString.Value = "path is null or empty";
			}
			else
			{
				#if UNITY_EDITOR
				AssetImporter.GetAtPath(_path).SetAssetBundleNameAndVariant(assetBundleName.Value, assetBundleVariant.Value);
				#endif
				errorString.Value = string.Empty;
			}

			if (!Application.isEditor)
			{
				errorString.Value = "Action can not be executed without the Unity editor";
			}
			
			Fsm.Event(string.IsNullOrEmpty(errorString.Value) ? doneEvent : errorEvent);
				
			Finish();

		}
	}
}
