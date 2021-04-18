using System;
using UnityEngine;

[Serializable]
public class DropItem
{
	[SerializeField] Item _item;

	[SerializeField] float _itemDropRate;

	public Item Item
	{
		get
		{
			return _item;
		}
	}

	public float ItemDropRate
	{
		get
		{
			return _itemDropRate;
		}
	}
}
