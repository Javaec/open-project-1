using UnityEngine;

/// <summary>
/// This class goes on a trigger which, when entered, sends the player to another Location
/// </summary>
public class LocationExit : MonoBehaviour
{
	[Header("Loading settings")] [SerializeField]
	GameSceneSO[] _locationsToLoad = default;

	[SerializeField] bool _showLoadScreen = default;
	[SerializeField] PathAnchor _pathTaken = default;
	[SerializeField] PathSO _exitPath = default;

	[Header("Broadcasting on")] [SerializeField]
	LoadEventChannelSO _locationExitLoadChannel = default;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			UpdatePathTaken();
			_locationExitLoadChannel.RaiseEvent(_locationsToLoad, _showLoadScreen);
		}
	}

	void UpdatePathTaken()
	{
		if (_pathTaken != null)
		{
			_pathTaken.Path = _exitPath;
		}
	}
}
