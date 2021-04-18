using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// Allows a "cold start" in the editor, when pressing Play and not passing from the Initialisation scene.
/// </summary> 
public class EditorColdStartup : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField] GameSceneSO _thisSceneSO = default;
	[SerializeField] GameSceneSO _persistentManagersSO = default;
	[SerializeField] AssetReference _loadSceneEventChannel = default;

	void Start()
	{
		if (!SceneManager.GetSceneByName(_persistentManagersSO.sceneReference.editorAsset.name).isLoaded)
		{
			_persistentManagersSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
		}
	}

	void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
	{
		_loadSceneEventChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += ReloadScene;
	}

	void ReloadScene(AsyncOperationHandle<LoadEventChannelSO> obj)
	{
		LoadEventChannelSO loadEventChannelSO = (LoadEventChannelSO)_loadSceneEventChannel.Asset;
		loadEventChannelSO.RaiseEvent(new GameSceneSO[] {_thisSceneSO});

		SceneManager.UnloadSceneAsync(_thisSceneSO.sceneReference.editorAsset.name);
	}
#endif
}
