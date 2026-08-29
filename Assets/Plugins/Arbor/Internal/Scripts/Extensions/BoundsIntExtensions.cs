//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.Extensions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntの拡張クラス
	/// </summary>
#else
	/// <summary>
	/// BoundsInt extension class
	/// </summary>
#endif
	public static class BoundsIntExtensions
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt(0, 0, 0, 0, 0, 0)を返す。
		/// </summary>
#else
		/// <summary>
		/// Returns BoundsInt (0, 0, 0, 0, 0, 0).
		/// </summary>
#endif
		public static BoundsInt zero
		{
			get
			{
				return new BoundsInt(0, 0, 0, 0, 0, 0);
			}
		}
	}
}