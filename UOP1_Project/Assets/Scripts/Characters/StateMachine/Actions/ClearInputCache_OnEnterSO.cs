﻿using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ClearInputCache_OnEnter", menuName = "State Machines/Actions/Clear Input Cache On Enter")]
public class ClearInputCache_OnEnterSO : StateActionSO
{
	protected override StateAction CreateAction()
	{
		return new ClearInputCache_OnEnter();
	}
}

public class ClearInputCache_OnEnter : StateAction
{
	Protagonist _protagonist;
	InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonist = stateMachine.GetComponent<Protagonist>();
		_interactionManager = stateMachine.GetComponentInChildren<InteractionManager>();
	}

	public override void OnUpdate()
	{
	}

	public override void OnStateEnter()
	{
		_protagonist.jumpInput = false;
		_interactionManager.currentInteractionType = InteractionType.None;
	}
}
