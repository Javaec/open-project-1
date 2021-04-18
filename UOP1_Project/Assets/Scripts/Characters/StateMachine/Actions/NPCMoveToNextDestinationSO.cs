﻿using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "NPCMoveToNextDestination", menuName = "State Machines/Actions/NPC Move To Next Destination")]
public class NPCMoveToNextDestinationSO : StateActionSO
{
	protected override StateAction CreateAction()
	{
		return new NPCMoveToNextDestination();
	}
}

public class NPCMoveToNextDestination : StateAction
{
	NPCMovement _npcMovement;
	NPCMovementConfigSO _config;
	NPCMovementAction _action;
	NavMeshAgent _agent;

	public override void Awake(StateMachine stateMachine)
	{
		_agent = stateMachine.GetComponent<NavMeshAgent>();
		_npcMovement = stateMachine.GetComponent<NPCMovement>();
		InitMovementStrategy(_npcMovement.NPCMovementConfig);
	}

	public override void OnStateEnter()
	{
		if (_config != _npcMovement.NPCMovementConfig)
		{
			InitMovementStrategy(_npcMovement.NPCMovementConfig);
		}

		_action.OnStateEnter();
	}

	public override void OnUpdate()
	{
		_action.OnUpdate();
	}

	public override void OnStateExit()
	{
		_action.OnStateExit();
	}

	void InitMovementStrategy(NPCMovementConfigSO config)
	{
		_config = config;
		if (_npcMovement.NPCMovementConfig is RoamingAroundCenterConfigSO)
		{
			_action = new RoamingMovementAction(
				(RoamingAroundCenterConfigSO)_npcMovement.NPCMovementConfig,
				_agent,
				_npcMovement.transform.position);
		}
		else if (_npcMovement.NPCMovementConfig is PathwayConfigSO)
		{
			_action = new PathwayMovementAction(
				(PathwayConfigSO)_npcMovement.NPCMovementConfig,
				_agent);
		}
	}
}
