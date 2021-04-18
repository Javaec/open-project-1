using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoamingAroundCenter", menuName = "EntityConfig/Roaming Around Center")]
public class RoamingAroundCenterConfigSO : NPCMovementConfigSO
{
	[Tooltip("Is roaming from spwaning center")] [SerializeField]
	bool _fromSpawningPoint = true;

	[Tooltip("Custom roaming center")] [SerializeField]
	Vector3 _customCenter;

	[Tooltip("Roaming distance from center")] [SerializeField]
	float _radius;

	public bool FromSpawningPoint
	{
		get
		{
			return _fromSpawningPoint;
		}
	}

	public Vector3 CustomCenter
	{
		get
		{
			return _customCenter;
		}
	}

	public float Radius
	{
		get
		{
			return _radius;
		}
	}
}
