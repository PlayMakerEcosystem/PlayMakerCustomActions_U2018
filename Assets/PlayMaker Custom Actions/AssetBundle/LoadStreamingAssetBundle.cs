// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;
using System;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("AssetBundle")]
	[Tooltip("Gets a Bundle from the streaming assets and store it in a FsmObject. See Bundles docs for more details.")]
	public class LoadStreamingAssetBundle : FsmStateAction
	{
		[RequiredField]
		[Tooltip("path to the bundle from within the StreamingAsset Folder.")]
		public FsmString path;
		
		
		[ActionSection("Results")]

		[UIHint(UIHint.Variable)]
		[ObjectType(typeof(AssetBundle))]
		[Tooltip("Gets bundle from the url.")]
		public FsmObject storeBundle;

		[UIHint(UIHint.Variable)]
		[Tooltip("Error message if there was an error during the download.")]
		public FsmString errorString;

		[UIHint(UIHint.Variable)] 
		[Tooltip("How far the download progressed (0-1).")]
		public FsmFloat progress;

		[ActionSection("Events")] 
		
		[Tooltip("Event to send when the bundle has finished loading (progress = 1).")]
		public FsmEvent isDone;
		
		[Tooltip("Event to send if there was an error.")]
		public FsmEvent isError;


		AssetBundleCreateRequest _request;

		public override void Reset()
		{
			
			storeBundle = null;
			errorString = null;
			progress = null;
			isDone = null;
		}

		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(path.Value))
			{
				errorString.Value = "path is null or empty";
				Finish();
				return;
			}

			_request = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath+"/"+path.Value);
			
		}


		public override void OnUpdate()
		{
			if (_request == null)
			{
				errorString.Value = "Missing loading request";
				Finish();
				return;
			}
				
			progress.Value = _request.progress;

			if (_request.isDone)
			{
				
                AssetBundleManifest manifest = _request.assetBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");

				//Hash128 _hash = manifest.GetAssetBundleHash (manifest.GetAllAssetBundles()[0]);


				UnityEngine.Debug.Log (manifest);

				storeBundle.Value = _request.assetBundle;

				errorString.Value = string.Empty;	

				Fsm.Event(string.IsNullOrEmpty(errorString.Value) ? isDone : isError);

				Finish();
			}
		}

	}
}
