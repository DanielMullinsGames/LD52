/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com 
/// </summary>

using UnityEngine;
using System;
using Pixelplacement;

namespace Pixelplacement.TweenSystem
{
	class ValueVector3 : TweenBase
	{
		#region Public Properties
		public Vector3 EndValue {get; private set;}
		#endregion

		#region Private Variables
		Action<Vector3> _valueUpdatedCallback;
		Vector3 _start;
		#endregion

		#region Constructor
		public ValueVector3 (Vector3 startValue, Vector3 endValue, Action<Vector3> valueUpdatedCallback, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			//set essential properties:
			SetEssentials (Tween.TweenType.Value, -1, duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);

			//catalog custom properties:
			_valueUpdatedCallback = valueUpdatedCallback;
			_start = startValue;
			EndValue = endValue;
		}
		#endregion

		#region Processes
		protected override bool SetStartValue ()
		{
			return true;
		}

		protected override void Operation (float percentage)
		{
			Vector3 calculatedValue = TweenUtilities.LinearInterpolate (_start, EndValue, percentage);
			_valueUpdatedCallback (calculatedValue);
		}
		#endregion

		#region Loops
		public override void Loop ()
		{
			ResetStartTime ();
		}

		public override void PingPong ()
		{
			ResetStartTime ();
			Vector3 temp = _start;
			_start = EndValue;
			EndValue = temp;
		}
		#endregion
	}
}