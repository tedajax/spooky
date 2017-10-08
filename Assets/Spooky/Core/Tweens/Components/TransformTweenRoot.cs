using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPositionTweener
{
	Vector3 GetOffset();
}

public interface IRotationTweener
{
	Quaternion GetOffset();
}

public interface IScaleTweener
{
	Vector3 GetOffset();
}

public class TransformTweenRoot : MonoBehaviour
{
	private Vector3 basePosition;
	private Quaternion baseRotation;
	private Vector3 baseScale;

	private List<IPositionTweener> positionTweeners = new List<IPositionTweener>();
	private List<IRotationTweener> rotationTweeners = new List<IRotationTweener>();
	private List<IScaleTweener> scaleTweeners = new List<IScaleTweener>();

	private Dictionary<string, TweenBehavior> namedTweens = new Dictionary<string, TweenBehavior>();

	void Awake()
	{
		basePosition = transform.localPosition;
		baseRotation = transform.localRotation;
		baseScale = transform.localScale;
	}

	public void RegisterBehavior(TweenBehavior behavior)
	{
		if (behavior is IPositionTweener)
		{
			positionTweeners.Add(behavior as IPositionTweener);
		}
		
		if (behavior is IRotationTweener)
		{
			rotationTweeners.Add(behavior as IRotationTweener);
		}

		if (behavior is IScaleTweener)
		{
			scaleTweeners.Add(behavior as IScaleTweener);
		}

		if (!string.IsNullOrEmpty(behavior.tweenName))
		{
			if (namedTweens.ContainsKey(behavior.tweenName))
			{
				Debug.LogWarningFormat("Already have a tween named {0}", behavior.tweenName);
			}
			else
			{
				namedTweens.Add(behavior.tweenName, behavior);
			}
		}
	}

	public void UnregisterBehavior(TweenBehavior behavior)
	{
		if (behavior is IPositionTweener)
		{
			var p = behavior as IPositionTweener;
			if (positionTweeners.Contains(p))
			{
				positionTweeners.Remove(p);
			}
		}

		if (behavior is IRotationTweener)
		{
			var r = behavior as IRotationTweener;
			if (rotationTweeners.Contains(r))
			{
				rotationTweeners.Remove(r);
			}
		}

		if (behavior is IScaleTweener)
		{
			var s = behavior as IScaleTweener;
			if (scaleTweeners.Contains(s))
			{
				scaleTweeners.Remove(s);
			}
		}

		if (!string.IsNullOrEmpty(behavior.tweenName))
		{
			if (namedTweens.ContainsKey(behavior.tweenName))
			{
				namedTweens.Remove(behavior.tweenName);
			}
		}
	}

	void Update()
	{
		{
			Vector3 position = basePosition;
			foreach (var pt in positionTweeners)
			{
				position += pt.GetOffset();
			}
			transform.localPosition = position;
		}

		{
			Quaternion rotation = baseRotation;
			foreach (var rt in rotationTweeners)
			{
				rotation *= rt.GetOffset();
			}
			transform.localRotation = rotation;
		}

		{
			Vector3 scale = baseScale;
			foreach (var st in scaleTweeners)
			{
				scale += st.GetOffset();
			}
			transform.localScale = scale;
		}
	}

	public TweenBehavior GetNamedTweenBehavior(string name)
	{
		if (namedTweens.ContainsKey(name))
		{
			return namedTweens[name];
		}
		return null;
	}
}