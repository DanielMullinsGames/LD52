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
	class LocalPosition : TweenBase
	{
		#region Public Properties
		public Vector3 EndValue {get; private set;}
		#endregion

		#region Private Variables
		Transform _target;
		Vector3 _start;
		#endregion

		#region Constructor
		public LocalPosition (Transform target, Vector3 endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			//set essential properties:
			SetEssentials (Tween.TweenType.Position, target.GetInstanceID (), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);

			//catalog custom properties:
			_target = target;
			EndValue = endValue;
		}
		#endregion

		#region Processes
		protected override bool SetStartValue ()
		{
			if (_target == null) return false;
			_start = _target.localPosition;
			return true;
		}

		protected override void Operation (float percentage)
		{
			Vector3 calculatedValue = TweenUtilities.LinearInterpolate (_start, EndValue, percentage);
			_target.localPosition = calculatedValue;
		}
		#endregion

		#region Loops
		public override void Loop ()
		{
			ResetStartTime ();
			_target.localPosition = _start;
		}

		public override void PingPong ()
		{
			ResetStartTime ();
			_target.localPosition = EndValue;
			EndValue = _start;
			_start = _target.localPosition;
		}
		#endregion
	}
}