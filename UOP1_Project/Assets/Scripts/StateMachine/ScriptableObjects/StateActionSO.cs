﻿using System.Collections.Generic;
using UnityEngine;

namespace UOP1.StateMachine.ScriptableObjects
{
	public abstract class StateActionSO : ScriptableObject
	{
		/// <summary>
		/// Will create a new custom <see cref="StateAction"/> or return an existing one inside <paramref name="createdInstances"/>
		/// </summary>
		internal StateAction GetAction(StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstances)
		{
			if (createdInstances.TryGetValue(this, out object obj))
			{
				return (StateAction)obj;
			}

			StateAction action = CreateAction();
			createdInstances.Add(this, action);
			action._originSO = this;
			action.Awake(stateMachine);
			return action;
		}

		protected abstract StateAction CreateAction();
	}

	public abstract class StateActionSO<T> : StateActionSO where T : StateAction, new()
	{
		protected override StateAction CreateAction()
		{
			return new T();
		}
	}
}
