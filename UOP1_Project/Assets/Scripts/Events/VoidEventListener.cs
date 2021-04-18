using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A flexible handler for void events in the form of a MonoBehaviour. Responses can be connected directly from the Unity Inspector.
/// </summary>
public class VoidEventListener : MonoBehaviour
{
	[SerializeField] VoidEventChannelSO _channel = default;

	public UnityEvent OnEventRaised;

	void OnEnable()
	{
		if (_channel != null)
		{
			_channel.OnEventRaised += Respond;
		}
	}

	void OnDisable()
	{
		if (_channel != null)
		{
			_channel.OnEventRaised -= Respond;
		}
	}

	void Respond()
	{
		if (OnEventRaised != null)
		{
			OnEventRaised.Invoke();
		}
	}
}
