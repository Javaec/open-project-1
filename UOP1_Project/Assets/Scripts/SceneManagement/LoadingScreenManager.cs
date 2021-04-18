using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
	[Header("Loading screen Event")]
	//The loading screen event we are listening to
	[SerializeField]
	BoolEventChannelSO _ToggleLoadingScreen = default;

	[Header("Loading screen ")] public GameObject loadingInterface;

	void OnEnable()
	{
		if (_ToggleLoadingScreen != null)
		{
			_ToggleLoadingScreen.OnEventRaised += ToggleLoadingScreen;
		}
	}

	void OnDisable()
	{
		if (_ToggleLoadingScreen != null)
		{
			_ToggleLoadingScreen.OnEventRaised += ToggleLoadingScreen;
		}
	}

	void ToggleLoadingScreen(bool state)
	{
		loadingInterface.SetActive(state);
	}
}
