using System;
using UnityEngine;

public abstract class GameEvent
{
	public long timestamp;
	public object sender;

	public GameEvent()
	{
		timestamp = DateTime.UtcNow.Ticks;
	}
}
