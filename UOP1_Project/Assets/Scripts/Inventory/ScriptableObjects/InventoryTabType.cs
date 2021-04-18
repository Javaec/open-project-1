using UnityEngine;
using UnityEngine.Localization;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

public enum TabType
{
	customization,
	cookingItem,
	recipe,
	none
}

[CreateAssetMenu(fileName = "tabType", menuName = "Inventory/tabType", order = 51)]
public class InventoryTabType : ScriptableObject
{
	[Tooltip("The tab Name that will be displayed in the inventory")] [SerializeField]
	LocalizedString _tabName = default;


	[Tooltip("The tab type used to reference the item")] [SerializeField]
	TabType _tabType = default;

	public LocalizedString TabName
	{
		get
		{
			return _tabName;
		}
	}

	public TabType TabType
	{
		get
		{
			return _tabType;
		}
	}
}
