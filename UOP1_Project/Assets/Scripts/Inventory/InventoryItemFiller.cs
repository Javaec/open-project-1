using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;

public class InventoryItemFiller : MonoBehaviour
{
	[SerializeField] Image _itemPreviewImage = default;
	[SerializeField] LocalizeStringEvent _itemTitle = default;
	[SerializeField] TextMeshProUGUI _itemCount = default;
	[SerializeField] Image _bgImage = default;
	[SerializeField] Image _imgHover = default;
	[SerializeField] Image _imgSelected = default;
	[HideInInspector] public ItemStack _currentItem = default;
	[SerializeField] Button _itemButton = default;

	public void SetItem(ItemStack itemStack, bool isSelected, ItemEventChannelSO selectItemEvent)
	{
		UnhoverItem();

		_currentItem = itemStack;

		_imgSelected.gameObject.SetActive(isSelected);

		_itemPreviewImage.sprite = itemStack.Item.PreviewImage;
		_itemTitle.StringReference = itemStack.Item.Name;
		_itemCount.text = itemStack.Amount.ToString();
		_bgImage.color = itemStack.Item.ItemType.TypeColor;

		_itemButton.onClick.RemoveAllListeners();

		_itemButton.onClick.AddListener(() =>
		{
			SelectItem();
			UnhoverItem();
			selectItemEvent.RaiseEvent(_currentItem.Item);
		});
	}


	public void HoverItem()
	{
		_imgHover.gameObject.SetActive(true);
	}

	public void UnhoverItem()
	{
		_imgHover.gameObject.SetActive(false);
	}

	public void SelectItem()

	{
		_imgSelected.gameObject.SetActive(true);
	}

	public void UnselectItem()
	{
		_imgSelected.gameObject.SetActive(false);
	}
}
