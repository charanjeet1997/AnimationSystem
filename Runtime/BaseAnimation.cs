using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace AnimationSystem
{
	public class BaseAnimation<T> : MonoBehaviour
	{
		#region PUBLIC_VARS

		public T animatableComponent;
		public UnityEvent onAnimationStart, onAnimationRunning, onAnimationEnd;
		public float duration;
		public AnimationCurve animationCurve;
		public bool loop;
		public bool playOnStart;
	    #endregion

	    #region PRIVATE_VARS

	    private float currentTime;
	    #endregion

	    #region UNITY_CALLBACKS

	    private void OnEnable()
	    {
		    if(playOnStart)
			    StartAnimate();
	    }

	    private void OnDisable()
	    {
		    if(playOnStart)
			    StopAnimate();
	    }

	    #endregion

	    #region PUBLIC_METHODS

	    public virtual void OnAnimationStart()
	    {
		    currentTime = 0;
		    onAnimationStart?.Invoke();
	    }

	    public virtual void OnAnimationRunning(float percentage)
	    {
		    onAnimationRunning?.Invoke();
	    }

	    public virtual void OnAnimationEnd()
	    {
		    onAnimationEnd?.Invoke();
	    }
	    [ContextMenu("Start Animation")]
	    public void StartAnimate()
	    {
		    OnAnimationStart();
		    StartCoroutine(RunAnimation());
	    }
	    [ContextMenu("Stop Animation")]
	    public void StopAnimate()
	    {
		    if(loop) OnAnimationEnd();
		    StopCoroutine(RunAnimation());
	    }
	    #endregion

	    #region PRIVATE_METHODS

	    IEnumerator RunAnimation()
	    {
		    while (currentTime < duration)
		    {
			    currentTime += Time.deltaTime;
			    float percentage = animationCurve.Evaluate(currentTime / duration);
			    OnAnimationRunning(percentage);
			    yield return null;
		    }
		    if (loop)
		    {
			    currentTime = 0;
			    StartCoroutine(RunAnimation());
		    }
		    OnAnimationEnd();
	    }
	    #endregion
	}
}
