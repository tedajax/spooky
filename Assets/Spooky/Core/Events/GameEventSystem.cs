using UnityEngine;
using System;
using System.Collections.Generic;

public class GameEventSystem
{
	Dictionary<Type, HashSet<Action<object>>> eventTypesToHandlers = new Dictionary<Type, HashSet<Action<object>>>();
	Dictionary<object, Dictionary<Type, Action<object>>> targetActions = new Dictionary<object, Dictionary<Type, Action<object>>>();

	Queue<GameEvent> messageQueue = new Queue<GameEvent>();

	public GameEventSystem()
	{

	}

	public void Flush()
	{
		while (messageQueue.Count > 0)
		{
			GameEvent message = messageQueue.Dequeue();
			Type eventType = message.GetType();
			if (eventTypesToHandlers.ContainsKey(eventType))
			{
				foreach (var handler in eventTypesToHandlers[eventType])
				{
					handler(message);
				}
			}
		}
	}

	public void Subscribe<TEvent>(object target, Action<TEvent> handler)
		where TEvent : GameEvent
	{
		Type eventType = typeof(TEvent);

		if (!eventTypesToHandlers.ContainsKey(eventType))
		{
			eventTypesToHandlers.Add(eventType, new HashSet<Action<object>>());
		}

		Action<object> action = o => handler((TEvent)o);

		if (!targetActions.ContainsKey(target))
		{
			targetActions.Add(target, new Dictionary<Type, Action<object>>());
		}

		if (targetActions[target].ContainsKey(typeof(TEvent)))
		{
			Debug.LogError(string.Format("Already have a registered handler for type '{0}' on object '{1}'.", typeof(TEvent), target));
			return;
		}

		targetActions[target].Add(typeof(TEvent), action);
		eventTypesToHandlers[eventType].Add(action);
	}

	public void Unsubscribe<TEvent>(object target)
		where TEvent : GameEvent
	{
		Type eventType = typeof(TEvent);

		if (!eventTypesToHandlers.ContainsKey(eventType))
		{
			return;
		}

		if (!targetActions.ContainsKey(target))
		{
			return;
		}

		if (!targetActions[target].ContainsKey(typeof(TEvent)))
		{
			return;
		}

		Action<object> action = targetActions[target][typeof(TEvent)];

		eventTypesToHandlers[eventType].Remove(action);
		targetActions[target].Remove(typeof(TEvent));

		if (targetActions[target].Count == 0)
		{
			targetActions.Remove(target);
		}
	}

	public void BroadcastDeferred<TEvent>(TEvent message, object sender)
		where TEvent : GameEvent
	{
		Type eventType = typeof(TEvent);

		if (!eventTypesToHandlers.ContainsKey(eventType))
		{
			return;
		}

		message.sender = sender;

		messageQueue.Enqueue(message);
	}

	public void Broadcast<TEvent>(TEvent message, object sender)
		where TEvent : GameEvent
	{
		Type eventType = typeof(TEvent);

		if (!eventTypesToHandlers.ContainsKey(eventType))
		{
			return;
		}

		message.sender = sender;

		var handlers = eventTypesToHandlers[eventType];
		foreach (var handler in handlers)
		{
			handler(message);
		}
	}
}
