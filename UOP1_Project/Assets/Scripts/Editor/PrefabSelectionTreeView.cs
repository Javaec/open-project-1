using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UOP1.EditorTools.Replacer
{
	class PrefabSelectionTreeView : TreeView
	{
		static Texture2D prefabOnIcon = EditorGUIUtility.IconContent("Prefab On Icon").image as Texture2D;
		static Texture2D prefabVariantOnIcon = EditorGUIUtility.IconContent("PrefabVariant On Icon").image as Texture2D;
		static Texture2D folderIcon = EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;
		static Texture2D folderOnIcon = EditorGUIUtility.IconContent("Folder On Icon").image as Texture2D;

		static GUIStyle whiteLabel;
		static GUIStyle foldout;

		public int RowsCount
		{
			get
			{
				return rows.Count;
			}
		}

		Event evt
		{
			get
			{
				return Event.current;
			}
		}

		public Action<GameObject> onSelectEntry;

		List<TreeViewItem> rows = new List<TreeViewItem>();
		HashSet<string> paths = new HashSet<string>();

		Dictionary<int, RenderTexture> previewCache = new Dictionary<int, RenderTexture>();
		HashSet<int> renderableItems = new HashSet<int>();

		GameObjectPreview itemPreview = new GameObjectPreview();
		GUIContent itemContent = new GUIContent();

		int selectedId;

		public PrefabSelectionTreeView(TreeViewState state) : base(state)
		{
			foldoutOverride = FoldoutOverride;
			Reload();
		}

		bool FoldoutOverride(Rect position, bool expandedState, GUIStyle style)
		{
			position.width = Screen.width;
			position.height = 20;
			position.y -= 2;

			expandedState = GUI.Toggle(position, expandedState, GUIContent.none, style);

			return expandedState;
		}

		public void Cleanup()
		{
			foreach (RenderTexture texture in previewCache.Values)
			{
				Object.DestroyImmediate(texture);
			}
		}

		public bool IsRenderable(int id)
		{
			return renderableItems.Contains(id);
		}

		void CachePreview(int itemId)
		{
			RenderTexture copy = new RenderTexture(itemPreview.outputTexture);
			RenderTexture previous = RenderTexture.active;
			Graphics.Blit(itemPreview.outputTexture, copy);
			RenderTexture.active = previous;
			previewCache.Add(itemId, copy);
		}

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return false;
		}

		bool IsPrefabAsset(int id, out GameObject prefab)
		{
			Object obj = EditorUtility.InstanceIDToObject(id);

			if (obj is GameObject go)
			{
				prefab = go;
				return true;
			}

			prefab = null;
			return false;
		}

		protected override void DoubleClickedItem(int id)
		{
			if (IsPrefabAsset(id, out GameObject prefab))
			{
				onSelectEntry(prefab);
			}
			else
			{
				SetExpanded(id, !IsExpanded(id));
			}
		}

		protected override void KeyEvent()
		{
			KeyCode key = evt.keyCode;
			if (key == KeyCode.KeypadEnter || key == KeyCode.Return)
			{
				DoubleClickedItem(selectedId);
			}
		}

		protected override void SelectionChanged(IList<int> selectedIds)
		{
			if (selectedIds.Count > 0)
			{
				selectedId = selectedIds[0];
			}
		}

		protected override TreeViewItem BuildRoot()
		{
			TreeViewItem root = new TreeViewItem(0, -1);
			rows.Clear();
			paths.Clear();

			foreach (string guid in AssetDatabase.FindAssets("t:Prefab"))
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				string[] splits = path.Split('/');
				int depth = splits.Length - 2;

				if (splits[0] != "Assets")
				{
					break;
				}

				GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);

				AddFoldersItems(splits);
				AddPrefabItem(asset, depth);
			}

			SetupParentsAndChildrenFromDepths(root, rows);

			return root;
		}

		protected override float GetCustomRowHeight(int row, TreeViewItem item)
		{
			// Hide folders during search
			if (!IsPrefabAsset(item.id, out _) && hasSearch)
			{
				return 0;
			}

			return 20;
		}

		public override void OnGUI(Rect rect)
		{
			if (whiteLabel == null)
			{
				whiteLabel = new GUIStyle(EditorStyles.label) {normal = {textColor = EditorStyles.whiteLabel.normal.textColor}};
			}

			base.OnGUI(rect);
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			Rect rect = args.rowRect;
			TreeViewItem item = args.item;

			bool isRenderable = IsRenderable(item.id);
			bool isSelected = IsSelected(item.id);
			bool isFocused = HasFocus() && isSelected;
			bool isPrefab = IsPrefabAsset(item.id, out GameObject prefab);
			bool isFolder = !isPrefab;

			if (isFolder && hasSearch)
			{
				return;
			}

			if (isFolder)
			{
				if (rect.Contains(evt.mousePosition) && evt.type == EventType.MouseUp)
				{
					SetSelection(new List<int> {item.id});
					SetFocus();
				}
			}

			GUIStyle labelStyle = isFocused ? whiteLabel : EditorStyles.label;
			float contentIndent = GetContentIndent(item);

			customFoldoutYOffset = 2;
			itemContent.text = item.displayName;

			rect.x += contentIndent;
			rect.width -= contentIndent;

			Rect iconRect = new Rect(rect) {width = 20};

			if (isPrefab)
			{
				PrefabAssetType type = PrefabUtility.GetPrefabAssetType(prefab);
				Texture2D onIcon = type == PrefabAssetType.Regular ? prefabOnIcon : prefabVariantOnIcon;

				Rect labelRect = new Rect(rect);

				if (isRenderable)
				{
					Rect previewRect = new Rect(rect) {width = 32, height = 32};

					if (!previewCache.TryGetValue(item.id, out RenderTexture previewTexture))
					{
						itemPreview.CreatePreviewForTarget(prefab);
						itemPreview.RenderInteractivePreview(previewRect);

						if (itemPreview.outputTexture)
						{
							CachePreview(item.id);
						}
					}

					if (!previewTexture)
					{
						Repaint();
					}
					else
					{
						GUI.DrawTexture(iconRect, previewTexture, ScaleMode.ScaleAndCrop);
					}

					labelRect.x += iconRect.width;
					labelRect.width -= iconRect.width + 24;

					GUI.Label(labelRect, args.label, labelStyle);

					if (isSelected)
					{
						Rect prefabIconRect = new Rect(iconRect) {x = rect.xMax - 24};
						GUI.Label(prefabIconRect, isFocused ? onIcon : item.icon);
					}
				}
				else
				{
					itemContent.image = isSelected ? onIcon : item.icon;
					GUI.Label(rect, itemContent, labelStyle);
				}
			}
			else
			{
				itemContent.image = isFocused ? folderOnIcon : folderIcon;
				GUI.Label(rect, itemContent, labelStyle);
			}
		}

		void AddFoldersItems(string[] splits)
		{
			for (int i = 1; i < splits.Length - 1; i++)
			{
				string split = splits[i];

				if (!paths.Contains(split))
				{
					rows.Add(new TreeViewItem(split.GetHashCode(), i - 1, " " + split) {icon = folderIcon});
					paths.Add(split);
				}
			}
		}

		void AddPrefabItem(GameObject asset, int depth)
		{
			int id = asset.GetInstanceID();
			GUIContent content = new GUIContent(EditorGUIUtility.ObjectContent(asset, asset.GetType()));

			if (GameObjectPreview.HasRenderableParts(asset))
			{
				renderableItems.Add(id);
			}

			rows.Add(new TreeViewItem(id, depth, content.text)
			{
				icon = content.image as Texture2D
			});
		}
	}
}
