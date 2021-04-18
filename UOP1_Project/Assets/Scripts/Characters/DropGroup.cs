using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class DropGroup
{
	[SerializeField] List<DropItem> _drops;

	[SerializeField] float _dropRate;

	public List<DropItem> Drops
	{
		get
		{
			return _drops;
		}
	}

	public float DropRate
	{
		get
		{
			return _dropRate;
		}
	}
}
