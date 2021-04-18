using System;
using System.Collections.ObjectModel;
using System.Reflection;
using TMPro;
using UnityEditor.Localization;
using UnityEngine.Events;
using UnityEngine.Localization.Tables;

namespace UnityEngine.Localization.Components
{
	/// <summary>
	/// Component that can be used to Localize a TMP_FontAsset asset.
	/// </summary>
	[AddComponentMenu("Localization/Asset/Localize TMPro Font Event")]
	public class LocalizeTMProFontEvent : LocalizedAssetEvent<TMP_FontAsset, LocalizedTMProFont, UnityEventFont>
	{
		void Reset()
		{
			//Set up Unity Event automatically
			TextMeshProUGUI target = GetComponent<TextMeshProUGUI>();
			MethodInfo setFontMethod = target.GetType().GetProperty("font").GetSetMethod();
			UnityAction<TMP_FontAsset> methodDelegate =
				Delegate.CreateDelegate(typeof(UnityAction<TMP_FontAsset>), target, setFontMethod) as UnityAction<TMP_FontAsset>;
			UnityEditor.Events.UnityEventTools.AddPersistentListener(OnUpdateAsset, methodDelegate);

			//Set up font localize asset table automatically
			ReadOnlyCollection<AssetTableCollection> collections = LocalizationEditorSettings.GetAssetTableCollections();
			foreach (AssetTableCollection tableCollection in collections)
			{
				if (tableCollection.name == "Fonts")
				{
					AssetReference.TableReference = tableCollection.TableCollectionNameReference;
					foreach (SharedTableData.SharedTableEntry entry in tableCollection.SharedData.Entries)
					{
						if (entry.Key == "font")
						{
							AssetReference.TableEntryReference = entry.Id;
						}
					}
				}
			}
		}
	}

	[Serializable]
	public class LocalizedTMProFont : LocalizedAsset<TMP_FontAsset>
	{
	}

	[Serializable]
	public class UnityEventFont : UnityEvent<TMP_FontAsset>
	{
	}
}
