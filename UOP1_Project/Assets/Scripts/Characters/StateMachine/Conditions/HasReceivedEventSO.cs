﻿using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Has Received Event")]
public class HasReceivedEventSO : StateConditionSO<HasReceivedEventCondition>
{
	public VoidEventChannelSO voidEvent;
}

public class HasReceivedEventCondition : Condition
{
	HasReceivedEventSO _originSO
	{
		get
		{
			return (HasReceivedEventSO)OriginSO; // The SO this Condition spawned from
		}
	}

	bool _eventTriggered;

	public override void Awake(StateMachine stateMachine)
	{
		_eventTriggered = false;
		_originSO.voidEvent.OnEventRaised += EventReceived;
	}

	protected override bool Statement()
	{
		return _eventTriggered;
	}

	void EventReceived()
	{
		_eventTriggered = true;
	}

	public override void OnStateExit()
	{
		_eventTriggered = false;
	}
}
