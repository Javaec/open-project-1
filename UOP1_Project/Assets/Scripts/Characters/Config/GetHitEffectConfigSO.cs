using UnityEngine;

[CreateAssetMenu(fileName = "GetHitEffectConfig", menuName = "EntityConfig/Get Hit Effect Config")]
public class GetHitEffectConfigSO : ScriptableObject
{
	[Tooltip("Flashing effect color applied when getting hit.")] [SerializeField]
	Color _getHitFlashingColor = default;

	[Tooltip("Flashing effect duration (in seconds).")] [SerializeField]
	float _getHitFlashingDuration = 0.5f;

	[Tooltip("Flashing effect speed (number of flashings during the duration).")] [SerializeField]
	float _getHitFlashingSpeed = 3.0f;

	public Color GetHitFlashingColor
	{
		get
		{
			return _getHitFlashingColor;
		}
	}

	public float GetHitFlashingDuration
	{
		get
		{
			return _getHitFlashingDuration;
		}
	}

	public float GetHitFlashingSpeed
	{
		get
		{
			return _getHitFlashingSpeed;
		}
	}
}
