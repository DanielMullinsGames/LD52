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
	class PanStereo : TweenBase
	{
		#region Public Properties
		public float EndValue {get; private set;}
		#endregion

		#region Private Variables
		AudioSource _target;
		float _start;
		#endregion

		#region Constructor
		public PanStereo (AudioSource target, float endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			//set essential properties:
			SetEssentials (Tween.TweenType.PanStereo, target.GetInstanceID (), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);

			//catalog custom properties:
			_target = target;
			EndValue = endValue;
		}
		#endregion

		#region Processes
		protected override bool SetStartValue ()
		{
			if (_target == null) return false;
			_start = _target.panStereo;
			return true;
		}

		protected override void Operation (float percentage)
		{
			float calculatedValue = TweenUtilities.LinearInterpolate (_start, EndValue, percentage);
			_target.panStereo = calculatedValue;
		}
		#endregion

		#region Loops
		public override void Loop ()
		{
			ResetStartTime ();
			_target.panStereo = _start;
		}

		public override void PingPong ()
		{
			ResetStartTime ();
			_target.panStereo = EndValue;
			EndValue = _start;
			_start = _target.panStereo;
		}
		#endregion
	}
}