using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Questline", menuName = "Quests/Questline", order = 51)]
public class QuestlineSO : ScriptableObject
{
	[Tooltip("The collection of Quests composing the Questline")] [SerializeField]
	List<QuestSO> _quests = new List<QuestSO>();

	[SerializeField] bool _isDone = false;

	public List<QuestSO> Quests
	{
		get
		{
			return _quests;
		}
	}

	public bool IsDone
	{
		get
		{
			return _isDone;
		}
	}

	public void FinishQuestline()
	{
		_isDone = true;
	}
}
