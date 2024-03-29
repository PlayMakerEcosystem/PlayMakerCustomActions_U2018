﻿// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	/// <summary>
	/// Action version of Unity's builtin MouseLook behaviour.
	/// TODO: Expose invert Y option.
	/// </summary>
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Based on MouseLook, Rotates a GameObject based on mouse movement using Inputs. Minimum and Maximum values can be used to constrain the rotation.")]
	public class InputLook : FsmStateAction
	{
		public enum RotationAxes { InputXAndY = 0, InputX = 1, InputY = 2 }

		[RequiredField]
		[Tooltip("The GameObject to rotate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The axes to rotate around.")]
		public RotationAxes axes = RotationAxes.InputXAndY;

		[Tooltip("Joystick value that controls Input X")]
		public FsmString InputX;

		[Tooltip("Joystick value that controls Input Y")]
		public FsmString InputY;

		[RequiredField]
		[Tooltip("Sensitivity of movement in X direction.")]
		public FsmFloat sensitivityX;

		[RequiredField]
		[Tooltip("Sensitivity of movement in Y direction.")]
		public FsmFloat sensitivityY;

		[HasFloatSlider(-360,360)]
        [Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
		public FsmFloat minimumX;

		[HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
        public FsmFloat maximumX;

		[HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
        public FsmFloat minimumY;

		[HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
        public FsmFloat maximumY;


		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

	



		float rotationX;
		float rotationY;

		public override void Reset()
		{
			gameObject = null;
			axes = RotationAxes.InputXAndY;
			sensitivityX = 15f;
			sensitivityY = 15f;
			minimumX = new FsmFloat {UseVariable = true};
            maximumX = new FsmFloat { UseVariable = true };
			minimumY = -60f;
			maximumY = 60f;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				Finish();
				return;
			}

			// Make the rigid body not change rotation
			// TODO: Original Unity script had this. Expose as option?
		    var rigidbody = go.GetComponent<Rigidbody>();
            if (rigidbody != null)
			{
				rigidbody.freezeRotation = true;
			}

            // initialize rotation

		    rotationX = go.transform.localRotation.eulerAngles.y;
            rotationY = go.transform.localRotation.eulerAngles.x;

			DoMouseLook();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMouseLook();
		}

		void DoMouseLook()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			var transform = go.transform;

			switch (axes)
			{
				case RotationAxes.InputXAndY:
					
					transform.localEulerAngles = new Vector3(GetYRotation(), GetXRotation(), 0);
					break;
				
				case RotationAxes.InputX:

					transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, GetXRotation(), 0);
					break;

				case RotationAxes.InputY:

					transform.localEulerAngles = new Vector3(-GetYRotation(), transform.localEulerAngles.y, 0);
					break;
			}
		}
			


		float GetXRotation()
		{
			rotationX += Input.GetAxis(InputX.Value) * sensitivityX.Value;
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			return rotationX;
		}

		float GetYRotation()
		{
			rotationY += Input.GetAxis(InputY.Value) * sensitivityY.Value;
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
			return rotationY;
		}

		// Clamp function that respects IsNone
		static float ClampAngle(float angle, FsmFloat min, FsmFloat max)
		{
			if (!min.IsNone && angle < min.Value)
			{
				angle = min.Value;
			}

			if (!max.IsNone && angle > max.Value)
			{
				angle = max.Value;
			}
			
			return angle;
		}
	}
}