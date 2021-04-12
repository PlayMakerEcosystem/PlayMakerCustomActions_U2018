// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// Keywords: download upload GET HEAD PUT PATCH DELETE file server
// source https://robots.thoughtbot.com/avoiding-out-of-memory-crashes-on-mobile


using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory ("WWW")]
	[Tooltip ("Gets data from a url and store it as text or texture or in a file.")]
	public class WebRequest : FsmStateAction
	{
		public enum Requests {GET,HEAD,POST,PUT,CREATE,PATCH,DELETE}
		
		[RequiredField]
		[Tooltip ("Url to download data from.")]
		public FsmString url;

		[Tooltip ("The type of request to make")]
		[ObjectType(typeof(Requests))]
		public FsmEnum request;

		[Tooltip ("The redirect limit for this request, leave to none for default")]
		public FsmInt redirectLimit;
		
		[Tooltip ("The headers data")]
		[CompoundArray("Headers", "Key", "Value")]
		public FsmString[] headerKeys;
		public FsmString[] headerValues;
		
		[Tooltip ("The post data, only use if request is set to POST or PUT")]
		[CompoundArray("PostData", "Key", "Value")]
		public FsmString[] postKeys;
		public FsmVar[] postValues;


		[ActionSection("Results")] 
		
		[UIHint (UIHint.Variable)]
		[Tooltip ("The response status code")]
		public FsmInt responseCode;
		
		[UIHint (UIHint.Variable)]
		[Tooltip ("Gets text from the url.")]
		public FsmString storeText;
		
		[UIHint (UIHint.Variable)]
		[Tooltip ("Gets a Texture from the url.")]
		public FsmTexture storeTexture;

		public FsmBool storedTextureAsReadable;
		

		[UIHint (UIHint.Variable)]
		[Tooltip ("Saves the content into a file as is")]
		public FsmString saveInFile;

		[ActionSection ("Feedback")]
		[UIHint (UIHint.Variable)]
		[Tooltip ("Error message if there was an error during the download.")]
		public FsmString errorString;

		[UIHint (UIHint.Variable)] 
		[Tooltip ("How far the download progressed (0-1).")]
		public FsmFloat progress;
		
		[UIHint (UIHint.Variable)] 
		[Tooltip ("If this flag is set to true it is canceled.")]
		public FsmBool cancel;
		
		public bool cancelOnExit;
		
		[ActionSection ("Events")] 
		
		[Tooltip ("Event to send when the data has finished loading (progress = 1).")]
		public FsmEvent isDone;
		
		[Tooltip ("Event to send if there was an error.")]
		public FsmEvent isError;


		private UnityWebRequest uwr;

		DownloadHandlerBuffer d;
		
		FileDownloadHandler f;

		private WWWForm _wwwForm;


		private UnityWebRequestAsyncOperation _asop;
		
		public override void Reset()
		{
			url = null;
			request = new FsmEnum();
			request.Value = Requests.GET;

			cancel = null;
			
			postKeys = new FsmString[0];
			postValues = new FsmVar[0];
			
			headerKeys = new FsmString[0];
			headerValues = new FsmString[0];

			responseCode = null;
			storeText = null;
			storeTexture = null;
			saveInFile = null;
			errorString = null;
			progress = null;
			isDone = null;
            isError = null;
		}

		public override void OnEnter()
		{
			
			bool _ok = false;
			
			_ok = CreateWebRequest();

			if (!_ok)
			{
				Finish();
				return;
			}
			
			_ok = handleHeader();
			
			if (!_ok)
			{
				Finish();
				return;
			}
				
			_ok = HandleWWWPost();
			
			if (!_ok)
			{
				Finish();
				return;
			}
				
			if (!storeTexture.IsNone)
			{
				uwr.downloadHandler = new DownloadHandlerTexture(storedTextureAsReadable.Value);

			}
			else if (!saveInFile.IsNone)
			{
                try
                {
                    f = new FileDownloadHandler(new byte[64 * 1024], saveInFile.Value);
                }catch(Exception e)
                {
                    errorString.Value = e.Message;
                    Fsm.Event(isError);
                    Finish();
                    return;
                }

				uwr.downloadHandler = f;
				
			}else {
				d = new DownloadHandlerBuffer();
				uwr.downloadHandler = d;
			}

			_asop = uwr.SendWebRequest();
			

		}

		bool CreateWebRequest()
		{
			if (string.IsNullOrEmpty (url.Value))
			{
				errorString.Value = "Url is empty";
				Fsm.Event(isError);
				return false;
			}

			
			UnityEngine.Debug.Log("CreateWebRequest "+ request.Value.ToString());

			if (cancel.Value == true)
			{
				errorString.Value = "Unity Web Request Cancelled because cancel property was true";
				Fsm.Event(isError);
				return false;
			}

			uwr = new UnityWebRequest(url.Value, request.Value.ToString());

			if (uwr ==null)
			{
				errorString.Value = "Unity Web request creation failed";
				Fsm.Event(isError);
				return false;
			}

			if (!redirectLimit.IsNone)
			{
				uwr.redirectLimit = redirectLimit.Value;
			}
			
			return true;
		}

		bool handleHeader()
		{
			int i = 0;
			foreach(FsmString _Fsmkey in headerKeys)
			{
				uwr.SetRequestHeader(_Fsmkey.Value, headerValues[i].Value);
				i++;
			}

			return true;
		}
		
		bool HandleWWWPost()
		{
			_wwwForm = new WWWForm();
			 int i = 0;
				
			foreach(FsmString _Fsmkey in postKeys)
			{
				string _key = _Fsmkey.Value;
					
				switch (postValues[i].Type)
				{
					case VariableType.Material:
					case VariableType.Unknown:
					case VariableType.Object:
						//not supported;
						break;
					case VariableType.Texture:
						
						Texture2D rt = (Texture2D)postValues[i].textureValue;
						
						_wwwForm.AddBinaryData(_key,rt.EncodeToPNG());
						break;
					default:
						_wwwForm.AddField(_key,postValues[i].ToString());
						break;
				}
					
					
				i++;
			}
			
			
			byte[] data = null;
			if (_wwwForm != null)
			{
				data = _wwwForm.data;
				if (data.Length == 0)
					data = null;
			}
			
			uwr.uploadHandler = new UploadHandlerRaw(data);
			
			if (_wwwForm != null)
			{
				foreach (KeyValuePair<string, string> header in _wwwForm.headers)
					uwr.SetRequestHeader(header.Key, header.Value);
			}
			
			return true;
		}

	
		public override void OnUpdate()
		{
			if (uwr == null)
			{
				errorString.Value = "Unity Web Request is Null!";
                Fsm.Event(isError);
                Finish();
				return;
			}

			responseCode.Value = (int)uwr.responseCode;
			
	
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

				if (uwr.downloadHandler != null)
				{
					uwr.downloadHandler.Dispose ();
				}

				Finish();
			}
		}

        public override string ErrorCheck()
        {
            if (storeText.IsNone && storeTexture.IsNone && saveInFile.IsNone)
            {
                return "Setup at least storeText, storeTexture or saveInFile property to save the data";
            }

            if (storeText.IsNone && ( !storeTexture.IsNone && !saveInFile.IsNone))
            {
                return "You can only select one type of result";
            }

            if (storeTexture.IsNone && (!storeText.IsNone && !saveInFile.IsNone))
            {
                return "You can only select one type of result";
            }

            if (saveInFile.IsNone && (!storeTexture.IsNone && !storeText.IsNone))
            {
                return "You can only select one type of result";
            }

            return string.Empty;
        }

    }

	public class FileDownloadHandler : DownloadHandlerScript
	{
		private int expected = -1;
		private int received = 0;
		private string filepath;
		private FileStream fileStream;
		private bool canceled = false;

		public FileDownloadHandler(byte[] buffer, string filepath): base(buffer)
		{
			this.filepath = filepath;
			fileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
		}

		protected override byte[] GetData() { return null; }

		protected override bool ReceiveData(byte[] data, int dataLength)
		{
			if (data == null || data.Length < 1)
			{
				return false;
			}
			received += dataLength;
			if (!canceled) fileStream.Write(data, 0, dataLength);
			return true;
		}

		protected override float GetProgress()
		{
			if (expected < 0) return 0;
			return (float)received / expected;
		}

		protected override void CompleteContent()
		{
			fileStream.Close();
		}

		protected override void ReceiveContentLength(int contentLength)
		{
			expected = contentLength;
		}

		public void Cancel()
		{
			canceled = true;
			fileStream.Close();
			File.Delete(filepath);
		}
	}


}

