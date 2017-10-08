using System;
using UnityEngine;

[Serializable]
public class MaterialColorControllerData
{
	public Color damageColor;
	public Color baseColor;
	public TweenData damageColorTween;
}

public class MaterialColorController : GameBehavior
{
	public MaterialColorControllerData data;
	public GameObject visualsObject;

	public string materialColorProperty = "_Color";

	private Material material = null;

	private Tween damageColorTween;

	void Start()
	{
		if (visualsObject != null)
		{
			Renderer renderer = visualsObject.GetComponent<Renderer>();
			if (renderer != null)
			{
				material = renderer.material;
			}
		}

		damageColorTween = GameSystems.Instance.Tweens.CreateTween(data.damageColorTween);
		damageColorTween.Reset(0f);
	}

	void Update()
	{
		Color current = data.baseColor;

		if (damageColorTween.IsPlaying)
		{
			current = Color.LerpUnclamped(data.baseColor, data.damageColor, damageColorTween.Evaluate());
		}

		if (material != null)
		{
			material.SetColor(materialColorProperty, current);
		}
	}

	public void Damage(float damageTime)
	{
		damageColorTween.Reset(1f, TweenDirection.Forward, damageTime);
	}
}