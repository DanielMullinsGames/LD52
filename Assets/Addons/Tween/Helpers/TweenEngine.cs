/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Used internally by the tween system to run all tween calculations.
/// 
/// </summary>

using UnityEngine;
using System.Collections;
using Pixelplacement;

namespace Pixelplacement.TweenSystem
{
	public class TweenEngine : MonoBehaviour 
	{
		#region Public Methods
		public void ExecuteTween (TweenBase tween)
		{
			StartCoroutine (RunTween (tween));
		}
		#endregion

		#region Coroutines
		//execute tween:
		static IEnumerator RunTween (TweenBase tween)
		{
			Tween.activeTweens.Add (tween);

			while (true) 
			{
				//tick tween:
				if (!tween.Tick ())
				{
					//clean up tween:
					Tween.activeTweens.Remove (tween);
					yield break;
				}

				//loop:
				yield return null;	
			}
		}
		#endregion
	}
}