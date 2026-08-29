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
	/// RectIntの拡張クラス
	/// </summary>
#else
	/// <summary>
	/// RectInt extension class
	/// </summary>
#endif
	public static class RectIntExtensions
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt(0, 0, 0, 0)を返す。
		/// </summary>
#else
		/// <summary>
		/// Returns RectInt (0, 0, 0, 0).
		/// </summary>
#endif
		[Obsolete("use RectInt.zero")]
		// The minimum supported Unity version is now 6.0, so RectInt.zero is recommended.
		public static RectInt zero
		{
			get
			{
				return new RectInt(0, 0, 0, 0);
			}
		}
	}
}