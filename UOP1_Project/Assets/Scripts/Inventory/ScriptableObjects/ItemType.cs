using UnityEngine;
using UnityEngine.Localization;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

public enum itemInventoryType
{
	recipe,
	utensil,
	ingredient,
	customisation,
	dish
}

public enum ItemInventoryActionType
{
	cook,
	use,
	equip,
	doNothing
}

[CreateAssetMenu(fileName = "ItemType", menuName = "Inventory/ItemType", order = 51)]
public class ItemType : ScriptableObject
{
	[Tooltip("The action associated with the item type")] [SerializeField]
	LocalizedString _actionName = default;

	[Tooltip("The action associated with the item type")] [SerializeField]
	LocalizedString _typeName = default;

	[Tooltip("The Item's background color in the UI")] [SerializeField]
	Color _typeColor = default;

	[Tooltip("The Item's type")] [SerializeField]
	itemInventoryType _type = default;

	[Tooltip("The Item's action type")] [SerializeField]
	ItemInventoryActionType _actionType = default;


	[Tooltip("The tab type under which the item will be added")] [SerializeField]
	InventoryTabType _tabType = default;

	public LocalizedString ActionName
	{
		get
		{
			return _actionName;
		}
	}

	public LocalizedString TypeName
	{
		get
		{
			return _typeName;
		}
	}

	public Color TypeColor
	{
		get
		{
			return _typeColor;
		}
	}

	public ItemInventoryActionType ActionType
	{
		get
		{
			return _actionType;
		}
	}

	public itemInventoryType Type
	{
		get
		{
			return _type;
		}
	}

	public InventoryTabType TabType
	{
		get
		{
			return _tabType;
		}
	}
}
