// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// Keywords: ToHtmlStringRGB ToHtmlStringRGBA Hexadecimal

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Convert a hexadecimal string to color")]
	public class ConvertHexToColor : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The hexadecimal color")]
        public FsmString hexaString;

        [Tooltip("Adds # at the beginning if necessary")]
        public bool appendHashChar;

        [ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("The resulting color")]
        public FsmColor color;

        public FsmBool success;
        public FsmEvent successEvent;
        public FsmEvent failureEvent;


        [Tooltip("Repeat every frame")]
		public bool everyframe;


        Color _c;
        string _s;

		public override void Reset()
		{
            hexaString = null;
            color = null;
            success = null;
            appendHashChar = true;
        successEvent = null;
            failureEvent = null;
            everyframe = false;
		}
		
		public override void OnEnter()
		{
			
			Execute();
			
			if (!everyframe)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
            Execute();
		}
		
		private void Execute()
		{
            _s = hexaString.Value;

            if (!_s.StartsWith("#",System.StringComparison.InvariantCulture) && appendHashChar)
            {
                _s = "#" + _s;
            }

            bool _ok =   ColorUtility.TryParseHtmlString(_s, out _c);

            if (_ok)
            {
                color.Value = _c;
            }

            if (!success.IsNone)
            {
                success.Value = _ok;
            }

            if (successEvent != null && _ok)
            {
                Fsm.Event(successEvent);
            }

            if (failureEvent != null && !_ok)
            {
                Fsm.Event(successEvent);
            }
        }
		
	}
}
