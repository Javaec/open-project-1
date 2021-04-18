using System.Collections.ObjectModel;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;

namespace UnityEditor.Localization.Plugins.TMPro
{
	static class LocalizeComponent_TMProFont
	{
		[MenuItem("CONTEXT/TextMeshProUGUI/Localize String And Font")]
		static void LocalizeTMProText(MenuCommand command)
		{
			TextMeshProUGUI target = command.context as TextMeshProUGUI;
			SetupForStringLocalization(target);
			SetupForFontLocalization(target);
		}

		public static MonoBehaviour SetupForStringLocalization(TextMeshProUGUI target)
		{
			//Avoid adding the component multiple times
			if (target.GetComponent<LocalizeStringEvent>() != null)
			{
				return null;
			}

			LocalizeStringEvent comp = Undo.AddComponent(target.gameObject, typeof(LocalizeStringEvent)) as LocalizeStringEvent;
			MethodInfo setStringMethod = target.GetType().GetProperty("text").GetSetMethod();
			UnityAction<string> methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction<string>), target, setStringMethod) as UnityAction<string>;
			Events.UnityEventTools.AddPersistentListener(comp.OnUpdateString, methodDelegate);

			const int kMatchThreshold = 5;
			(StringTableCollection collection, SharedTableData.SharedTableEntry entry, int matchDistance) foundKey =
				LocalizationEditorSettings.FindSimilarKey(target.text);
			if (foundKey.collection != null && foundKey.matchDistance < kMatchThreshold)
			{
				comp.StringReference.TableEntryReference = foundKey.entry.Id;
				comp.StringReference.TableReference = foundKey.collection.TableCollectionNameReference;
			}

			return null;
		}

		public static MonoBehaviour SetupForFontLocalization(TextMeshProUGUI target)
		{
			//Avoid adding the component multiple times
			if (target.GetComponent<LocalizeTMProFontEvent>() != null)
			{
				return null;
			}

			LocalizeTMProFontEvent comp = Undo.AddComponent(target.gameObject, typeof(LocalizeTMProFontEvent)) as LocalizeTMProFontEvent;
			MethodInfo setFontMethod = target.GetType().GetProperty("font").GetSetMethod();
			UnityAction<TMP_FontAsset> methodDelegate =
				System.Delegate.CreateDelegate(typeof(UnityAction<TMP_FontAsset>), target, setFontMethod) as UnityAction<TMP_FontAsset>;
			Events.UnityEventTools.AddPersistentListener(comp.OnUpdateAsset, methodDelegate);

			//Find font table and set it up automatically
			ReadOnlyCollection<AssetTableCollection> collections = LocalizationEditorSettings.GetAssetTableCollections();
			foreach (AssetTableCollection tableCollection in collections)
			{
				if (tableCollection.name == "Fonts")
				{
					comp.AssetReference.TableReference = tableCollection.TableCollectionNameReference;
					foreach (SharedTableData.SharedTableEntry entry in tableCollection.SharedData.Entries)
					{
						if (entry.Key == "font")
						{
							comp.AssetReference.TableEntryReference = entry.Id;
						}
					}
				}
			}

			return null;
		}
	}
}
