using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
	[SerializeField] NPCMovementConfigSO _npcMovementConfig;

	[SerializeField] NPCMovementEventChannelSO _channel;

	public NPCMovementConfigSO NPCMovementConfig
	{
		get
		{
			return _npcMovementConfig;
		}
	}

	void OnEnable()
	{
		if (_channel != null)
		{
			_channel.OnEventRaised += Respond;
		}
	}

	void Respond(NPCMovementConfigSO value)
	{
		_npcMovementConfig = value;
	}
}
