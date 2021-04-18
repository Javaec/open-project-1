using UnityEngine;

public class MenuController : MonoBehaviour
{
	[SerializeField] InputReader _inputReader;
	[SerializeField] GameObject _menuPrefab;
	GameObject _menuInstance;

	void OnEnable()
	{
		_inputReader.pauseEvent += OpenMenu;
		_inputReader.menuUnpauseEvent += UnpauseMenu;
	}

	void OnDisable()
	{
		_inputReader.pauseEvent -= OpenMenu;
		_inputReader.menuUnpauseEvent -= UnpauseMenu;
	}

	void OpenMenu()
	{
		if (_menuInstance == null)
		{
			_menuInstance = Instantiate(_menuPrefab);
		}

		_menuInstance.SetActive(true);
		_inputReader.EnableMenuInput();
	}

	void UnpauseMenu()
	{
		_menuInstance.SetActive(false);
		_inputReader.EnableGameplayInput();
	}
}
