using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
	[SerializeField] InputReader _inputReader = default;
	[SerializeField] DialogueManager _dialogueManager = default;

	[SerializeField] PlayableDirectorChannelSO _playCutsceneEvent = default;

	[SerializeField] public DialogueLineChannelSO _playDialogueEvent = default;

	[SerializeField] public VoidEventChannelSO _pauseTimelineEvent = default;

	PlayableDirector _activePlayableDirector;
	bool _isPaused;

	bool IsCutscenePlaying
	{
		get
		{
			return _activePlayableDirector.playableGraph.GetRootPlayable(0).GetSpeed() != 0d;
		}
	}

	void OnEnable()
	{
		_inputReader.advanceDialogueEvent += OnAdvance;
	}

	void OnDisable()
	{
		_inputReader.advanceDialogueEvent -= OnAdvance;
	}

	void Start()
	{
		if (_playCutsceneEvent != null)
		{
			_playCutsceneEvent.OnEventRaised += PlayCutscene;
		}

		if (_playDialogueEvent != null)
		{
			_playDialogueEvent.OnEventRaised += PlayDialogueFromClip;
		}

		if (_pauseTimelineEvent != null)
		{
			_pauseTimelineEvent.OnEventRaised += PauseTimeline;
		}
	}

	void PlayCutscene(PlayableDirector activePlayableDirector)
	{
		_inputReader.EnableDialogueInput();

		_activePlayableDirector = activePlayableDirector;

		_isPaused = false;
		_activePlayableDirector.Play();
		_activePlayableDirector.stopped += HandleDirectorStopped;
	}

	void CutsceneEnded()
	{
		if (_activePlayableDirector != null)
		{
			_activePlayableDirector.stopped -= HandleDirectorStopped;
		}

		_inputReader.EnableGameplayInput();
		_dialogueManager.DialogueEndedAndCloseDialogueUI();
	}

	void HandleDirectorStopped(PlayableDirector director)
	{
		CutsceneEnded();
	}

	void PlayDialogueFromClip(LocalizedString dialogueLine, ActorSO actor)
	{
		_dialogueManager.DisplayDialogueLine(dialogueLine, actor);
	}

	/// <summary>
	/// This callback is executed when the player presses the button to advance dialogues. If the Timeline is currently paused due to a <c>DialogueControlClip</c>, it will resume its playback.
	/// </summary>
	void OnAdvance()
	{
		if (_isPaused)
		{
			ResumeTimeline();
		}
	}

	/// <summary>
	/// Called by <c>DialogueControlClip</c> on the Timeline.
	/// </summary>
	void PauseTimeline()
	{
		_isPaused = true;
		_activePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
	}

	void ResumeTimeline()
	{
		_isPaused = false;
		_activePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
	}
}
