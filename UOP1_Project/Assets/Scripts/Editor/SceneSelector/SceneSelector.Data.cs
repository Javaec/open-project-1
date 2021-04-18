using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SceneSelector : EditorWindow
{
	[Serializable]
	class Item
	{
		public string guid;
		public int order = int.MaxValue;
		public bool isVisible = true;
		public Color color = Color.clear;

		[NonSerialized] public GameSceneSO gameSceneSO;
	}

	class Styles
	{
		public GUIStyle item;
	}

	class Textures
	{
		public Texture2D visibilityOn;
		public Texture2D visibilityOff;
	}

	[Serializable]
	class Storage : ISerializationCallbackReceiver
	{
		[SerializeField] public List<Item> items = new List<Item>();

		[NonSerialized] public Dictionary<string, Item> itemsMap = new Dictionary<string, Item>();

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			for (int i = 0, count = items.Count; i < count; ++i)
			{
				Item item = items[i];
				item.order = i;
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			items.OrderBy(x => x.order);
			foreach (Item item in items)
			{
				itemsMap.Add(item.guid, item);
			}
		}
	}
}
