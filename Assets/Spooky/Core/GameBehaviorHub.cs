using UnityEngine;
using System.Collections.Generic;
using System;

public class GameBehaviorHub : MonoBehaviour, IGameEventReceiver
{
	private HashSet<IGameEventReceiver> receivers = new HashSet<IGameEventReceiver>();

	void Awake()
	{
	}

	public void Subscribe(IGameEventReceiver receiver)
	{
		if (receiver == this as IGameEventReceiver)
		{
			return;
		}
		receivers.Add(receiver);
	}

	public void Unsubscribe(IGameEventReceiver receiver)
	{
		if (receiver == this as IGameEventReceiver)
		{
			return;
		}
		receivers.Remove(receiver);
	}

	public void SendEvent<TEvent>(TEvent message, object sender = null) where TEvent : GameEvent
	{
		foreach (var receiver in receivers)
		{
			//						this is for a weird interface related unity thing
			if (receiver != null && !receiver.Equals(null))
			{
				receiver.SendEvent(message, sender);
			}
		}
	}
}