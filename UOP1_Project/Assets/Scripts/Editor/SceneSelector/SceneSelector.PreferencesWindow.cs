using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using SceneSelectorInternal;

public partial class SceneSelector : EditorWindow
{
	class PreferencesWindow : EditorWindow
	{
		class Styles
		{
			public GUIStyle itemBorder;
			public GUIStyle buttonVisibilityOn;
			public GUIStyle buttonVisibilityOff;
		}

		const string kWindowCaption = "Scene Selector Preferences";
		const float kHeaderHeight = 0.0f;
		const float kItemHeight = 24.0f;
		const float kVisibilityButtonSize = 16.0f;

		public static float kColorMarkerFieldSize = Mathf.Ceil(Helper.kColorMarkerNormalSize * 1.41f + 8.0f);
		static readonly Color kItemBorderColor = new Color(1.0f, 1.0f, 1.0f, 0.16f);

		SceneSelector _owner;
		ColorSelectorWindow _colorSelectorWindow;
		ReorderableList _itemsReorderableList;
		Styles _styles;
		Vector2 _windowScrollPosition;

		List<Item> items
		{
			get
			{
				return _owner._storage.items;
			}
		}

		public static PreferencesWindow Open(SceneSelector owner)
		{
			PreferencesWindow window = GetWindow<PreferencesWindow>(true, kWindowCaption, true);
			window.Init(owner);
			return window;
		}

		void OnEnable()
		{
			wantsMouseMove = true;
		}

		void OnDisable()
		{
			_owner.SaveStorage();
			if (_colorSelectorWindow != null)
			{
				_colorSelectorWindow.Close();
			}
		}

		void OnGUI()
		{
			EnsureStyles();
			Helper.RepaintOnMouseMove(this);
			DrawWindow();
		}

		public void RepaintAll()
		{
			RepaintOwner();
			Repaint();
		}

		void Init(SceneSelector owner)
		{
			_owner = owner;
			CreateReorderableList();
		}

		void CreateReorderableList()
		{
			_itemsReorderableList = new ReorderableList(items, typeof(Item), true, true, false, false);
			_itemsReorderableList.drawElementCallback = DrawItem;
			_itemsReorderableList.drawElementBackgroundCallback = DrawItemBackground;
			_itemsReorderableList.onReorderCallback = OnReorder;
			_itemsReorderableList.headerHeight = kHeaderHeight;
			_itemsReorderableList.elementHeight = kItemHeight;
		}

		void DrawWindow()
		{
			using (EditorGUILayout.ScrollViewScope scrollScope = new EditorGUILayout.ScrollViewScope(_windowScrollPosition))
			{
				GUILayout.Space(4.0f);
				_itemsReorderableList.DoLayoutList();
				_windowScrollPosition = scrollScope.scrollPosition;
			}
		}

		void DrawItem(Rect rect, int index, bool isActive, bool isFocused)
		{
			Item item = items[index];
			GameSceneSO gameScene = item.gameSceneSO;
			if (gameScene != null)
			{
				Rect colorMarkerRect = rect;
				colorMarkerRect.width = colorMarkerRect.height;

				if (Helper.DrawColorMarker(colorMarkerRect, item.color, true, true))
				{
					Rect colorSelectorRect = GUIUtility.GUIToScreenRect(colorMarkerRect);
					_colorSelectorWindow = ColorSelectorWindow.Open(colorSelectorRect, this, item);
				}

				Rect itemLabelRect = rect;
				itemLabelRect.x += colorMarkerRect.width;
				itemLabelRect.width -= kVisibilityButtonSize + colorMarkerRect.width;

				GUI.Label(itemLabelRect, gameScene.name);

				Rect visibilityButtonRect = new Rect(rect);
				visibilityButtonRect.width = kVisibilityButtonSize;
				visibilityButtonRect.height = kVisibilityButtonSize;
				visibilityButtonRect.x = itemLabelRect.x + itemLabelRect.width;
				visibilityButtonRect.y += (rect.height - visibilityButtonRect.height) * 0.5f;

				GUIStyle visibilityStyle = item.isVisible
					? _styles.buttonVisibilityOn
					: _styles.buttonVisibilityOff;

				if (GUI.Button(visibilityButtonRect, GUIContent.none, visibilityStyle))
				{
					item.isVisible = !item.isVisible;
					RepaintOwner();
				}
			}
		}

		void DrawItemBackground(Rect rect, int index, bool isActive, bool isFocused)
		{
			ReorderableList.defaultBehaviours.DrawElementBackground(rect, index, isActive, isFocused, true);
			using (Helper.ReplaceColor.With(kItemBorderColor))
			{
				GUI.Box(rect, GUIContent.none, _styles.itemBorder);
			}
		}

		void OnReorder(ReorderableList _)
		{
			RepaintOwner();
		}

		void RepaintOwner()
		{
			_owner.Repaint();
		}

		void EnsureStyles()
		{
			if (_styles == null)
			{
				_styles = new Styles();

				_styles.itemBorder = new GUIStyle(GUI.skin.GetStyle("HelpBox"));

				_styles.buttonVisibilityOn = new GUIStyle(GUI.skin.label);
				_styles.buttonVisibilityOn.padding = new RectOffset(0, 0, 0, 0);
				_styles.buttonVisibilityOn.normal.background = EditorGUIUtility.FindTexture("d_scenevis_visible");
				_styles.buttonVisibilityOn.hover.background = EditorGUIUtility.FindTexture("d_scenevis_visible_hover");

				_styles.buttonVisibilityOff = new GUIStyle(GUI.skin.label);
				_styles.buttonVisibilityOff.padding = new RectOffset(0, 0, 0, 0);
				_styles.buttonVisibilityOff.normal.background = EditorGUIUtility.FindTexture("d_scenevis_hidden");
				_styles.buttonVisibilityOff.hover.background = EditorGUIUtility.FindTexture("d_scenevis_hidden_hover");
			}
		}
	}
}
