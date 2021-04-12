// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.
// original action by Krileon: https://hutonggames.com/playmakerforum/index.php?topic=5819.msg28493#msg28493
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions {
	[ActionCategory(ActionCategory.StateMachine)]
	public class AddFsmTemplate : FsmStateAction {
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmTemplate template;
		public FsmString name;
		public FsmBool active;
		public FsmBool unique;
		public FsmBool replace;
		[CompoundArray("Variables", "Name", "Variable")]
		[RequiredField]
		public FsmString[] variableNames;
		[RequiredField]
		public FsmVar[] variables;
		[ObjectType(typeof(PlayMakerFSM)), UIHint(UIHint.Variable)]
		public FsmObject storeComponent;
		[ActionSection("Exists Event")]
		public FsmEventTarget existsEventTarget;
		public FsmString existsSendEvent;
		[ActionSection("Replace Event")]
		public FsmBool doNotDestroy;
		public FsmEventTarget replaceEventTarget;
		public FsmString replaceSendEvent;

		private GameObject previousGo;
		private List<PlayMakerFSM> fsms;
		
		public override void Reset() {
			gameObject = null;
			template = null;
			name = new FsmString { UseVariable = true };
			active = new FsmBool { Value = true };
			unique = new FsmBool { Value = false };
			replace = new FsmBool { Value = false };
			variableNames = new FsmString[0];
			variables = new FsmVar[0];
			storeComponent = null;
			existsEventTarget = null;
			existsSendEvent = null;
			doNotDestroy = new FsmBool { Value = false };
			replaceEventTarget = null;
			replaceSendEvent = null;
		}
		
		public override void OnEnter() {
			var go = Fsm.GetOwnerDefaultTarget( gameObject );
			
			if ( go == null ) {
				return;
			}
			
			bool exists = false;
			
			if ( ( ! unique.Value ) || replace.Value ) {
				if ( go != previousGo ) {
					fsms = new List<PlayMakerFSM>();
					
					fsms.AddRange( go.GetComponents<PlayMakerFSM>() );
					
					previousGo = go;
				}
				
				if ( fsms.Count > 0 ) foreach ( PlayMakerFSM fsm in fsms ) {
					if ( ( ( name.Value != "" ) && ( fsm.FsmName == name.Value ) ) || ( ( fsm.FsmTemplate != null ) && ( fsm.FsmTemplate.name == template.name ) ) ) {
						if ( replace.Value ) {
							if ( replaceSendEvent.Value != "" ) {
								Fsm.Event( replaceEventTarget, replaceSendEvent.Value );
							}

							if ( ! doNotDestroy.Value ) {
								Object.Destroy( fsm );
							}
						} else {
							storeComponent.Value = fsm;
							exists = true;
						}
					}
				}
			}
			
			if ( ! exists ) {
				PlayMakerFSM newFsm = go.AddComponent<PlayMakerFSM>();
				
				if ( name.Value != "" ) {
					newFsm.FsmName = name.Value;
				}
				
				if ( ( ! active.Value ) || ( variableNames.Length > 0 ) ) {
					newFsm.enabled = false;
				}
				
				newFsm.SetFsmTemplate( template );
				
				if ( variableNames.Length > 0 ) {
					if ( variableNames.Length > 0 ) for ( int i = 0; i < variableNames.Length; i++ ) {
						if ( ! variableNames[i].IsNone ) {
							NamedVariable target = newFsm.Fsm.Variables.GetVariable( variableNames[i].Value );
							
							if ( target != null ) {
								variables[i].ApplyValueTo( target );
							}
						}
					}
					
					if ( active.Value && ( ! newFsm.enabled ) ) {
						newFsm.enabled = true;
					}
				}

				if ( ( ! unique.Value ) || replace.Value ) {
					fsms.Add( newFsm );
				}

				storeComponent.Value = newFsm;
			} else {
				if ( existsSendEvent.Value != "" ) {
					Fsm.Event( existsEventTarget, existsSendEvent.Value );
				}
			}

			Finish();
		}
	}
}