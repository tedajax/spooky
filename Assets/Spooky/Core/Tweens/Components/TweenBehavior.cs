using UnityEngine;

public class TweenBehavior : MonoBehaviour
{
	public string tweenName = "";
	public Transform rootOverride = null;
	public bool resetWhenFinished = false;
	public TweenData tweenData;

	protected Transform targetTransform;
	protected Tween tween;
	protected TransformTweenRoot tweenRoot = null;

	void Awake()
	{
		targetTransform = (rootOverride == null) ? transform : rootOverride;

		tweenRoot = targetTransform.GetComponent<TransformTweenRoot>();
		if (tweenRoot == null)
		{
			tweenRoot = targetTransform.gameObject.AddComponent<TransformTweenRoot>();
		}

		tweenRoot.RegisterBehavior(this);

		tween = GameSystems.Instance.Tweens.CreateTween(tweenData);
		tween.name = tweenName;
	}

	void OnDestroy()
	{
		tweenRoot.UnregisterBehavior(this);
	}

	public void Play(float seconds)
	{
		tween.Reset(1f, TweenDirection.Forward, seconds);
	}

	public void SetTweenData(TweenData tweenData)
	{
		this.tweenData = tweenData;
	}

	protected float Evaluate()
	{
		if (resetWhenFinished)
		{
			if (!tween.IsPlaying)
			{
				return 0f;
			}
		}
		return tween.Evaluate();
	}
}