using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum stepType
{
	dialogue,
	giveItem,
	checkItem,
	rewardItem
}

[CreateAssetMenu(fileName = "step", menuName = "Quests/step", order = 51)]
public class StepSO : ScriptableObject
{
	[Tooltip("The Character this mission will need interaction with")] [SerializeField]
	ActorSO _actor = default;

	[Tooltip("The dialogue that will be diplayed befor an action, if any")] [SerializeField]
	DialogueDataSO _dialogueBeforeStep = default;

	[Tooltip("The dialogue that will be diplayed when the step is achieved")] [SerializeField] [FormerlySerializedAs("_winDialogue")]
	DialogueDataSO _completeDialogue = default;

	[Tooltip("The dialogue that will be diplayed if the step is not achieved yet")] [SerializeField] [FormerlySerializedAs("_loseDialogue")]
	DialogueDataSO _incompleteDialogue = default;

	[Tooltip("The item to check/give/reward")] [SerializeField]
	Item _item = default;

	[Tooltip("The type of the step")] [SerializeField]
	stepType _type = default;

	[SerializeField] bool _isDone = false;

	public DialogueDataSO DialogueBeforeStep
	{
		get
		{
			return _dialogueBeforeStep;
		}
	}

	public DialogueDataSO CompleteDialogue
	{
		get
		{
			return _completeDialogue;
		}
	}

	public DialogueDataSO IncompleteDialogue
	{
		get
		{
			return _incompleteDialogue;
		}
	}

	public Item Item
	{
		get
		{
			return _item;
		}
	}

	public stepType Type
	{
		get
		{
			return _type;
		}
	}

	public bool IsDone
	{
		get
		{
			return _isDone;
		}
	}

	public ActorSO Actor
	{
		get
		{
			return _actor;
		}
	}

	public void FinishStep()
	{
		_isDone = true;
	}
}
