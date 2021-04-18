using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class UIDialogueManager : MonoBehaviour
{
	[SerializeField] LocalizeStringEvent _lineText = default;

	[SerializeField] LocalizeStringEvent _actorNameText = default;

	[SerializeField] UIDialogueChoicesManager _choicesManager = default;

	[SerializeField] DialogueChoicesChannelSO _showChoicesEvent = default;

	void Start()
	{
		if (_showChoicesEvent != null)
		{
			_showChoicesEvent.OnEventRaised += ShowChoices;
		}
	}

	public void SetDialogue(LocalizedString dialogueLine, ActorSO actor)
	{
		_choicesManager.gameObject.SetActive(false);
		_lineText.StringReference = dialogueLine;
		_actorNameText.StringReference = actor.ActorName;
	}

	void ShowChoices(List<Choice> choices)
	{
		_choicesManager.FillChoices(choices);
		_choicesManager.gameObject.SetActive(true);
	}

	void HideChoices()
	{
		_choicesManager.gameObject.SetActive(false);
	}
}
