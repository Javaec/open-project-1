using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntrance : MonoBehaviour
{
	[Header("Asset References")] [SerializeField]
	PathSO _entrancePath;

	public PathSO EntrancePath
	{
		get
		{
			return _entrancePath;
		}
	}
}
