﻿using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsDeadCondition", menuName = "State Machines/Conditions/Is Dead")]
public class IsDeadConditionSO : StateConditionSO
{
	protected override Condition CreateCondition()
	{
		return new IsDeadCondition();
	}
}

public class IsDeadCondition : Condition
{
	Damageable _damageableScript;

	public override void Awake(StateMachine stateMachine)
	{
		_damageableScript = stateMachine.GetComponent<Damageable>();
	}

	protected override bool Statement()
	{
		return _damageableScript.IsDead;
	}
}
