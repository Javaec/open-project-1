using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] VoidEventChannelSO _onSceneReady = default;
	[SerializeField] AudioCueEventChannelSO _playMusicOn = default;
	[SerializeField] GameSceneSO _thisSceneSO = default;
	[SerializeField] AudioConfigurationSO _audioConfig = default;

	void OnEnable()
	{
		_onSceneReady.OnEventRaised += PlayMusic;
	}

	void OnDisable()
	{
		_onSceneReady.OnEventRaised -= PlayMusic;
	}

	void PlayMusic()
	{
		_playMusicOn.RaisePlayEvent(_thisSceneSO.musicTrack, _audioConfig);
	}
}
