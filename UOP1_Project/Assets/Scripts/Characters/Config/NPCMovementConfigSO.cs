using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class NPCMovementConfigSO : ScriptableObject
{
	[Tooltip("Waypoint stop duration")] [SerializeField]
	float _stopDuration;

	[Tooltip("Roaming speed")] [SerializeField]
	float _speed;

	public float Speed
	{
		get
		{
			return _speed;
		}
	}

	public float StopDuration
	{
		get
		{
			return _stopDuration;
		}
	}
}
