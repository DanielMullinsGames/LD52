/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// </summary>

using UnityEngine;
using System;
using UnityEngine.UI;
using Pixelplacement;

namespace Pixelplacement.TweenSystem
{
	class TextColor : TweenBase
	{
		#region Public Properties
		public Color EndValue {get; private set;}
		#endregion

		#region Private Variables
		Text _target;
		Color _start;
		#endregion

		#region Constructor
		public TextColor (Text target, Color endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			//set essential properties:
			SetEssentials (Tween.TweenType.TextColor, target.GetInstanceID (), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);

			//catalog custom properties:
			_target = target;
			EndValue = endValue;
		}
		#endregion

		#region Processes
		protected override bool SetStartValue ()
		{
			if (_target == null) return false;
			_start = _target.color;
			return true;
		}

		protected override void Operation (float percentage)
		{
			Color calculatedValue = TweenUtilities.LinearInterpolate (_start, EndValue, percentage);
			_target.color = calculatedValue;
		}
		#endregion

		#region Loops
		public override void Loop ()
		{
			ResetStartTime ();
			_target.color = _start;
		}

		public override void PingPong ()
		{
			ResetStartTime ();
			_target.color = EndValue;
			EndValue = _start;
			_start = _target.color;
		}
		#endregion
	}
}