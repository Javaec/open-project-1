using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsActuallyMoving", menuName = "State Machines/Conditions/Is Actually Moving")]
public class IsActuallyMovingConditionSO : StateConditionSO
{
	[SerializeField] float _treshold = 0.02f;

	protected override Condition CreateCondition()
	{
		return new IsActuallyMovingCondition(_treshold);
	}
}

public class IsActuallyMovingCondition : Condition
{
	float _treshold;
	CharacterController _characterController;

	public override void Awake(StateMachine stateMachine)
	{
		_characterController = stateMachine.GetComponent<CharacterController>();
	}

	public IsActuallyMovingCondition(float treshold)
	{
		_treshold = treshold;
	}

	protected override bool Statement()
	{
		return _characterController.velocity.sqrMagnitude > _treshold * _treshold;
	}
}
