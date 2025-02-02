﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField] Inventory _currentInventory = default;
	[SerializeField] ItemEventChannelSO _cookRecipeEvent = default;
	[SerializeField] ItemEventChannelSO _useItemEvent = default;
	[SerializeField] ItemEventChannelSO _equipItemEvent = default;
	[SerializeField] ItemEventChannelSO _rewardItemEvent = default;
	[SerializeField] ItemEventChannelSO _giveItemEvent = default;
	[SerializeField] ItemEventChannelSO _addItemEvent = default;
	[SerializeField] ItemEventChannelSO _removeItemEvent = default;
	[SerializeField] SaveSystem _saveSystem;

	void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (_cookRecipeEvent != null)
		{
			_cookRecipeEvent.OnEventRaised += CookRecipeEventRaised;
		}

		if (_useItemEvent != null)
		{
			_useItemEvent.OnEventRaised += UseItemEventRaised;
		}

		if (_equipItemEvent != null)
		{
			_equipItemEvent.OnEventRaised += EquipItemEventRaised;
		}

		if (_addItemEvent != null)
		{
			_addItemEvent.OnEventRaised += AddItem;
		}

		if (_removeItemEvent != null)
		{
			_removeItemEvent.OnEventRaised += RemoveItem;
		}

		if (_rewardItemEvent != null)
		{
			_rewardItemEvent.OnEventRaised += AddItem;
		}

		if (_giveItemEvent != null)
		{
			_giveItemEvent.OnEventRaised += RemoveItem;
		}
	}

	void OnDisable()
	{
		if (_cookRecipeEvent != null)
		{
			_cookRecipeEvent.OnEventRaised -= CookRecipeEventRaised;
		}

		if (_useItemEvent != null)
		{
			_useItemEvent.OnEventRaised -= UseItemEventRaised;
		}

		if (_equipItemEvent != null)
		{
			_equipItemEvent.OnEventRaised -= EquipItemEventRaised;
		}

		if (_addItemEvent != null)
		{
			_addItemEvent.OnEventRaised -= AddItem;
		}

		if (_removeItemEvent != null)
		{
			_removeItemEvent.OnEventRaised -= RemoveItem;
		}
	}


	void AddItemWithUIUpdate(Item item)
	{
		_currentInventory.Add(item);
		if (_currentInventory.Contains(item))
		{
			ItemStack itemToUpdate = _currentInventory.Items.Find(o => o.Item == item);
			//	UIManager.Instance.UpdateInventoryScreen(itemToUpdate, false);
		}
	}

	void RemoveItemWithUIUpdate(Item item)
	{
		ItemStack itemToUpdate = new ItemStack();

		if (_currentInventory.Contains(item))
		{
			itemToUpdate = _currentInventory.Items.Find(o => o.Item == item);
		}

		_currentInventory.Remove(item);

		bool removeItem = _currentInventory.Contains(item);
		//	UIManager.Instance.UpdateInventoryScreen(itemToUpdate, removeItem);
	}

	void AddItem(Item item)
	{
		_currentInventory.Add(item);
		_saveSystem.SaveDataToDisk();
	}

	void RemoveItem(Item item)
	{
		_currentInventory.Remove(item);
		_saveSystem.SaveDataToDisk();
	}


	void CookRecipeEventRaised(Item recipe)
	{
		//find recipe
		if (_currentInventory.Contains(recipe))
		{
			List<ItemStack> ingredients = recipe.IngredientsList;
			//remove ingredients (when it's a consumable)
			if (_currentInventory.hasIngredients(ingredients))
			{
				for (int i = 0; i < ingredients.Count; i++)
				{
					if (ingredients[i].Item.ItemType.ActionType == ItemInventoryActionType.use)
					{
						_currentInventory.Remove(ingredients[i].Item, ingredients[i].Amount);
					}
				}

				//add dish
				_currentInventory.Add(recipe.ResultingDish);
			}
		}
	}

	public void UseItemEventRaised(Item item)
	{
		RemoveItem(item);
	}

	public void EquipItemEventRaised(Item item)
	{
	}
}
