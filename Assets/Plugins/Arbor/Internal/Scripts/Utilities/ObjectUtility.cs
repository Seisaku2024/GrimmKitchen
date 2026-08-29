using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Unityオブジェクトのユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// Utility class for Unity objects
	/// </summary>
#endif
	public static class ObjectUtility
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Object.FindObjectOfTypeを呼び出す。<br />
		/// Unity2023.1以降の場合はObject.FindFirstObjectByTypeを呼び出す。
		/// </summary>
		/// <typeparam name="T">Objectの型</typeparam>
		/// <returns>見つかったオブジェクト</returns>
#else
		/// <summary>
		/// Call Object.FindObjectOfType.<br />
		/// For Unity2023.1 or later, call Object.FindFirstObjectByType.
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <returns>the object found</returns>
#endif
		[System.Obsolete("use Object.FindAnyObjectByType")] // The minimum supported Unity version is now 6.0, so direct calls to Object-type methods are recommended.
		public static T FindObjectOfType<T>() where T : Object
		{
			return Object.FindAnyObjectByType<T>();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Object.FindObjectsOfTypeを呼び出す。<br />
		/// Unity2023.1以降の場合はObject.FindObjectsByTypeを呼び出す。
		/// </summary>
		/// <typeparam name="T">Objectの型</typeparam>
		/// <returns>見つかったオブジェクトの配列</returns>
#else
		/// <summary>
		/// Call Object.FindObjectsOfType. <br />
		/// For Unity2023.1 or later, call Object.FindObjectsByType.
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <returns>an array of found objects</returns>
#endif
		[System.Obsolete("use Object.FindObjectsByType")] // The minimum supported Unity version is now 6.0, so direct calls to Object-type methods are recommended.
		public static T[] FindObjectsOfType<T>() where T : Object
		{
			return FindObjectsByTypeInternal<T>();
		}

		internal static T[] FindObjectsByTypeInternal<T>() where T : Object
		{
#if UNITY_6000_4_OR_NEWER
			return Object.FindObjectsByType<T>();
#else
			return Object.FindObjectsByType<T>(FindObjectsSortMode.InstanceID);
#endif
		}
	}
}