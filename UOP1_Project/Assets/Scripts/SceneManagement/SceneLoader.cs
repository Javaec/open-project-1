using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the scene loading and unloading.
/// </summary>
public class SceneLoader : MonoBehaviour
{
	[SerializeField] GameSceneSO _gameplayScene = default;

	[Header("Load Events")] [SerializeField]
	LoadEventChannelSO _loadLocation = default;

	[SerializeField] LoadEventChannelSO _loadMenu = default;

	[Header("Broadcasting on")] [SerializeField]
	BoolEventChannelSO _toggleLoadingScreen = default;

	[SerializeField] VoidEventChannelSO _onSceneReady = default;

	List<AsyncOperationHandle<SceneInstance>> _loadingOperationHandles = new List<AsyncOperationHandle<SceneInstance>>();
	AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

	//Parameters coming from scene loading requests
	GameSceneSO[] _scenesToLoad;
	GameSceneSO[] _currentlyLoadedScenes = new GameSceneSO[] { };
	bool _showLoadingScreen;

	SceneInstance _gameplayManagerSceneInstance = new SceneInstance();

	void OnEnable()
	{
		_loadLocation.OnLoadingRequested += LoadLocation;
		_loadMenu.OnLoadingRequested += LoadMenu;
	}

	void OnDisable()
	{
		_loadLocation.OnLoadingRequested -= LoadLocation;
		_loadMenu.OnLoadingRequested -= LoadMenu;
	}

	/// <summary>
	/// This function loads the location scenes passed as array parameter
	/// </summary>
	void LoadLocation(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		_scenesToLoad = locationsToLoad;
		_showLoadingScreen = showLoadingScreen;

		//In case we are coming from the main menu, we need to load the persistent Gameplay manager scene first
		if (_gameplayManagerSceneInstance.Scene == null
		    || !_gameplayManagerSceneInstance.Scene.isLoaded)
		{
			StartCoroutine(ProcessGameplaySceneLoading(locationsToLoad, showLoadingScreen));
		}
		else
		{
			UnloadPreviousScenes();
		}
	}

	IEnumerator ProcessGameplaySceneLoading(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		_gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);

		while (_gameplayManagerLoadingOpHandle.Status != AsyncOperationStatus.Succeeded)
		{
			yield return null;
		}

		_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

		UnloadPreviousScenes();
	}

	/// <summary>
	/// Prepares to load the main menu scene, first removing the Gameplay scene in case the game is coming back from gameplay to menus.
	/// </summary>
	void LoadMenu(GameSceneSO[] menusToLoad, bool showLoadingScreen)
	{
		_scenesToLoad = menusToLoad;
		_showLoadingScreen = showLoadingScreen;

		//In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
		if (_gameplayManagerSceneInstance.Scene != null
		    && _gameplayManagerSceneInstance.Scene.isLoaded)
		{
			Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);
		}

		UnloadPreviousScenes();
	}

	/// <summary>
	/// In both Location and Menu loading, this function takes care of removing previously loaded temporary scenes.
	/// </summary>
	void UnloadPreviousScenes()
	{
		for (int i = 0; i < _currentlyLoadedScenes.Length; i++)
		{
			_currentlyLoadedScenes[i].sceneReference.UnLoadScene();
		}

		LoadNewScenes();
	}

	/// <summary>
	/// Kicks off the asynchronous loading of an array of scenes, either menus or Locations.
	/// </summary>
	void LoadNewScenes()
	{
		if (_showLoadingScreen)
		{
			_toggleLoadingScreen.RaiseEvent(true);
		}

		_loadingOperationHandles.Clear();
		//Build the array of handles of the temporary scenes to load
		for (int i = 0; i < _scenesToLoad.Length; i++)
		{
			_loadingOperationHandles.Add(_scenesToLoad[i].sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0));
		}

		StartCoroutine(LoadingProcess());
	}

	IEnumerator LoadingProcess()
	{
		bool done = _loadingOperationHandles.Count == 0;

		//This while will exit when all scenes requested have been unloaded
		while (!done)
		{
			for (int i = 0; i < _loadingOperationHandles.Count; ++i)
			{
				if (_loadingOperationHandles[i].Status != AsyncOperationStatus.Succeeded)
				{
					break;
				}
				else
				{
					done = true;
				}
			}

			yield return null;
		}

		//Save loaded scenes (to be unloaded at next load request)
		_currentlyLoadedScenes = _scenesToLoad;
		SetActiveScene();

		if (_showLoadingScreen)
		{
			_toggleLoadingScreen.RaiseEvent(false);
		}
	}

	/// <summary>
	/// This function is called when all the scenes have been loaded
	/// </summary>
	void SetActiveScene()
	{
		//All the scenes have been loaded, so we assume the first in the array is ready to become the active scene
		Scene s = ((SceneInstance)_loadingOperationHandles[0].Result).Scene;
		SceneManager.SetActiveScene(s);

		LightProbes.TetrahedralizeAsync();

		_onSceneReady.RaiseEvent(); //Spawn system will spawn the PigChef
	}

	void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!");
	}
}
