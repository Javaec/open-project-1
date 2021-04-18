using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
/// </summary>
[CreateAssetMenu(fileName = "newActor", menuName = "Dialogues/Actor")]
public class ActorSO : ScriptableObject
{
	public LocalizedString ActorName
	{
		get
		{
			return _actorName;
		}
	}

	[SerializeField] LocalizedString _actorName = default;
}
