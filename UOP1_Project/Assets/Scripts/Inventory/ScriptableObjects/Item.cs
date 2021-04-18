using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 51)]
public class Item : SerializableScriptableObject
{
	[Tooltip("The name of the item")] [SerializeField]
	LocalizedString _name = default;

	[Tooltip("A preview image for the item")] [SerializeField]
	Sprite _previewImage = default;

	[Tooltip("A description of the item")] [SerializeField]
	LocalizedString _description = default;


	[Tooltip("The type of item")] [SerializeField]
	ItemType _itemType = default;

	[Tooltip("A prefab reference for the model of the item")] [SerializeField]
	GameObject _prefab = default;

	[Tooltip("The list of the ingredients necessary to the recipe")] [SerializeField]
	List<ItemStack> _ingredientsList = new List<ItemStack>();

	[Tooltip("The resulting dish to the recipe")] [SerializeField]
	Item _resultingDish = default;


	public LocalizedString Name
	{
		get
		{
			return _name;
		}
	}

	public Sprite PreviewImage
	{
		get
		{
			return _previewImage;
		}
	}

	public LocalizedString Description
	{
		get
		{
			return _description;
		}
	}

	public ItemType ItemType
	{
		get
		{
			return _itemType;
		}
	}

	public GameObject Prefab
	{
		get
		{
			return _prefab;
		}
	}

	public List<ItemStack> IngredientsList
	{
		get
		{
			return _ingredientsList;
		}
	}

	public Item ResultingDish
	{
		get
		{
			return _resultingDish;
		}
	}
}
