using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ChasingTargetAction", menuName = "State Machines/Actions/Chasing Target Action")]
public class ChasingTargetActionSO : StateActionSO
{
	[Tooltip("Target transform anchor.")] [SerializeField]
	TransformAnchor _targetTransform = default;

	[Tooltip("NPC chasing speed")] [SerializeField]
	float _chasingSpeed = default;

	public Vector3 TargetPosition
	{
		get
		{
			return _targetTransform.Transform.position;
		}
	}

	public float ChasingSpeed
	{
		get
		{
			return _chasingSpeed;
		}
	}

	protected override StateAction CreateAction()
	{
		return new ChasingTargetAction();
	}
}

public class ChasingTargetAction : StateAction
{
	Critter _critter;
	ChasingTargetActionSO _config;
	NavMeshAgent _agent;
	bool _isActiveAgent;

	public override void Awake(StateMachine stateMachine)
	{
		_config = (ChasingTargetActionSO)OriginSO;
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
		_isActiveAgent = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;
	}

	public override void OnUpdate()
	{
		if (_isActiveAgent)
		{
			_agent.isStopped = false;
			_agent.SetDestination(_config.TargetPosition);
		}
	}

	public override void OnStateEnter()
	{
		if (_isActiveAgent)
		{
			_agent.speed = _config.ChasingSpeed;
		}
	}
}
