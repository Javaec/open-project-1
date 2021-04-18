﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace UOP1.StateMachine
{
	public class StateMachine : MonoBehaviour
	{
		[Tooltip("Set the initial state of this StateMachine")] [SerializeField]
		ScriptableObjects.TransitionTableSO _transitionTableSO = default;

#if UNITY_EDITOR
		[Space] [SerializeField] internal Debugging.StateMachineDebugger _debugger = default;
#endif

		readonly Dictionary<Type, Component> _cachedComponents = new Dictionary<Type, Component>();
		internal State _currentState;

		void Awake()
		{
			_currentState = _transitionTableSO.GetInitialState(this);
#if UNITY_EDITOR
			_debugger.Awake(this);
#endif
		}

#if UNITY_EDITOR
		void OnEnable()
		{
			UnityEditor.AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
		}

		void OnAfterAssemblyReload()
		{
			_currentState = _transitionTableSO.GetInitialState(this);
			_debugger.Awake(this);
		}

		void OnDisable()
		{
			UnityEditor.AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
		}
#endif

		void Start()
		{
			_currentState.OnStateEnter();
		}

		public new bool TryGetComponent<T>(out T component) where T : Component
		{
			Type type = typeof(T);
			if (!_cachedComponents.TryGetValue(type, out Component value))
			{
				if (base.TryGetComponent<T>(out component))
				{
					_cachedComponents.Add(type, component);
				}

				return component != null;
			}

			component = (T)value;
			return true;
		}

		public T GetOrAddComponent<T>() where T : Component
		{
			if (!TryGetComponent<T>(out T component))
			{
				component = gameObject.AddComponent<T>();
				_cachedComponents.Add(typeof(T), component);
			}

			return component;
		}

		public new T GetComponent<T>() where T : Component
		{
			return TryGetComponent(out T component)
				? component
				: throw new InvalidOperationException($"{typeof(T).Name} not found in {name}.");
		}

		void Update()
		{
			if (_currentState.TryGetTransition(out State transitionState))
			{
				Transition(transitionState);
			}

			_currentState.OnUpdate();
		}

		void Transition(State transitionState)
		{
			_currentState.OnStateExit();
			_currentState = transitionState;
			_currentState.OnStateEnter();
		}
	}
}
