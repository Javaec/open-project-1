﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Helper component, to quickly apply the settings from an <c>AudioConfigurationSO</c> SO to an <c>AudioSource</c>.
/// Useful to add a configuration to the AudioSource that a Timeline is pointing to.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioConfigApplier : MonoBehaviour
{
	public AudioConfigurationSO config;

	void OnValidate()
	{
		ConfigureAudioSource();
	}

	void Start()
	{
		ConfigureAudioSource();
	}

	void ConfigureAudioSource()
	{
		if (config != null)
		{
			AudioSource audioSource = GetComponent<AudioSource>();
			config.ApplyTo(audioSource);
		}
	}
}
