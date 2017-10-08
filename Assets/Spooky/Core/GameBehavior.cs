using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEventReceiver
{
	void SendEvent<TEvent>(TEvent message, object sender = null) where TEvent : GameEvent;
}

public interface IGameEventNotifier
{
	void Subscribe<TEvent>(IGameEventReceiver receiver) where TEvent : GameEvent;
	void Unsubscribe<TEvent>(IGameEventReceiver receiver) where TEvent : GameEvent;
	void BroadcastEvent<TEvent>(TEvent message) where TEvent : GameEvent;
}

[RequireComponent(typeof(GameBehaviorHub))]
public class GameBehavior : MonoBehaviour, IGameEventReceiver, IGameEventNotifier
{
	Dictionary<Type, Action<object>> eventHandlers = new Dictionary<Type, Action<object>>();
	Dictionary<Type, HashSet<IGameEventReceiver>> eventReceivers = new Dictionary<Type, HashSet<IGameEventReceiver>>();

	public GameSystems Game { get { return GameSystems.Instance; } }
	public GameEventSystem Events { get { return Game.Events; } }

	protected GameBehaviorHub Hub;

	public virtual void Awake()
	{
		if (Hub == null)
		{
			Hub = GetComponent<GameBehaviorHub>();
		}

		Hub.Subscribe(this);
	}

	public virtual void OnDestroy()
	{
		Hub.Unsubscribe(this);
	}

	protected void RegisterHandler<TEvent>(Action<TEvent> handler) where TEvent : GameEvent
	{
		eventHandlers[typeof(TEvent)] = o => handler((TEvent)o);
	}

	protected void UnregisterHandler<TEvent>() where TEvent : GameEvent
	{
		if (eventHandlers.ContainsKey(typeof(TEvent)))
		{
			eventHandlers[typeof(TEvent)] = null;
		}
	}

	public void SendEvent<TEvent>(TEvent message, object sender = null) where TEvent : GameEvent
	{
		if (eventHandlers.ContainsKey(typeof(TEvent)))
		{
			if (message.sender == null)
			{
				message.sender = sender;
			}
			eventHandlers[typeof(TEvent)](message);
		}
	}

	public void Subscribe<TEvent>(IGameEventReceiver receiver) where TEvent : GameEvent
	{
		Type t = typeof(TEvent);
		if (!eventReceivers.ContainsKey(t))
		{
			eventReceivers[t] = new HashSet<IGameEventReceiver>();
		}

		eventReceivers[t].Add(receiver);
	}

	public void Unsubscribe<TEvent>(IGameEventReceiver receiver) where TEvent : GameEvent
	{
		Type t = typeof(TEvent);
		if (eventReceivers.ContainsKey(t))
		{
			eventReceivers[t].Remove(receiver);
		}
	}

	public void BroadcastEvent<TEvent>(TEvent message) where TEvent : GameEvent
	{
		if (eventReceivers.ContainsKey(typeof(TEvent)))
		{
			message.sender = this;

			HashSet<IGameEventReceiver> receivers = eventReceivers[typeof(TEvent)];
			foreach (var receiver in receivers)
			{
				receiver.SendEvent(message);
			}
		}
	}

	protected void SubscribeHandler<TEvent>(IGameEventNotifier notifier, Action<TEvent> handler) where TEvent : GameEvent
	{
		notifier.Subscribe<TEvent>(this);
		RegisterHandler(handler);
	} 

}