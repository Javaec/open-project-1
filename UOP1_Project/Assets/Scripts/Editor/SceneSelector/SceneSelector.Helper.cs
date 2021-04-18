using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SceneSelectorInternal
{
	static class KeyValuePairExtension
	{
		public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
		{
			key = tuple.Key;
			value = tuple.Value;
		}
	}

	static class Helper
	{
		public const float kColorMarkerNormalSize = 4.0f;
		public const float kColorMarkerHoveredSize = 6.0f;
		public static readonly Color kColorMarkerDarkTint = Color.gray;
		public static readonly Color kColorMarkerLightTint = new Color(1.0f, 1.0f, 1.0f, 0.32f);

		public struct ReplaceColor : IDisposable
		{
			public static ReplaceColor With(Color color)
			{
				return new ReplaceColor(color);
			}

			Color _oldColor;

			ReplaceColor(Color color)
			{
				_oldColor = GUI.color;
				GUI.color = color;
			}

			void IDisposable.Dispose()
			{
				GUI.color = _oldColor;
			}
		}

		static readonly Dictionary<Type, Color> kDefaultMarkerColors = new Dictionary<Type, Color>()
		{
			{typeof(PersistentManagersSO), Color.magenta},
			{typeof(GameplaySO), Color.magenta},
			{typeof(LocationSO), Color.green},
			{typeof(MenuSO), Color.cyan}
		};

		public static void RepaintOnMouseMove(EditorWindow window)
		{
			if (Event.current.type == EventType.MouseMove)
			{
				window.Repaint();
			}
		}

		public static bool DrawColorMarker(Rect rect, Color color, bool isClickable = false, bool isHoverable = false)
		{
			bool isClicked = false;
			if (isClickable)
			{
				isClicked = GUI.Button(rect, GUIContent.none, GUIStyle.none);
			}

			Event currentEvent = Event.current;
			bool isHovered = isHoverable && rect.Contains(currentEvent.mousePosition);
			float targetSize = isHovered ? kColorMarkerHoveredSize : kColorMarkerNormalSize;

			Vector2 size = rect.size;
			rect.size = new Vector2(targetSize, targetSize);
			rect.position += (size - rect.size) * 0.5f;

			Rect shadowRect = rect;
			shadowRect.position -= Vector2.one;
			shadowRect.size += Vector2.one;
			Rect lightRect = rect;
			lightRect.size += Vector2.one;

			GUIUtility.RotateAroundPivot(45.0f, rect.center);
			EditorGUI.DrawRect(shadowRect, color * kColorMarkerDarkTint);
			EditorGUI.DrawRect(lightRect, kColorMarkerLightTint);
			EditorGUI.DrawRect(rect, color);
			GUIUtility.RotateAroundPivot(-45.0f, rect.center);

			return isClicked;
		}

		public static void OpenSceneSafe(GameSceneSO gameSceneSO)
		{
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(gameSceneSO.sceneReference.editorAsset));
			}
		}

		public static Color GetDefaultColor(GameSceneSO gameScene)
		{
			Type type = gameScene.GetType();
			if (kDefaultMarkerColors.TryGetValue(type, out Color color))
			{
				return color;
			}

			return Color.red;
		}

		public static int FindAssetsByType<T>(List<T> assets) where T : UnityEngine.Object
		{
			int foundAssetsCount = 0;
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
			for (int i = 0, count = guids.Length; i < count; ++i)
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				T asset = AssetDatabase.LoadAssetAtPath<T>(path);
				if (asset != null)
				{
					assets.Add(asset);
					foundAssetsCount += 1;
				}
			}

			return foundAssetsCount;
		}
	}
}
