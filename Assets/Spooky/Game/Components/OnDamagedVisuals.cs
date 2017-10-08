using UnityEngine;
using System.Collections.Generic;

public class OnDamagedVisuals : GameBehavior
{
	public MaterialColorController materialController;
	public List<TweenBehavior> damageTweens = new List<TweenBehavior>();

	public override void Awake()
	{
		base.Awake();
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}
}
