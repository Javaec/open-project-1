using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class UIManager : MonoBehaviour
{
	[Header("Listening on channels")] [Header("Dialogue Events")] [SerializeField]
	DialogueLineChannelSO _openUIDialogueEvent = default;

	[SerializeField] VoidEventChannelSO _closeUIDialogueEvent = default;

	[Header("Inventory Events")] [SerializeField]
	VoidEventChannelSO _openInventoryScreenEvent = default;

	[SerializeField] VoidEventChannelSO _openInventoryScreenForCookingEvent = default;
	[SerializeField] VoidEventChannelSO _closeInventoryScreenEvent = default;

	[Header("Interaction Events")] [SerializeField]
	VoidEventChannelSO _onInteractionEndedEvent = default;

	[SerializeField] InteractionUIEventChannelSO _setInteractionEvent = default;

	void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (_openUIDialogueEvent != null)
		{
			_openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
		}

		if (_closeUIDialogueEvent != null)
		{
			_closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;
		}

		if (_openInventoryScreenForCookingEvent != null)
		{
			_openInventoryScreenForCookingEvent.OnEventRaised += SetInventoryScreenForCooking;
		}

		if (_openInventoryScreenEvent != null)
		{
			_openInventoryScreenEvent.OnEventRaised += SetInventoryScreen;
		}

		if (_closeInventoryScreenEvent != null)
		{
			_closeInventoryScreenEvent.OnEventRaised += CloseInventoryScreen;
		}

		if (_setInteractionEvent != null)
		{
			_setInteractionEvent.OnEventRaised += SetInteractionPanel;
		}
	}

	void Start()
	{
		CloseUIDialogue();
	}

	[SerializeField] UIDialogueManager dialogueController = default;

	[SerializeField] UIInventoryManager inventoryPanel = default;

	[SerializeField] UIInteractionManager interactionPanel = default;

	public void OpenUIDialogue(LocalizedString dialogueLine, ActorSO actor)
	{
		dialogueController.SetDialogue(dialogueLine, actor);
		dialogueController.gameObject.SetActive(true);
	}

	public void CloseUIDialogue()
	{
		dialogueController.gameObject.SetActive(false);
	}

	public void SetInventoryScreenForCooking()
	{
		isForCooking = true;
		OpenInventoryScreen();
	}

	public void SetInventoryScreen()
	{
		isForCooking = false;
		OpenInventoryScreen();
	}

	bool isForCooking = false;

	void OpenInventoryScreen()
	{
		inventoryPanel.gameObject.SetActive(true);

		if (isForCooking)
		{
			inventoryPanel.FillInventory(TabType.recipe, true);
		}
		else
		{
			inventoryPanel.FillInventory();
		}
	}


	public void CloseInventoryScreen()
	{
		inventoryPanel.gameObject.SetActive(false);

		if (isForCooking)
		{
			_onInteractionEndedEvent.RaiseEvent();
		}
	}

	public void SetInteractionPanel(bool isOpenEvent, InteractionType interactionType)
	{
		if (isOpenEvent)
		{
			interactionPanel.FillInteractionPanel(interactionType);
		}

		interactionPanel.gameObject.SetActive(isOpenEvent);
	}
}
