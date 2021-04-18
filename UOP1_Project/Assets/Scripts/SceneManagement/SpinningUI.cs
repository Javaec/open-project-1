using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningUI : MonoBehaviour
{
	RectTransform rectComponent;
	public float rotateSpeed = 200f;

	void Start()
	{
		rectComponent = GetComponent<RectTransform>();
	}

	void Update()
	{
		rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
	}
}
