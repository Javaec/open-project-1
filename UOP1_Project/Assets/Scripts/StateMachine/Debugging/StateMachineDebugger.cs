#if UNITY_EDITOR

using System;
using System.Text;
using UnityEngine;

namespace UOP1.StateMachine.Debugging
{
	/// <summary>
	/// Class specialized in debugging the state transitions, should only be used while in editor mode.
	/// </summary>
	[Serializable]
	class StateMachineDebugger
	{
		[SerializeField] [Tooltip("Issues a debug log when a state transition is triggered")]
		internal bool debugTransitions = false;

		[SerializeField] [Tooltip("List all conditions evaluated, the result is read: ConditionName == BooleanResult [PassedTest]")]
		internal bool appendConditionsInfo = true;

		[SerializeField] [Tooltip("List all actions activated by the new State")]
		internal bool appendActionsInfo = true;

		[SerializeField] [Tooltip("The current State name [Readonly]")]
		internal string currentState;

		StateMachine _stateMachine;
		StringBuilder _logBuilder;
		string _targetState = string.Empty;

		const string CHECK_MARK = "\u2714";
		const string UNCHECK_MARK = "\u2718";
		const string THICK_ARROW = "\u279C";
		const string SHARP_ARROW = "\u27A4";

		/// <summary>
		/// Must be called together with <c>StateMachine.Awake()</c>
		/// </summary>
		internal void Awake(StateMachine stateMachine)
		{
			_stateMachine = stateMachine;
			_logBuilder = new StringBuilder();

			currentState = stateMachine._currentState._originSO.name;
		}

		internal void TransitionEvaluationBegin(string targetState)
		{
			_targetState = targetState;

			if (!debugTransitions)
			{
				return;
			}

			_logBuilder.Clear();
			_logBuilder.AppendLine($"{_stateMachine.name} state changed");
			_logBuilder.AppendLine($"{currentState}  {SHARP_ARROW}  {_targetState}");

			if (appendConditionsInfo)
			{
				_logBuilder.AppendLine();
				_logBuilder.AppendLine($"Transition Conditions:");
			}
		}

		internal void TransitionConditionResult(string conditionName, bool result, bool isMet)
		{
			if (!debugTransitions || _logBuilder.Length == 0 || !appendConditionsInfo)
			{
				return;
			}

			_logBuilder.Append($"    {THICK_ARROW} {conditionName} == {result}");

			if (isMet)
			{
				_logBuilder.AppendLine($" [{CHECK_MARK}]");
			}
			else
			{
				_logBuilder.AppendLine($" [{UNCHECK_MARK}]");
			}
		}

		internal void TransitionEvaluationEnd(bool passed, StateAction[] actions)
		{
			if (passed)
			{
				currentState = _targetState;
			}

			if (!debugTransitions || _logBuilder.Length == 0)
			{
				return;
			}

			if (passed)
			{
				LogActions(actions);
				PrintDebugLog();
			}

			_logBuilder.Clear();
		}

		void LogActions(StateAction[] actions)
		{
			if (!appendActionsInfo)
			{
				return;
			}

			_logBuilder.AppendLine();
			_logBuilder.AppendLine("State Actions:");

			foreach (StateAction action in actions)
			{
				_logBuilder.AppendLine($"    {THICK_ARROW} {action._originSO.name}");
			}
		}

		void PrintDebugLog()
		{
			_logBuilder.AppendLine();
			_logBuilder.Append("--------------------------------");

			Debug.Log(_logBuilder.ToString());
		}
	}
}

#endif
