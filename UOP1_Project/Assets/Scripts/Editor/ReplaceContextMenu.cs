using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UOP1.EditorTools.Replacer
{
	class ReplaceContextMenu
	{
		static Type hierarchyType;

		static EditorWindow focusedWindow;
		static IMGUIContainer hierarchyGUI;

		static Vector2 mousePosition;
		static bool hasExecuted;

		[InitializeOnLoadMethod]
		static void OnInitialize()
		{
			hierarchyType = typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");

			EditorApplication.update += TrackFocusedHierarchy;
		}

		static void TrackFocusedHierarchy()
		{
			if (focusedWindow != EditorWindow.focusedWindow)
			{
				focusedWindow = EditorWindow.focusedWindow;

				if (focusedWindow?.GetType() == hierarchyType)
				{
					if (hierarchyGUI != null)
					{
						hierarchyGUI.onGUIHandler -= OnFocusedHierarchyGUI;
					}

					hierarchyGUI = focusedWindow.rootVisualElement.parent.Query<IMGUIContainer>();
					hierarchyGUI.onGUIHandler += OnFocusedHierarchyGUI;
				}
			}
		}

		static void OnFocusedHierarchyGUI()
		{
			// As Event.current is null during context-menu callback, we need to track mouse position on hierarchy GUI
			mousePosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
		}

		[MenuItem("GameObject/Replace", true, priority = 0)]
		static bool ReplaceSelectionValidate()
		{
			return Selection.gameObjects.Length > 0;
		}

		[MenuItem("GameObject/Replace", priority = 0)]
		static void ReplaceSelection()
		{
			if (hasExecuted)
			{
				return;
			}

			Rect rect = new Rect(mousePosition, new Vector2(240, 360));

			ReplacePrefabSearchPopup.Show(rect);

			EditorApplication.delayCall += () => hasExecuted = false;
		}
	}
}
