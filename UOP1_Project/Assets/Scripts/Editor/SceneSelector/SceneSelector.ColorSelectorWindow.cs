using UnityEditor;
using UnityEngine;
using SceneSelectorInternal;

public partial class SceneSelector : EditorWindow
{
	class ColorSelectorWindow : EditorWindow
	{
		static readonly float kCellSize = PreferencesWindow.kColorMarkerFieldSize * 2.0f;
		static readonly Color kCellBackColor = new Color(0.0f, 0.0f, 0.0f, 0.1f);
		static readonly Vector2 kCellOffset = new Vector2(1.0f, 1.0f);
		static readonly Vector2Int kCount = new Vector2Int(5, 5);

		PreferencesWindow _owner;
		Color[,] _colors;
		Item _item;

		public static ColorSelectorWindow Open(Rect rect, PreferencesWindow owner, Item item)
		{
			ColorSelectorWindow window = CreateInstance<ColorSelectorWindow>();
			window.Init(rect, owner, item);
			return window;
		}

		void Init(Rect rect, PreferencesWindow owner, Item item)
		{
			Vector2 size = (Vector2)kCount * kCellSize;
			ShowAsDropDown(rect, size);
			_owner = owner;
			_item = item;
		}

		void OnEnable()
		{
			wantsMouseMove = true;
			InitColors();
		}

		void OnGUI()
		{
			Helper.RepaintOnMouseMove(this);
			DrawMarkers();
		}

		void DrawMarkers()
		{
			Vector2 size = new Vector2(kCellSize, kCellSize);
			for (int x = 0; x < kCount.x; ++x)
			{
				for (int y = 0; y < kCount.y; ++y)
				{
					Color color = _colors[x, y];
					Vector2 position = size * new Vector2(x, y);
					Rect rect = new Rect(position, size);
					{
						Rect cellBackRect = rect;
						cellBackRect.position += kCellOffset;
						cellBackRect.size -= kCellOffset * 2.0f;
						EditorGUI.DrawRect(cellBackRect, kCellBackColor);
					}
					if (Helper.DrawColorMarker(rect, color, true, true))
					{
						_item.color = color;
						_owner.RepaintAll();
						Close();
					}
				}
			}
		}

		void InitColors()
		{
			int count = kCount.x * kCount.y;
			_colors = new Color[kCount.x, kCount.y];
			for (int x = 0; x < kCount.x; ++x)
			{
				int h = x * kCount.y;
				for (int y = 0; y < kCount.y; ++y)
				{
					float hue = (float)(h + y) / count;
					_colors[x, y] = Color.HSVToRGB(hue, 1.0f, 1.0f);
				}
			}
		}
	}
}
