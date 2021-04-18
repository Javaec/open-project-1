using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Class to trigger a cutscene.
/// </summary>
public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] PlayableDirector _playableDirector = default;
	[SerializeField] bool _playOnStart = default;
	[SerializeField] bool _playOnce = default;

	[SerializeField] PlayableDirectorChannelSO _playCutsceneEvent = default;

	void Start()
	{
		if (_playOnStart)
		{
			if (_playCutsceneEvent != null)
			{
				_playCutsceneEvent.RaiseEvent(_playableDirector);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (_playCutsceneEvent != null)
		{
			_playCutsceneEvent.RaiseEvent(_playableDirector);
		}

		if (_playOnce)
		{
			Destroy(this);
		}
	}
}
