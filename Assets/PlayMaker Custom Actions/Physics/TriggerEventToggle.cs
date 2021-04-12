// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Detect trigger collisions between GameObjects that have RigidBody/Collider components and toggle a boolean when triggered.")]
	public class TriggerEventToggle : FsmStateAction
	{
        [Tooltip("The GameObject to detect trigger events on.")]
	    public FsmOwnerDefault gameObject;

	    [UIHint(UIHint.TagMenu)]
        [Tooltip("Filter by Tag.")]
		public FsmString collideTag;

        [Tooltip("Event to send if the trigger event is detected.")]
		public FsmEvent triggerEnterEvent;
		
		[Tooltip("Event to send when the trigger has exited.")]
		public FsmEvent triggerExitEvent;

		[Tooltip("true while an collider is triggering")]
		public FsmBool isTriggered;
		
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the GameObject that collided with the Owner of this FSM.")]
		public FsmGameObject storeCollider;

	    // cached proxy component for callbacks
	    private PlayMakerProxyBase cachedEnterProxy;
	    private PlayMakerProxyBase cachedExitProxy;
	    
		public override void Reset()
		{
		    gameObject = null;
			collideTag = "";
			triggerEnterEvent = null;
			triggerExitEvent = null;
			isTriggered = null;
			
			storeCollider = null;
		}

		public override void OnPreprocess()
		{
            if (gameObject == null) gameObject = new FsmOwnerDefault();
            
		    if (gameObject.OwnerOption == OwnerDefaultOption.UseOwner)
		    {
			    Fsm.HandleTriggerEnter = true;
			    Fsm.HandleTriggerExit = true;
		    }
		    else
		    {
		        // Add proxy components now if we can
		        GetProxyComponent();
		    }
		}

	    public override void OnEnter()
	    {
	        if (gameObject.OwnerOption == OwnerDefaultOption.UseOwner)
	            return;

	        if (cachedEnterProxy == null || cachedExitProxy == null)
	            GetProxyComponent();

	        AddCallback();

	        gameObject.GameObject.OnChange += UpdateCallback;
	    }

	    public override void OnExit()
	    {
	        if (gameObject.OwnerOption == OwnerDefaultOption.UseOwner)
	            return;

	        RemoveCallback();

	        gameObject.GameObject.OnChange -= UpdateCallback;
	    }

	    private void UpdateCallback()
	    {
	        RemoveCallback();
	        GetProxyComponent();
	        AddCallback();
	    }

	    private void GetProxyComponent()
	    {
		    cachedEnterProxy = null;
		    cachedExitProxy = null;
		    
	        var source = gameObject.GameObject.Value;
	        if (source == null)
	            return;

		    cachedEnterProxy = PlayMakerFSM.GetEventHandlerComponent<PlayMakerTriggerEnter>(source);
	     
		    cachedExitProxy = PlayMakerFSM.GetEventHandlerComponent<PlayMakerTriggerExit>(source);
	                
	        
	    }

	    private void AddCallback()
	    {
	        if (cachedEnterProxy != null)  cachedEnterProxy.AddTriggerEventCallback(TriggerEnter);
	        
	        if (cachedExitProxy != null)  cachedEnterProxy.AddTriggerEventCallback(TriggerExit);

	        
	    }

	    private void RemoveCallback()
	    {
		    if (cachedEnterProxy != null)  cachedEnterProxy.RemoveTriggerEventCallback(TriggerEnter);
	        
	        if (cachedExitProxy != null)  cachedEnterProxy.RemoveTriggerEventCallback(TriggerExit);

	    }

	    private void StoreCollisionInfo(Collider collisionInfo)
		{
			storeCollider.Value = collisionInfo.gameObject;
		}

		public override void DoTriggerEnter(Collider other)
		{
		    if (gameObject.OwnerOption == OwnerDefaultOption.UseOwner)
		        TriggerEnter(other);
		}
		

		public override void DoTriggerExit(Collider other)
		{
		    if (gameObject.OwnerOption == OwnerDefaultOption.UseOwner)
		        TriggerExit(other);
		}

	    private void TriggerEnter(Collider other)
	    {
		    if (TagMatches(collideTag, other))
		    {
			    isTriggered = true;
			    StoreCollisionInfo(other);
			    Fsm.Event(triggerEnterEvent);
		    }
	    }


	    private void TriggerExit(Collider other)
	    {
		    if (TagMatches(collideTag, other))
		    {
			    isTriggered = false;
			    StoreCollisionInfo(other);
			    Fsm.Event(triggerExitEvent);
		    }
	    }

		public override string ErrorCheck()
		{
			return ActionHelpers.CheckPhysicsSetup(Fsm.GetOwnerDefaultTarget(gameObject));
		}
	}
}