using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;


public class InspectorDescriptionFiller : MonoBehaviour
{
	[SerializeField] LocalizeStringEvent _textDescription = default;

	[SerializeField] LocalizeStringEvent _textName = default;

	public void FillDescription(Item itemToInspect)
	{
		_textName.gameObject.SetActive(true);
		_textDescription.gameObject.SetActive(true);


		_textName.StringReference = itemToInspect.Name;
		_textDescription.StringReference = itemToInspect.Description;
	}
}
