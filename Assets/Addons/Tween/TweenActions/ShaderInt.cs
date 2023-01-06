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
	class ShaderInt : TweenBase
	{
		#region Public Properties
		public int EndValue {get; private set;}
		#endregion

		#region Private Variables
		Material _target;
		int _start;
		string _propertyName;
		#endregion
		
		#region Constructor
		public ShaderInt (Material target, string propertyName, int endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			//set essential properties:
			SetEssentials (Tween.TweenType.ShaderInt, target.GetInstanceID (), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			
			//catalog custom properties:
			_target = target;
			_propertyName = propertyName;
			EndValue = endValue;
		}
		#endregion
		
		#region Processes
		protected override bool SetStartValue ()
		{
			_start = _target.GetInt (_propertyName);
			if (_target == null) return false;
			return true;
		}

		protected override void Operation (float percentage)
		{
			float calculatedValue = TweenUtilities.LinearInterpolate (_start, EndValue, percentage);
			_target.SetInt (_propertyName, (int)calculatedValue);
		}
		#endregion
		
		#region Loops
		public override void Loop ()
		{
			ResetStartTime ();
			_target.SetInt (_propertyName, _start);
		}
		
		public override void PingPong ()
		{
			ResetStartTime ();
			_target.SetInt (_propertyName, EndValue);
			EndValue = _start;
			_start = _target.GetInt (_propertyName);
		}
		#endregion
	}
}