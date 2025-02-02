using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UOP1.EditorTools
{
	class GameObjectPreview
	{
		static Type gameObjectInspectorType;
		static MethodInfo getPreviewDataMethod;
		static FieldInfo renderUtilityField;

		Rect renderRect;
		Color light0Color;
		Color light1Color;
		PreviewRenderUtility renderUtility;

		Editor cachedEditor;

		public RenderTexture outputTexture;

		[InitializeOnLoadMethod]
		static void OnInitialize()
		{
			gameObjectInspectorType = typeof(Editor).Assembly.GetType("UnityEditor.GameObjectInspector");
			Type previewDataType = gameObjectInspectorType.GetNestedType("PreviewData", BindingFlags.NonPublic);

			getPreviewDataMethod = gameObjectInspectorType.GetMethod("GetPreviewData", BindingFlags.NonPublic | BindingFlags.Instance);
			renderUtilityField = previewDataType.GetField("renderUtility", BindingFlags.Public | BindingFlags.Instance);
		}

		public void CreatePreviewForTarget(GameObject target)
		{
			if (!cachedEditor || cachedEditor.target != target)
			{
				renderUtility = null;
				// There is a bug that breaks previews and Prefab mode after creating too many editors.
				// Simply using CreateCachedEditor is fixing that problem.
				Editor.CreateCachedEditor(target, gameObjectInspectorType, ref cachedEditor);
			}
		}

		public void RenderInteractivePreview(Rect rect)
		{
			if (!cachedEditor)
			{
				return;
			}

			if (renderUtility == null || renderUtility.lights[0] == null)
			{
				object previewData = getPreviewDataMethod.Invoke(cachedEditor, null);
				renderUtility = renderUtilityField.GetValue(previewData) as PreviewRenderUtility;

				light0Color = renderUtility.lights[0].color;
				light1Color = renderUtility.lights[1].color;
			}

			renderUtility.lights[0].color = light0Color * 1.6f;
			renderUtility.lights[1].color = light1Color * 6f;
			Color backColor = renderUtility.camera.backgroundColor;
			renderUtility.camera.backgroundColor = new Color(backColor.r, backColor.g, backColor.b, 0);
			renderUtility.camera.clearFlags = CameraClearFlags.Depth;

			Color color = GUI.color;

			// Hide default preview texture, since it has no alpha blending
			GUI.color = new Color(1, 1, 1, 0);

			cachedEditor.OnPreviewGUI(rect, null);

			GUI.color = color;

			outputTexture = renderUtility.camera.targetTexture;
		}

		public void DrawPreviewTexture(Rect rect)
		{
			GUI.DrawTexture(rect, outputTexture, ScaleMode.ScaleToFit, true, 0);
		}

		public static bool HasRenderableParts(GameObject go)
		{
			Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers)
			{
				switch (renderer)
				{
					case MeshRenderer _:
						MeshFilter filter = renderer.gameObject.GetComponent<MeshFilter>();
						if (filter && filter.sharedMesh)
						{
							return true;
						}

						break;
					case SkinnedMeshRenderer skinnedMesh:
						if (skinnedMesh.sharedMesh)
						{
							return true;
						}

						break;
					case SpriteRenderer sprite:
						if (sprite.sprite)
						{
							return true;
						}

						break;
					case BillboardRenderer billboard:
						if (billboard.billboard && billboard.sharedMaterial)
						{
							return true;
						}

						break;
				}
			}

			return false;
		}
	}
}
