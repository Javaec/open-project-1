﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;

public class UIDialogueChoiceFiller : MonoBehaviour
{
	[SerializeField] LocalizeStringEvent _choiceText = default;
	[SerializeField] DialogueChoiceChannelSO _makeAChoiceEvent = default;

	Choice currentChoice;

	public void FillChoice(Choice choiceToFill)
	{
		currentChoice = choiceToFill;
		_choiceText.StringReference = choiceToFill.Response;
	}

	public void ButtonClicked()
	{
		if (_makeAChoiceEvent != null)
		{
			_makeAChoiceEvent.RaiseEvent(currentChoice);
		}
	}
}
