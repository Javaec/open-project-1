﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>
public class StartGame : MonoBehaviour
{
	public LoadEventChannelSO onPlayButtonPress;
	public GameSceneSO[] locationsToLoad;
	public bool showLoadScreen;

	public SaveSystem saveSystem;
	public Text startText;

	public Button resetSaveDataButton;
	bool _hasSaveData;

	void Start()
	{
		_hasSaveData = saveSystem.LoadSaveDataFromDisk();

		if (_hasSaveData)
		{
			startText.text = "Continue";
			resetSaveDataButton.gameObject.SetActive(true);
		}
		else
		{
			resetSaveDataButton.gameObject.SetActive(false);
		}
	}

	public void OnPlayButtonPress()
	{
		if (!_hasSaveData)
		{
			saveSystem.WriteEmptySaveFile();
			//Start new game
			onPlayButtonPress.RaiseEvent(locationsToLoad, showLoadScreen);
		}
		else
		{
			//Load Game
			StartCoroutine(LoadSaveGame());
		}
	}

	public void OnResetSaveDataPress()
	{
		_hasSaveData = false;
		startText.text = "Play";
		resetSaveDataButton.gameObject.SetActive(false);
	}

	public IEnumerator LoadSaveGame()
	{
		yield return StartCoroutine(saveSystem.LoadSavedInventory());

		string locationGuid = saveSystem.saveData._locationId;
		AsyncOperationHandle<LocationSO> asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);
		yield return asyncOperationHandle;
		if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
		{
			LocationSO locationSo = asyncOperationHandle.Result;
			onPlayButtonPress.RaiseEvent(new[] {(GameSceneSO)locationSo}, showLoadScreen);
		}
	}
}
