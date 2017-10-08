using System;
using UnityEngine;

public class ScaleTweener : TweenBehavior, IScaleTweener
{
	public Vector3 axis;
	public float magnitude;

	public Vector3 GetOffset()
	{
		return axis * magnitude * Evaluate();
	}
}