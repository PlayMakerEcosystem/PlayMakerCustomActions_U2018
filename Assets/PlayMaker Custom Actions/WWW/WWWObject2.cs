// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// Keywords: download file server

using UnityEngine;

using System.IO;

#if UNITY_2018_3_OR_NEWER
using UnityEngine.Networking;
#endif

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory ("WWW")]
	[Tooltip ("Gets data from a url and store it in variables. See Unity WWW docs for more details. Works On IOS")]
	public class WWWObject2 : FsmStateAction
	{
		[RequiredField]
		[Tooltip ("Url to download data from.")]
		public FsmString url;

		[ActionSection ("Results")]

		[UIHint (UIHint.Variable)]
		[Tooltip ("Gets text from the url.")]
		public FsmString storeText;
		
		[UIHint (UIHint.Variable)]
		[Tooltip ("Gets a Texture from the url.")]
		public FsmTexture storeTexture;

		[UIHint (UIHint.Variable)]
		[Tooltip ("Save the content in a file")]
		public FsmString SaveInFile;
		
		#if ! UNITY_2018_3_OR_NEWER
		[UIHint (UIHint.Variable)]
		[ObjectType (typeof(MovieTexture))]
		[Tooltip ("Gets a Texture from the url.")]
		public FsmObject storeMovieTexture;
		#endif
		
		[UIHint (UIHint.Variable)]
		[Tooltip ("Error message if there was an error during the download.")]
		public FsmString errorString;

		[UIHint (UIHint.Variable)] 
		[Tooltip ("How far the download progressed (0-1).")]
		public FsmFloat progress;

		[ActionSection ("Events")] 
		
		[Tooltip ("Event to send when the data has finished loading (progress = 1).")]
		public FsmEvent isDone;
		
		[Tooltip ("Event to send if there was an error.")]
		public FsmEvent isError;

#if ! UNITY_2018_3_OR_NEWER
		private WWW wwwObject;
		#else
		private UnityWebRequest uwr;

		DownloadHandlerBuffer d;
#endif
		public override void Reset()
		{
			url = null;
			storeText = null;
			storeTexture = null;
			SaveInFile = null;
			errorString = null;
			progress = null;
			isDone = null;
		}

		public override void OnEnter()
		{
			if (string.IsNullOrEmpty (url.Value))
			{
				Finish ();
				return;
			}

#if UNITY_2018_3_OR_NEWER
			if (!storeTexture.IsNone)
			{
				uwr = UnityWebRequestTexture.GetTexture(url.Value);
			}else{
				uwr = new UnityWebRequest(url.Value);
				d = new DownloadHandlerBuffer();
				uwr.downloadHandler = d;
			}

			uwr.SendWebRequest();

#else
			wwwObject = new WWW (url.Value);
#endif
		}


#if UNITY_2018_3_OR_NEWER
		
		public override void OnUpdate()
		{
			if (uwr == null)
			{
				errorString.Value = "Unity Web Request is Null!";
				Finish();
				return;
			}

			errorString.Value = uwr.error;

			if (!string.IsNullOrEmpty(uwr.error))
			{
				Finish();
				Fsm.Event(isError);
				return;
			}

			progress.Value = uwr.downloadProgress;

			if (progress.Value.Equals(1f) && uwr.isDone)
			{
				if (!string.IsNullOrEmpty(SaveInFile.Value))
				{
					UnityEngine.Debug.Log("WWWOBJECT: Saving data in "+SaveInFile.Value );
					FileInfo file = new FileInfo(SaveInFile.Value); 
					file.Directory.Create();
					File.WriteAllBytes(SaveInFile.Value,uwr.downloadHandler.data);
				}
				
				if (!storeText.IsNone)
				{
					storeText.Value = uwr.downloadHandler.text;
				}

				if (!storeTexture.IsNone)
				{
					storeTexture.Value = ((DownloadHandlerTexture)uwr.downloadHandler).texture as Texture;
				}

				errorString.Value = uwr.error;

				Fsm.Event(string.IsNullOrEmpty(errorString.Value) ? isDone : isError);

				Finish();
			}
		}

#else

		public override void OnUpdate()
		{
			if (wwwObject == null)
			{
				errorString.Value = "WWW Object is Null!";
				Finish ();
				return;
			}

			errorString.Value = wwwObject.error;

			if (!string.IsNullOrEmpty (wwwObject.error))
			{
				Finish ();
				Fsm.Event (isError);
				return;
			}

			progress.Value = wwwObject.progress;

			if (progress.Value.Equals (1f) && wwwObject.isDone)
			{
				if (!string.IsNullOrEmpty(SaveInFile.Value))
				{
					UnityEngine.Debug.Log("WWWOBJECT: Saving data in "+SaveInFile.Value );
					File.WriteAllBytes(SaveInFile.Value,wwwObject.bytes);
				}

				storeText.Value = wwwObject.text;
				storeTexture.Value = wwwObject.texture;

#if UNITY_2018_2_OR_NEWER
				#if UNITY_5_6_OR_NEWER
                storeMovieTexture.Value = wwwObject.GetMovieTexture();
				#else
				storeMovieTexture.Value = wwwObject.movie;
				#endif
#endif
				errorString.Value = wwwObject.error;

				Fsm.Event (string.IsNullOrEmpty (errorString.Value) ? isDone : isError);

				Finish ();
			}
		}
		#endif
		
	}
}
