using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
	internal static class ValueSafeUtility<T>
	{
		public static readonly bool isValueType = TypeUtility.IsValueType(typeof(T));

		public static bool IsNull(T value)
		{
			return !isValueType
				&& (value is Object obj && obj == null || value == null);
		}

		public static string ToString(T value)
		{
			if (IsNull(value))
			{
				return "null";
			}
			return value.ToString();
		}
	}
}