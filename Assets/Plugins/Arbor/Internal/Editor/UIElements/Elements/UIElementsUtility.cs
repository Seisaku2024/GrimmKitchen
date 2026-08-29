//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	internal static class UIElementsUtility
	{
		static UnityEngine.TextCore.Text.FontAsset s_BoldFont;

		public static bool IsVisible(VisualElement target)
		{
			while (target != null)
			{
				if (target.resolvedStyle.display == DisplayStyle.None)
				{
					return false;
				}

				target = target.parent;
			}

			return true;
		}

		public static VisualElement GetFirstAncestorWithClass(VisualElement element, string className)
		{
			if (element == null)
				return null;

			if (element.ClassListContains(className))
				return element;

			return GetFirstAncestorWithClass(element.parent, className);
		}

		[System.Obsolete("use style.transformOrigin")] // The minimum supported Unity version is now 6.0, so direct calling is recommended.
		public static void SetTransformOrigin(VisualElement element, float x, float y, float z)
		{
			element.style.transformOrigin = new TransformOrigin(x, y, z);
		}

		[System.Obsolete("use style.textOverflow = TextOverflow.Ellipsis")] // The minimum supported Unity version is now 6.0, so direct calling is recommended.
		public static void SetEllipsis(VisualElement element)
		{
			element.style.textOverflow = TextOverflow.Ellipsis;
		}

		public static void SetBoldFont(VisualElement element)
		{
			if (s_BoldFont == null)
			{
				s_BoldFont = EditorGUIUtility.Load("UIPackageResources/Fonts/Inter/Inter-SemiBold SDF.asset") as UnityEngine.TextCore.Text.FontAsset;
			}

			element.style.unityFontDefinition = FontDefinition.FromSDFFont(s_BoldFont);
		}

		public static Vector3 GetTransformPosition(VisualElement element)
		{
#if UNITY_6000_3_OR_NEWER
			return element.resolvedStyle.translate;
#else
			return element.transform.position;
#endif
		}

		public static void SetTransformPosition(VisualElement element, Vector3 position)
		{
#if UNITY_6000_3_OR_NEWER
			element.style.translate = position;
#else
			element.transform.position = position;
#endif
		}

		public static Quaternion GetTrasformRotation(VisualElement element)
		{
#if UNITY_6000_3_OR_NEWER
			var rotate = element.resolvedStyle.rotate;
			return Quaternion.AngleAxis(rotate.angle.ToDegrees(), Vector3.forward);
#else
			return element.transform.rotation;
#endif
		}

		public static void SetTransformRotation(VisualElement element, Quaternion rotation)
		{
#if UNITY_6000_3_OR_NEWER
			element.style.rotate = rotation;
#else
			element.transform.rotation = rotation;
#endif
		}

		public static Vector3 GetTransformScale(VisualElement element)
		{
#if UNITY_6000_3_OR_NEWER
			return element.resolvedStyle.scale.value;
#else
			return element.transform.scale;
#endif
		}

		public static void SetTrasnformScale(VisualElement element, Vector3 scale)
		{
#if UNITY_6000_3_OR_NEWER
			element.style.scale = scale;
#else
			element.transform.scale = scale;
#endif
		}
	}
}