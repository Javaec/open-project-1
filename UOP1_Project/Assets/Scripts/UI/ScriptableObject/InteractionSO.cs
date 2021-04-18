using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Interaction", menuName = "UI/Interaction", order = 51)]
public class InteractionSO : ScriptableObject
{
	[Tooltip("The interaction name")] [SerializeField]
	LocalizedString _interactionName = default;

	[Tooltip("The Interaction Type")] [SerializeField]
	InteractionType _interactionType = default;


	public LocalizedString InteractionName
	{
		get
		{
			return _interactionName;
		}
	}

	public InteractionType InteractionType
	{
		get
		{
			return _interactionType;
		}
	}
}
