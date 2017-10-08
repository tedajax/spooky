using System;
using UnityEngine;

public class PlayAudioEvent : GameEvent
{
	public AudioClip audioClip = null;
	public AudioSource sourceOverride = null;
	public float volume = 1f;
	public float delay = 0f;
}

public class DamageEvent : GameEvent
{
	public float damage;
}

