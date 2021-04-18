using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/NPC Movement Stop Elapsed")]
public class NPCMovementStopConditionSO : StateConditionSO<NPCMovementStopCondition>
{
}

public class NPCMovementStopCondition : Condition
{
	float _startTime;
	NPCMovement _npcMovement;

	public override void Awake(StateMachine stateMachine)
	{
		_npcMovement = stateMachine.GetComponent<NPCMovement>();
	}

	public override void OnStateEnter()
	{
		_startTime = Time.time;
	}

	protected override bool Statement()
	{
		return Time.time >= _startTime + _npcMovement.NPCMovementConfig.StopDuration;
	}
}
