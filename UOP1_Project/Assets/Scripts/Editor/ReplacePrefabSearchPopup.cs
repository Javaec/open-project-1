using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static UnityEditor.EditorGUIUtility;
using static UnityEditor.EditorJsonUtility;
using static UnityEngine.Application;

namespace UOP1.EditorTools.Replacer
{
	class ReplacePrefabSearchPopup : EditorWindow
	{
		const float previewHeight = 128;

		class ViewState : ScriptableObject
		{
			public TreeViewState treeViewState = new TreeViewState();
		}

		static ReplacePrefabSearchPopup window;
		static Styles styles;

		static Event evt
		{
			get
			{
				return Event.current;
			}
		}

		static string assetPath
		{
			get
			{
				return Path.Combine(dataPath.Remove(dataPath.Length - 7, 7), "Library", "ReplacePrefabTreeState.asset");
			}
		}

		bool hasSelection
		{
			get
			{
				return tree.state.selectedIDs.Count > 0;
			}
		}

		int selectedId
		{
			get
			{
				return tree.state.selectedIDs[0];
			}
		}

		GameObject instance
		{
			get
			{
				return EditorUtility.InstanceIDToObject(selectedId) as GameObject;
			}
		}

		SearchField searchField;
		PrefabSelectionTreeView tree;
		ViewState viewState;

		Vector2 startPos;
		Vector2 startSize;
		Vector2 lastSize;

		GameObjectPreview selectionPreview = new GameObjectPreview();

		public static void Show(Rect rect)
		{
			ReplacePrefabSearchPopup[] windows = Resources.FindObjectsOfTypeAll<ReplacePrefabSearchPopup>();
			window = windows.Length != 0 ? windows[0] : CreateInstance<ReplacePrefabSearchPopup>();

			window.Init();

			window.startPos = rect.position;
			window.startSize = rect.size;

			window.position = new Rect(rect.position, rect.size);
			// Need to predict start window size to avoid trash frame
			window.SetInitialSize();

			// This type of window supports resizing, but is also persistent, so we need to close it manually
			window.ShowPopup();

			//onSelectEntry += _ => window.Close();
		}

		void Init()
		{
			viewState = CreateInstance<ViewState>();

			if (File.Exists(assetPath))
			{
				FromJsonOverwrite(File.ReadAllText(assetPath), viewState);
			}

			tree = new PrefabSelectionTreeView(viewState.treeViewState);
			tree.onSelectEntry += OnSelectEntry;

			AssetPreview.SetPreviewTextureCacheSize(tree.RowsCount);

			searchField = new SearchField();
			searchField.downOrUpArrowKeyPressed += tree.SetFocusAndEnsureSelectedItem;
			searchField.SetFocus();
		}

		void OnSelectEntry(GameObject prefab)
		{
			ReplaceTool.ReplaceSelectedObjects(Selection.gameObjects, prefab);
		}

		void OnEnable()
		{
			Init();
		}

		void OnDisable()
		{
			tree.Cleanup();
		}

		public new void Close()
		{
			SaveState();
			base.Close();
		}

		void SaveState()
		{
			File.WriteAllText(assetPath, ToJson(viewState));
		}

		void OnGUI()
		{
			if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Escape)
			{
				if (tree.hasSearch)
				{
					tree.searchString = "";
				}
				else
				{
					Close();
				}
			}

			if (focusedWindow != this)
			{
				Close();
			}

			if (styles == null)
			{
				styles = new Styles();
			}

			DoToolbar();
			DoTreeView();
			DoSelectionPreview();
		}

		void DoToolbar()
		{
			tree.searchString = searchField.OnToolbarGUI(tree.searchString);

			GUILayout.Label("Replace With...", styles.headerLabel);
		}

		void DoTreeView()
		{
			Rect rect = GUILayoutUtility.GetRect(0, 10000, 0, 10000);
			rect.x += 2;
			rect.width -= 4;

			rect.y += 2;
			rect.height -= 4;

			tree.OnGUI(rect);
		}

		void DoSelectionPreview()
		{
			if (hasSelection && tree.IsRenderable(selectedId))
			{
				SetSize(startSize.x, startSize.y + previewHeight);
				Rect previewRect = GUILayoutUtility.GetRect(position.width, previewHeight);

				selectionPreview.CreatePreviewForTarget(instance);
				selectionPreview.RenderInteractivePreview(previewRect);

				selectionPreview.DrawPreviewTexture(previewRect);
			}
			else
			{
				SetSize(startSize.x, startSize.y);
			}
		}

		void SetInitialSize()
		{
			if (hasSelection && tree.IsRenderable(selectedId))
			{
				SetSize(startSize.x, startSize.y + previewHeight);
			}
			else
			{
				SetSize(startSize.x, startSize.y);
			}
		}

		void SetSize(float width, float height)
		{
			Vector2 newSize = new Vector2(width, height);
			if (newSize != lastSize)
			{
				lastSize = newSize;
				position = new Rect(position.x, position.y, width, height);
			}
		}

		class Styles
		{
			public GUIStyle headerLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
			{
				fontSize = 11,
				fontStyle = FontStyle.Bold
			};
		}
	}
}
