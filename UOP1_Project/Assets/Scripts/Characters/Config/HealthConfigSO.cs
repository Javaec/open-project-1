using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthConfig", menuName = "EntityConfig/Health Config")]
public class HealthConfigSO : ScriptableObject
{
	[Tooltip("Initial critter health")] [SerializeField]
	int _maxHealth;

	public int MaxHealth
	{
		get
		{
			return _maxHealth;
		}
	}
}
