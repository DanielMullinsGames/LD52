using UnityEngine;
using System.Collections;

public class CustomCoroutine : MonoBehaviour
{
    public static CustomCoroutine Instance { get { TryCreateInstance();  return instance; } }
    private static CustomCoroutine instance;

    #region Common

    private static void TryCreateInstance()
    {
        if (instance == null)
        {
            instance = (new GameObject("CustomCoroutineRunner")).AddComponent<CustomCoroutine>();
        }
    }

    public static Coroutine FlickerSequence(System.Action flickerOn, System.Action flickerOff, bool startOn, bool endOn, float interval, int iterations, System.Action completeCallback = null)
    {
        TryCreateInstance();
        return instance.StartFlickerSequence(flickerOn, flickerOff, startOn, endOn, interval, iterations, completeCallback);
    }

    public Coroutine StartFlickerSequence(System.Action flickerOn, System.Action flickerOff, bool startOn, bool endOn, float interval, int iterations, System.Action completeCallback = null)
    {
        return StartCoroutine(DoFlickerSequence(flickerOn, flickerOff, startOn, endOn, interval, iterations, completeCallback));
    }

    IEnumerator DoFlickerSequence(System.Action flickerOn, System.Action flickerOff, bool startOn, bool endOn, float interval, int iterations, System.Action completeCallback = null)
    {
        var firstAction = startOn ? flickerOn : flickerOff;
        var secondAction = startOn ? flickerOff : flickerOn;

        for (int i = 0; i < iterations; i++)
        {
            firstAction?.Invoke();
            yield return new WaitForSeconds(interval);
            secondAction?.Invoke();
            yield return new WaitForSeconds(interval);
        }

        if (endOn)
        {
            flickerOn?.Invoke();
        }
        else
        {
            flickerOff?.Invoke();
        }

        completeCallback?.Invoke();
    }

    #endregion

    #region WaitOnConditionThenExecute
    public static void WaitOnConditionThenExecute(System.Func<bool> condition, System.Action action)
	{
		TryCreateInstance();
		instance.StartWaitOnConditionThenExecute(condition, action);
	}

	public void StartWaitOnConditionThenExecute(System.Func<bool> condition, System.Action action)
	{
		StartCoroutine(DoWaitOnConditionThenExecute(condition, action));
	}

	IEnumerator DoWaitOnConditionThenExecute(System.Func<bool> condition, System.Action action)
	{
		yield return new WaitUntil (() => condition() == true);
		action();
	}
	#endregion

    #region WaitThenExecute
	public static void WaitThenExecute(float wait, System.Action action, bool unscaledTime = false)
    {
        TryCreateInstance();
		instance.StartWaitThenExecute(wait, action, unscaledTime);
    }

	public void StartWaitThenExecute(float wait, System.Action action, bool unscaledTime = false)
    {
		StartCoroutine(DoWaitThenExecute(wait, action, unscaledTime));
    }

	IEnumerator DoWaitThenExecute(float wait, System.Action action, bool unscaledTime = false)
    {
        if (wait <= 0f)
        {
            yield return new WaitForEndOfFrame();
        }
        else
        {
			if (unscaledTime)
			{
				yield return new WaitForSecondsRealtime (wait);
			}
			else
			{
				yield return new WaitForSeconds (wait);
			}
        }
        action();
    }
    #endregion

    #region WaitOnCondition
    public static Coroutine WaitOnCondition(System.Func<bool> condition)
    {
        TryCreateInstance();
        return instance.StartWaitOnCondition(condition);
    }

    public Coroutine StartWaitOnCondition(System.Func<bool> condition)
    {
        return StartCoroutine(DoWaitOnCondition(condition));
    }

    IEnumerator DoWaitOnCondition(System.Func<bool> condition)
    {
        while (condition())
        {
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
