using System;
using UnityEngine;

public class RotationTweener : TweenBehavior, IRotationTweener
{
	public Vector3 axis;
	public float angle;

	public Quaternion GetOffset()
	{
		return Quaternion.AngleAxis(angle * Evaluate(), axis);
	}
}