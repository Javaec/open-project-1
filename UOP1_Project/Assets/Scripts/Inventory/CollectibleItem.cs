﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
	[SerializeField] Item _currentItem = default;
	[SerializeField] SpriteRenderer[] _itemImages = default;

	void Start()
	{
		if (_itemImages != null)
		{
			SetCubeItem();
		}
	}

	public void PickedItem()
	{
	}

	public Item GetItem()
	{
		return _currentItem;
	}

	public void SetItem(Item item)
	{
		_currentItem = item;
	}

	//this function is only for testing 
	public void SetCubeItem()
	{
		for (int i = 0; i < _itemImages.Length; i++)
		{
			_itemImages[i].sprite = _currentItem.PreviewImage;
		}
	}
}
