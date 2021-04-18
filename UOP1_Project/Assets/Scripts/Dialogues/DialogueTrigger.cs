using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] DialogueManager _dialogueManager = default;
	[SerializeField] DialogueDataSO _dialogueData = default;

	void OnTriggerEnter(Collider other)
	{
		_dialogueManager.DisplayDialogueData(_dialogueData);
	}
}
