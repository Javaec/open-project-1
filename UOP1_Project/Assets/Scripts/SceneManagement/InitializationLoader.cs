﻿using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for starting the game by loading the persistent managers scene 
/// and raising the event to load the Main Menu
/// </summary>
public class InitializationLoader : MonoBehaviour
{
	[Header("Persistent managers Scene")] [SerializeField]
	GameSceneSO _persistentManagersScene = default;

	[Header("Loading settings")] [SerializeField]
	GameSceneSO[] _menuToLoad = default;

	[Header("Broadcasting on")] [SerializeField]
	AssetReference _menuLoadChannel = default;

	void Start()
	{
		//Load the persistent managers scene
		_persistentManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
	}

	void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
	{
		_menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
	}

	void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
	{
		LoadEventChannelSO loadEventChannelSO = (LoadEventChannelSO)_menuLoadChannel.Asset;
		loadEventChannelSO.RaiseEvent(_menuToLoad);

		SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
	}
}
