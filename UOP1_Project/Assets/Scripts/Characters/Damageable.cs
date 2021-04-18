using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	[SerializeField] HealthConfigSO _healthConfigSO;
	[SerializeField] GetHitEffectConfigSO _getHitEffectSO;
	[SerializeField] Renderer _mainMeshRenderer;
	[SerializeField] DroppableRewardConfigSO _droppableRewardSO;

	public DroppableRewardConfigSO DropableRewardConfig
	{
		get
		{
			return _droppableRewardSO;
		}
	}

	int _currentHealth = default;

	public bool GetHit { get; set; }
	public bool IsDead { get; set; }

	public GetHitEffectConfigSO GetHitEffectConfig
	{
		get
		{
			return _getHitEffectSO;
		}
	}

	public Renderer MainMeshRenderer
	{
		get
		{
			return _mainMeshRenderer;
		}
	}

	public int CurrentHealth
	{
		get
		{
			return _currentHealth;
		}
	}

	public UnityAction OnDie;

	void Awake()
	{
		_currentHealth = _healthConfigSO.MaxHealth;
	}

	public void ReceiveAnAttack(int damage)
	{
		_currentHealth -= damage;
		GetHit = true;
		if (_currentHealth <= 0)
		{
			IsDead = true;
			if (OnDie != null)
			{
				OnDie.Invoke();
			}
		}
	}
}
