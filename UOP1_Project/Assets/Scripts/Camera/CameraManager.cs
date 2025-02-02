﻿using System;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	public InputReader inputReader;
	public Camera mainCamera;
	public CinemachineFreeLook freeLookVCam;
	bool _isRMBPressed;

	[SerializeField] [Range(.5f, 3f)] float _speedMultiplier = 1f; //TODO: make this modifiable in the game settings											
	[SerializeField] TransformAnchor _cameraTransformAnchor = default;

	[Header("Listening on channels")]
	[Tooltip("The CameraManager listens to this event, fired by objects in any scene, to adapt camera position")]
	[SerializeField]
	TransformEventChannelSO _frameObjectChannel = default;


	bool _cameraMovementLock = false;

	public void SetupProtagonistVirtualCamera(Transform target)
	{
		freeLookVCam.Follow = target;
		freeLookVCam.LookAt = target;
		freeLookVCam.OnTargetObjectWarped(target, target.position - freeLookVCam.transform.position - Vector3.forward);
	}

	void OnEnable()
	{
		inputReader.cameraMoveEvent += OnCameraMove;
		inputReader.enableMouseControlCameraEvent += OnEnableMouseControlCamera;
		inputReader.disableMouseControlCameraEvent += OnDisableMouseControlCamera;

		if (_frameObjectChannel != null)
		{
			_frameObjectChannel.OnEventRaised += OnFrameObjectEvent;
		}

		_cameraTransformAnchor.Transform = mainCamera.transform;
	}

	void OnDisable()
	{
		inputReader.cameraMoveEvent -= OnCameraMove;
		inputReader.enableMouseControlCameraEvent -= OnEnableMouseControlCamera;
		inputReader.disableMouseControlCameraEvent -= OnDisableMouseControlCamera;

		if (_frameObjectChannel != null)
		{
			_frameObjectChannel.OnEventRaised -= OnFrameObjectEvent;
		}
	}

	void OnEnableMouseControlCamera()
	{
		_isRMBPressed = true;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		StartCoroutine(DisableMouseControlForFrame());
	}

	IEnumerator DisableMouseControlForFrame()
	{
		_cameraMovementLock = true;
		yield return new WaitForEndOfFrame();
		_cameraMovementLock = false;
	}

	void OnDisableMouseControlCamera()
	{
		_isRMBPressed = false;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		// when mouse control is disabled, the input needs to be cleared
		// or the last frame's input will 'stick' until the action is invoked again
		freeLookVCam.m_XAxis.m_InputAxisValue = 0;
		freeLookVCam.m_YAxis.m_InputAxisValue = 0;
	}

	void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
	{
		if (_cameraMovementLock)
		{
			return;
		}

		if (isDeviceMouse && !_isRMBPressed)
		{
			return;
		}

		freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * Time.deltaTime * _speedMultiplier;
		freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * Time.deltaTime * _speedMultiplier;
	}

	void OnFrameObjectEvent(Transform value)
	{
		SetupProtagonistVirtualCamera(value);
	}
}
