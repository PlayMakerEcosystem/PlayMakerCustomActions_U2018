// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// keywords: copy material paste

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Copy a material properties onto another.")]
	public class CopyMaterialProperties : FsmStateAction
	{
		[RequiredField]
		public FsmMaterial source;

		[RequiredField]
		public FsmMaterial recipient;
		
		public override void Reset()
		{
			source = null;
			recipient = null;
		}

		public override void OnEnter()
		{
			CopyMaterial();
			
			Finish();
		}

		void CopyMaterial()
		{
			if (recipient.Value != null && source.Value != null)
			{
				recipient.Value.CopyPropertiesFromMaterial(source.Value);
			}
		}
		
	}
}