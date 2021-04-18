using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.
/// Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.
/// </summary>
public class DialogueManager : MonoBehaviour
{
	//	[SerializeField] private ChoiceBox _choiceBox; // TODO: Demonstration purpose only. Remove or adjust later.

	[SerializeField] InputReader _inputReader = default;
	int _counter;

	bool _reachedEndOfDialogue
	{
		get
		{
			return _counter >= _currentDialogue.DialogueLines.Count;
		}
	}

	[Header("Listening on channels")] [SerializeField]
	DialogueDataChannelSO _startDialogue = default;

	[SerializeField] DialogueChoiceChannelSO _makeDialogueChoiceEvent = default;

	[Header("BoradCasting on channels")] [SerializeField]
	DialogueLineChannelSO _openUIDialogueEvent = default;

	[SerializeField] DialogueChoicesChannelSO _showChoicesUIEvent = default;
	[SerializeField] DialogueDataChannelSO _endDialogue = default;
	[SerializeField] VoidEventChannelSO _continueWithStep = default;
	[SerializeField] VoidEventChannelSO _closeDialogueUIEvent = default;


	DialogueDataSO _currentDialogue = default;

	void Start()
	{
		if (_startDialogue != null)
		{
			_startDialogue.OnEventRaised += DisplayDialogueData;
		}
	}

	/// <summary>
	/// Displays DialogueData in the UI, one by one.
	/// </summary>
	/// <param name="dialogueDataSO"></param>
	public void DisplayDialogueData(DialogueDataSO dialogueDataSO)
	{
		BeginDialogueData(dialogueDataSO);
		DisplayDialogueLine(_currentDialogue.DialogueLines[_counter], dialogueDataSO.Actor);
	}

	/// <summary>
	/// Prepare DialogueManager when first time displaying DialogueData. 
	/// <param name="dialogueDataSO"></param>
	void BeginDialogueData(DialogueDataSO dialogueDataSO)
	{
		_counter = 0;
		_inputReader.EnableDialogueInput();
		_inputReader.advanceDialogueEvent += OnAdvance;
		_currentDialogue = dialogueDataSO;
	}

	/// <summary>
	/// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
	/// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
	/// </summary>
	/// <param name="dialogueLine"></param>
	public void DisplayDialogueLine(LocalizedString dialogueLine, ActorSO actor)
	{
		if (_openUIDialogueEvent != null)
		{
			_openUIDialogueEvent.RaiseEvent(dialogueLine, actor);
		}
	}

	void OnAdvance()
	{
		_counter++;

		if (!_reachedEndOfDialogue)
		{
			DisplayDialogueLine(_currentDialogue.DialogueLines[_counter], _currentDialogue.Actor);
		}
		else
		{
			if (_currentDialogue.Choices.Count > 0)
			{
				DisplayChoices(_currentDialogue.Choices);
			}
			else
			{
				DialogueEndedAndCloseDialogueUI();
			}
		}
	}

	void DisplayChoices(List<Choice> choices)
	{
		_inputReader.advanceDialogueEvent -= OnAdvance;
		if (_makeDialogueChoiceEvent != null)
		{
			_makeDialogueChoiceEvent.OnEventRaised += MakeDialogueChoice;
		}

		if (_showChoicesUIEvent != null)
		{
			_showChoicesUIEvent.RaiseEvent(choices);
		}
	}

	void MakeDialogueChoice(Choice choice)
	{
		if (_makeDialogueChoiceEvent != null)
		{
			_makeDialogueChoiceEvent.OnEventRaised -= MakeDialogueChoice;
		}

		if (choice.ActionType == ChoiceActionType.continueWithStep)
		{
			if (_continueWithStep != null)
			{
				_continueWithStep.RaiseEvent();
			}

			if (choice.NextDialogue != null)
			{
				DisplayDialogueData(choice.NextDialogue);
			}
		}
		else
		{
			if (choice.NextDialogue != null)
			{
				DisplayDialogueData(choice.NextDialogue);
			}
			else
			{
				DialogueEndedAndCloseDialogueUI();
			}
		}
	}

	void DialogueEnded()
	{
		if (_endDialogue != null)
		{
			_endDialogue.RaiseEvent(_currentDialogue);
		}
	}

	public void DialogueEndedAndCloseDialogueUI()
	{
		if (_endDialogue != null)
		{
			_endDialogue.RaiseEvent(_currentDialogue);
		}

		if (_closeDialogueUIEvent != null)
		{
			_closeDialogueUIEvent.RaiseEvent();
		}

		_inputReader.advanceDialogueEvent -= OnAdvance;
		_inputReader.EnableGameplayInput();
	}
}
