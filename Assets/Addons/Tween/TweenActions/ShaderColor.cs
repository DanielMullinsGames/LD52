/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// </summary>

using UnityEngine;
using System;
using Pixelplacement;
using System.Collections.Generic;

namespace Pixelplacement.TweenSystem
{
	class ShaderColor : TweenBase
	{
		#region Public Properties
		public Color EndValue {get; private set;}
		#endregion

		#region Private Variables
		Material _target;
		Color _start;
		string _propertyName;

        List<Renderer> _rendererTargets;
        MaterialPropertyBlock _propertyBlock;
		#endregion
		
		#region Constructor
		public ShaderColor (Material target, string propertyName, Color endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			//set essential properties:
			SetEssentials (Tween.TweenType.ShaderColor, target.GetInstanceID (), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			
			//catalog custom properties:
			_target = target;
			_propertyName = propertyName;
			EndValue = endValue;
		}

        public ShaderColor (List<Renderer> renderers, MaterialPropertyBlock propertyBlock, string propertyName, Color endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
        {
            //set essential properties:
            SetEssentials(Tween.TweenType.ShaderColor, renderers[0].GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);

            //catalog custom properties:
            _rendererTargets = renderers;
            _propertyBlock = propertyBlock;
            _propertyName = propertyName;
            EndValue = endValue;
        }
        #endregion

        #region Processes
        protected override bool SetStartValue ()
		{
            _start = GetTargetColor();
			if (_target == null && _propertyBlock == null) return false;
			return true;
		}

		protected override void Operation (float percentage)
		{
			Color calculatedValue = TweenUtilities.LinearInterpolate (_start, EndValue, percentage);
			SetTargetColor(calculatedValue);
		}
		#endregion
		
		#region Loops
		public override void Loop ()
		{
			ResetStartTime ();
            SetTargetColor(_start);
        }
		
		public override void PingPong ()
		{
			ResetStartTime ();
            SetTargetColor(EndValue);
			EndValue = _start;
            _start = GetTargetColor();
		}
		#endregion

        private Color GetTargetColor()
        {
            if (_propertyBlock != null)
            {
                _rendererTargets[0].GetPropertyBlock(_propertyBlock);
                return _propertyBlock.GetColor(_propertyName);
            }
            else
            {
                return _target.GetColor(_propertyName);
            }
        }

        private void SetTargetColor(Color color)
        {
            if (_propertyBlock != null)
            {
                _rendererTargets[0].GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetColor(_propertyName, color);
                _rendererTargets.ForEach(x => x.SetPropertyBlock(_propertyBlock));
            }
            else
            {
                _target.SetColor(_propertyName, color);
            }
        }
	}
}