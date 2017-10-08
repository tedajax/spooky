using System;
using UnityEngine;

[Serializable]
public class GameConfigData
{
}

public class GameSystems : MonoBehaviour
{
	public static GameSystems Instance { get; private set; }

	public GameConfigData Config;

	public TweenSystem Tweens { get; private set; }
	public GameEventSystem Events { get; private set; }
	public AudioSystem Audio { get; private set; }

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		Tweens = new TweenSystem();
		Events = new GameEventSystem();

		Audio = GetComponent<AudioSystem>() ?? gameObject.AddComponent<AudioSystem>();
	}

	void Update()
	{
		Tweens.Update(Time.deltaTime);
	}

	void LateUpdate()
	{
		Events.Flush();
	}
}