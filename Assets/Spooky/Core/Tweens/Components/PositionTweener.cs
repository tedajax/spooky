using System;
using UnityEngine;

public class PositionTweener : TweenBehavior, IPositionTweener
{
	public Vector3 normal;
	public float magnitude;

	public Vector3 GetOffset()
	{
		return normal.normalized * magnitude * Evaluate();
	}
}