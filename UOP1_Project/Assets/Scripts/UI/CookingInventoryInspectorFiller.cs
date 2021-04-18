using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingInventoryInspectorFiller : MonoBehaviour
{
	[SerializeField] InspectorPreviewFiller _inspectorPreviewFiller = default;

	[SerializeField] InspectorDescriptionFiller _inspectorDescriptionFiller = default;

	[SerializeField] RecipeIngredientsFiller _recipeIngredientsFiller = default;


	public void FillItemInspector(Item itemToInspect, bool[] availabilityArray)
	{
		_inspectorPreviewFiller.FillPreview(itemToInspect);
		_inspectorDescriptionFiller.FillDescription(itemToInspect);
		_recipeIngredientsFiller.FillIngredients(itemToInspect.IngredientsList, availabilityArray);
	}
}
