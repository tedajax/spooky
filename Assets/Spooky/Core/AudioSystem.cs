using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
	private GameEventSystem events;

	private AudioSource source;

	void Awake()
	{
		events = GameSystems.Instance.Events;

		source = GetComponent<AudioSource>();

		events.Subscribe<PlayAudioEvent>(this, onPlayAudioEvent);
	}

	private void onPlayAudioEvent(PlayAudioEvent message)
	{
		AudioSource source = this.source;
		if (message.sourceOverride != null)
		{
			source = message.sourceOverride;
		}

		source.clip = message.audioClip;
		source.PlayScheduled(message.delay);
	}
}