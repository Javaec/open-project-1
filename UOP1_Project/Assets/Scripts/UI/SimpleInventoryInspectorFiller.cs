using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventoryInspectorFiller : MonoBehaviour
{
	[SerializeField] InspectorPreviewFiller _inspectorPreviewFiller = default;

	[SerializeField] InspectorDescriptionFiller _inspectorDescriptionFiller = default;


	public void FillItemInspector(Item itemToInspect)
	{
		_inspectorPreviewFiller.FillPreview(itemToInspect);
		_inspectorDescriptionFiller.FillDescription(itemToInspect);
	}
}
