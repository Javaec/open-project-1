﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple implementation of a MonoBehaviour that is able to request a sound being played by the <c>AudioManager</c>.
/// It fires an event on an <c>AudioCueEventSO</c> which acts as a channel, that the <c>AudioManager</c> will pick up and play.
/// </summary>
public class AudioCue : MonoBehaviour
{
	[Header("Sound definition")] [SerializeField]
	AudioCueSO _audioCue = default;

	[SerializeField] bool _playOnStart = false;

	[Header("Configuration")] [SerializeField]
	AudioCueEventChannelSO _audioCueEventChannel = default;

	[SerializeField] AudioConfigurationSO _audioConfiguration = default;

	AudioCueKey controlKey = AudioCueKey.Invalid;

	void Start()
	{
		if (_playOnStart)
		{
			StartCoroutine(PlayDelayed());
		}
	}

	void OnDisable()
	{
		_playOnStart = false;
	}

	IEnumerator PlayDelayed()
	{
		//The wait allows the AudioManager to be ready for play requests
		yield return new WaitForSeconds(.1f);

		//This additional check prevents the AudioCue from playing if the object is disabled or the scene unloaded
		//This prevents playing a looping AudioCue which then would be never stopped
		if (_playOnStart)
		{
			PlayAudioCue();
		}
	}

	public void PlayAudioCue()
	{
		controlKey = _audioCueEventChannel.RaisePlayEvent(_audioCue, _audioConfiguration, transform.position);
	}

	public void StopAudioCue()
	{
		if (controlKey != AudioCueKey.Invalid)
		{
			if (!_audioCueEventChannel.RaiseStopEvent(controlKey))
			{
				controlKey = AudioCueKey.Invalid;
			}
		}
	}

	public void FinishAudioCue()
	{
		if (controlKey != AudioCueKey.Invalid)
		{
			if (!_audioCueEventChannel.RaiseFinishEvent(controlKey))
			{
				controlKey = AudioCueKey.Invalid;
			}
		}
	}
}
