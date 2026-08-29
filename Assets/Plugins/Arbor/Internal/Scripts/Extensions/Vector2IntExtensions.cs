//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.Extensions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2Intの拡張クラス
	/// </summary>
#else
	/// <summary>
	/// Vector2Int extension class
	/// </summary>
#endif
	public static class Vector2IntExtensions
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Intを整数で除算する。
		/// </summary>
		/// <param name="v">被除数のVector2Int</param>
		/// <param name="i">除数の整数</param>
		/// <returns>除算したVector2Intを返す。</returns>
#else
		/// <summary>
		/// Divide Vector2Int by an integer.
		/// </summary>
		/// <param name="v">Vector2Int of the divisor</param>
		/// <param name="i">Integer for the divisor</param>
		/// <returns>Returns the divided Vector2Int.</returns>
#endif
		[Obsolete("use `v / i`")]
		// The minimum supported Unity version is now 6.0, so Vector2Int.operator/ is recommended.
		public static Vector2Int Div(this Vector2Int v, int i)
		{
			return v / i;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Intの各成分の符号を反転する
		/// </summary>
		/// <param name="v">各成分の符号を反転するVector2Int</param>
		/// <returns>各成分の符号を反転した結果を返す。</returns>
#else
		/// <summary>
		/// Invert the sign of each component of Vector2Int.
		/// </summary>
		/// <param name="v">Vector2Int to reverse the sign of each component</param>
		/// <returns>Returns the result of reversing the sign of each component.</returns>
#endif
		[Obsolete("use `-v`")]
		// The minimum supported Unity version is now 6.0, so Vector2Int.operator- is recommended.
		public static Vector2Int Negative(this Vector2Int v)
		{
			return new Vector2Int(-v.x, -v.y);
		}
	}
}