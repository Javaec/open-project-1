using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 51)]
public class QuestSO : ScriptableObject
{
	[Tooltip("The collection of Steps composing the Quest")] [SerializeField]
	List<StepSO> _steps = new List<StepSO>();

	[SerializeField] bool _isDone = false;

	public List<StepSO> Steps
	{
		get
		{
			return _steps;
		}
	}

	public bool IsDone
	{
		get
		{
			return _isDone;
		}
	}

	public void FinishQuest()
	{
		_isDone = true;
	}
}
