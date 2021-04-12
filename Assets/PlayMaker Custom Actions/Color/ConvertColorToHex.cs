// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// Keywords: ToHtmlStringRGB ToHtmlStringRGBA Hexadecimal

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Convert a color to hexa (html)")]
	public class ConvertColorToHex : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The color")]
		public FsmColor color;

        [Tooltip("Include the alpha channel")]
        public bool withAlpha;

        [Tooltip("Adds # at the beginning")]
        public bool appendHashChar;

        [ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("The resulting hexa")]
		public FsmString result;
		
		[Tooltip("Repeat every frame")]
		public bool everyframe;

        string _hex;

		public override void Reset()
		{
			color = null;
            withAlpha = true;
            appendHashChar = true;
            result = null;

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
            if (withAlpha)
            {
                _hex = ColorUtility.ToHtmlStringRGBA(color.Value);
            }else{
                _hex = ColorUtility.ToHtmlStringRGB(color.Value);
            }

            if (appendHashChar)
            {
                _hex = "#" + _hex;
            }
            result.Value = _hex;

		}
		
	}
}
