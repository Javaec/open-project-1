using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "EntityConfig/Attack Config")]
public class AttackConfigSO : ScriptableObject
{
	[Tooltip("Character attack strength")] [SerializeField]
	int _attackStrength;

	[Tooltip("Character attack reload duration (in second).")] [SerializeField]
	float _attackReloadDuration;

	public int AttackStrength
	{
		get
		{
			return _attackStrength;
		}
	}

	public float AttackReloadDuration
	{
		get
		{
			return _attackReloadDuration;
		}
	}
}
