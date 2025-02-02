﻿using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SaveSystem : ScriptableObject
{
	[SerializeField] LoadEventChannelSO _loadLocation = default;
	[SerializeField] Inventory _playerInventory;

	public string saveFilename = "save.chop";
	public string backupSaveFilename = "save.chop.bak";
	public Save saveData = new Save();

	void OnEnable()
	{
		_loadLocation.OnLoadingRequested += CacheLoadLocations;
	}

	void OnDisable()
	{
		_loadLocation.OnLoadingRequested -= CacheLoadLocations;
	}

	void CacheLoadLocations(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		LocationSO locationSo = locationsToLoad[0] as LocationSO;
		if (locationSo)
		{
			saveData._locationId = locationSo.Guid;
		}

		SaveDataToDisk();
	}

	public bool LoadSaveDataFromDisk()
	{
		if (FileManager.LoadFromFile(saveFilename, out string json))
		{
			saveData.LoadFromJson(json);
			return true;
		}

		return false;
	}

	public IEnumerator LoadSavedInventory()
	{
		_playerInventory.Items.Clear();
		foreach (SerializedItemStack serializedItemStack in saveData._itemStacks)
		{
			AsyncOperationHandle<Item> loadItemOperationHandle = Addressables.LoadAssetAsync<Item>(serializedItemStack.itemGuid);
			yield return loadItemOperationHandle;
			if (loadItemOperationHandle.Status == AsyncOperationStatus.Succeeded)
			{
				Item itemSo = loadItemOperationHandle.Result;
				_playerInventory.Add(itemSo, serializedItemStack.amount);
			}
		}
	}

	public void SaveDataToDisk()
	{
		saveData._itemStacks.Clear();
		foreach (ItemStack itemStack in _playerInventory.Items)
		{
			saveData._itemStacks.Add(new SerializedItemStack(itemStack.Item.Guid, itemStack.Amount));
		}

		if (FileManager.MoveFile(saveFilename, backupSaveFilename))
		{
			if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
			{
				Debug.Log("Save successful");
			}
		}
	}

	public void WriteEmptySaveFile()
	{
		FileManager.WriteToFile(saveFilename, "");
	}
}
