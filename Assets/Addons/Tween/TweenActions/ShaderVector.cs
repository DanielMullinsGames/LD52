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
	class ShaderVector : TweenBase
	{
		#region Public Properties
		public Vector4 EndValue {get; private set;}
		#endregion

		#region Private Variables
		Material _target;
		Vector4 _start;
		string _propertyName;
		#endregion
		
		#region Constructor
		public ShaderVector (Material target, string propertyName, Vector4 endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			//set essential properties:
			SetEssentials (Tween.TweenType.ShaderVector, target.GetInstanceID (), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			
			//catalog custom properties:
			_target = target;
			_propertyName = propertyName;
			EndValue = endValue;
		}
		#endregion
		
		#region Processes
		protected override bool SetStartValue ()
		{
			if (_target == null) return false;
			_start = _target.GetVector (_propertyName);
			return true;
		}

		protected override void Operation (float percentage)
		{
			Vector4 calculatedValue = TweenUtilities.LinearInterpolate (_start, EndValue, percentage);
			_target.SetVector (_propertyName, calculatedValue);
		}
		#endregion
		
		#region Loops
		public override void Loop ()
		{
			ResetStartTime ();
			_target.SetVector (_propertyName, _start);
		}
		
		public override void PingPong ()
		{
			ResetStartTime ();
			_target.SetVector (_propertyName, EndValue);
			EndValue = _start;
			_start = _target.GetVector (_propertyName);
		}
		#endregion
	}
}