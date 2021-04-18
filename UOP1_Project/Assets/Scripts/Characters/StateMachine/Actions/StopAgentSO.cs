﻿using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "StopAgent", menuName = "State Machines/Actions/Stop NavMesh Agent")]
public class StopAgentSO : StateActionSO
{
	protected override StateAction CreateAction()
	{
		return new StopAgent();
	}
}

public class StopAgent : StateAction
{
	NavMeshAgent _agent;
	bool _agentDefined;

	public override void Awake(StateMachine stateMachine)
	{
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
		_agentDefined = _agent != null;
	}

	public override void OnUpdate()
	{
	}

	public override void OnStateEnter()
	{
		if (_agentDefined)
		{
			_agent.isStopped = true;
		}
	}
}
