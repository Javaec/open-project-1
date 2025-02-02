﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeIngredientsFiller : MonoBehaviour
{
	[SerializeField] List<IngredientFiller> instantiatedGameObjects = new List<IngredientFiller>();

	public void FillIngredients(List<ItemStack> listofIngredients, bool[] availabilityArray)
	{
		if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
		{
			gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;
		}


		int maxCount = Mathf.Max(listofIngredients.Count, instantiatedGameObjects.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < listofIngredients.Count)
			{
				if (i >= instantiatedGameObjects.Count)
				{
					//Do nothing, maximum ingredients for a recipe reached
					Debug.Log("Maximum ingredients reached");
				}
				else
				{
					//fill
					bool isAvailable = availabilityArray[i];
					instantiatedGameObjects[i].FillIngredient(listofIngredients[i], isAvailable);

					instantiatedGameObjects[i].gameObject.SetActive(true);
				}
			}
			else if (i < instantiatedGameObjects.Count)
			{
				//Desactive
				instantiatedGameObjects[i].gameObject.SetActive(false);
			}
		}

		StartCoroutine(waitBeforeDesactiveLayout());
	}

	IEnumerator waitBeforeDesactiveLayout()
	{
		yield return new WaitForSeconds(1);
		//disable layout group after layout calculation
		if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
		{
			gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
		}
	}
}
