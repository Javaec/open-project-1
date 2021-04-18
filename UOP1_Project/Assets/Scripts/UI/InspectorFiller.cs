using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorFiller : MonoBehaviour
{
	[SerializeField] SimpleInventoryInspectorFiller _simpleInventoryInspector = default;

	[SerializeField] CookingInventoryInspectorFiller _cookingInventoryInspector = default;

	public void FillItemInspector(Item itemToInspect, bool[] availabilityArray = null)
	{
		bool isForCooking = itemToInspect.ItemType.ActionType == ItemInventoryActionType.cook;

		_simpleInventoryInspector.gameObject.SetActive(!isForCooking);
		_cookingInventoryInspector.gameObject.SetActive(isForCooking);

		if (!isForCooking)
		{
			_simpleInventoryInspector.FillItemInspector(itemToInspect);
		}
		else
		{
			_cookingInventoryInspector.FillItemInspector(itemToInspect, availabilityArray);
		}
	}

	public void HideItemInspector()
	{
		_simpleInventoryInspector.gameObject.SetActive(false);
		_cookingInventoryInspector.gameObject.SetActive(false);
	}
}
