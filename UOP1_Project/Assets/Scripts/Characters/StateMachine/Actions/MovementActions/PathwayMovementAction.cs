﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathwayMovementAction : NPCMovementAction
{
	NavMeshAgent _agent;
	bool _isActiveAgent;
	List<WaypointData> _wayppoints;
	int _wayPointIndex;
	float _roamingSpeed;

	public PathwayMovementAction(
		PathwayConfigSO config, NavMeshAgent agent)
	{
		_agent = agent;
		_isActiveAgent = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;
		_wayPointIndex = 0;
		_roamingSpeed = config.Speed;
		_wayppoints = config.Waypoints;
	}

	public override void OnUpdate()
	{
	}

	public override void OnStateEnter()
	{
		if (_isActiveAgent)
		{
			_agent.speed = _roamingSpeed;
			_agent.isStopped = false;
			_agent.SetDestination(GetNextDestination());
		}
	}

	public override void OnStateExit()
	{
	}

	Vector3 GetNextDestination()
	{
		Vector3 result = _agent.transform.position;
		if (_wayppoints.Count > 0)
		{
			_wayPointIndex = (_wayPointIndex + 1) % _wayppoints.Count;
			result = _wayppoints[_wayPointIndex].waypoint;
		}

		return result;
	}
}
