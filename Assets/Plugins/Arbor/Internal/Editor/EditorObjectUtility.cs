using Arbor;
using UnityEditor;
using UnityEngine;

namespace ArborEditor
{
	internal static class EditorObjectUtility
	{
		public static Object IdToObject(ObjectId id)
		{
#if UNITY_6000_3_OR_NEWER
			return EditorUtility.EntityIdToObject(id._EntityId);
#else
			return EditorUtility.InstanceIDToObject(id._InstanceId);
#endif
		}
	}
}