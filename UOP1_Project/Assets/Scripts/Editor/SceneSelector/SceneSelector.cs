using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SceneSelectorInternal;
using UnityEditor.SceneManagement;
using SceneType = GameSceneSO.GameSceneType;

public partial class SceneSelector : EditorWindow, IHasCustomMenu
{
	const string kPreferencesKey = "uop1.SceneSelector.Preferences";
	const int kItemContentLeftPadding = 32;
	static readonly GUIContent kOpenPreferencesItemContent = new GUIContent("Open Preferences");

	Styles _styles;
	Storage _storage;
	PreferencesWindow _preferencesWindow;
	Vector2 _windowScrollPosition;
	bool _hasEmptyItems;

	List<Item> items
	{
		get
		{
			return _storage.items;
		}
	}

	Dictionary<string, Item> itemsMap
	{
		get
		{
			return _storage.itemsMap;
		}
	}

	[MenuItem("ChopChop/Scene Selector")]
	static void Open()
	{
		GetWindow<SceneSelector>();
	}

	void OnEnable()
	{
		wantsMouseMove = true;
		LoadStorage();
		PopulateItems();
	}

	void OnDisable()
	{
		if (_preferencesWindow != null)
		{
			_preferencesWindow.Close();
		}

		SaveStorage();
	}

	void OnGUI()
	{
		EnsureStyles();
		Helper.RepaintOnMouseMove(this);
		RemoveEmptyItemsIfRequired();
		DrawWindow();
	}

	void DrawWindow()
	{
		using (EditorGUILayout.ScrollViewScope scrollScope = new EditorGUILayout.ScrollViewScope(_windowScrollPosition))
		{
			GUILayout.Space(4.0f);
			DrawItems();
			_windowScrollPosition = scrollScope.scrollPosition;
		}

		if (GUILayout.Button("Reset list"))
		{
			//Force deletion of the storage
			_storage = new Storage();
			EditorPrefs.SetString(kPreferencesKey, "");

			OnEnable(); //search the project and populate the scene list again
		}
	}

	void DrawItems()
	{
		foreach (Item item in items)
		{
			DrawItem(item);
		}
	}

	void DrawItem(Item item)
	{
		if (item.isVisible)
		{
			GameSceneSO gameSceneSO = item.gameSceneSO;
			if (gameSceneSO != null)
			{
				if (GUILayout.Button(gameSceneSO.name, _styles.item))
				{
					Helper.OpenSceneSafe(gameSceneSO);
				}

				Rect colorMarkerRect = GUILayoutUtility.GetLastRect();
				colorMarkerRect.width = colorMarkerRect.height;
				colorMarkerRect.x += (_styles.item.padding.left - colorMarkerRect.width) * 0.5f;
				Helper.DrawColorMarker(colorMarkerRect, item.color);
			}
			else
			{
				// In case GameSceneSO was removed (see RemoveEmptyItemsIfRequired)
				_hasEmptyItems = true;
			}
		}
	}

	void LoadStorage()
	{
		_storage = new Storage();
		if (EditorPrefs.HasKey(kPreferencesKey))
		{
			string preferencesJSON = EditorPrefs.GetString(kPreferencesKey);
			EditorJsonUtility.FromJsonOverwrite(preferencesJSON, _storage);
		}
	}

	void SaveStorage()
	{
		string preferencesJSON = EditorJsonUtility.ToJson(_storage);
		EditorPrefs.SetString(kPreferencesKey, preferencesJSON);
	}

	void PopulateItems()
	{
		List<GameSceneSO> gameSceneSOs = new List<GameSceneSO>();
		Helper.FindAssetsByType(gameSceneSOs);

		foreach (GameSceneSO gameSceneSO in gameSceneSOs)
		{
			if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(gameSceneSO, out string guid, out long _))
			{
				if (itemsMap.TryGetValue(guid, out Item item))
				{
					item.gameSceneSO = gameSceneSO;
				}
				else
				{
					item = new Item()
					{
						gameSceneSO = gameSceneSO,
						guid = guid,
						color = Helper.GetDefaultColor(gameSceneSO)
					};

					items.Add(item);
					itemsMap.Add(guid, item);
				}
			}
		}
	}

	void RemoveEmptyItemsIfRequired()
	{
		if (_hasEmptyItems)
		{
			for (int i = items.Count - 1; i >= 0; --i)
			{
				Item sceneItem = items[i];
				if (sceneItem == null || sceneItem.gameSceneSO == null)
				{
					items.RemoveAt(i);
					itemsMap.Remove(sceneItem.guid);
				}
			}
		}

		_hasEmptyItems = false;
	}


	void EnsureStyles()
	{
		if (_styles == null)
		{
			_styles = new Styles();

			_styles.item = "MenuItem";
			_styles.item.padding.left = kItemContentLeftPadding;
		}
	}

	void OpenPreferences()
	{
		_preferencesWindow = PreferencesWindow.Open(this);
	}

	void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
	{
		menu.AddItem(kOpenPreferencesItemContent, false, OpenPreferences);
	}
}
