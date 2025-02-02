﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractionManager : MonoBehaviour
{
	[SerializeField] List<InteractionSO> _listInteractions = default;

	[SerializeField] UIInteractionItemFiller _interactionItem = default;

	public void FillInteractionPanel(InteractionType interactionType)
	{
		if (_listInteractions != null && _interactionItem != null)
		{
			if (_listInteractions.Exists(o => o.InteractionType == interactionType))

			{
				_interactionItem.FillInteractionPanel(_listInteractions.Find(o => o.InteractionType == interactionType));
			}
		}
	}
}
