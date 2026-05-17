//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2IntListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector2IntList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector2IntList)]
	public sealed class Vector2IntListParameterReference : ParameterReferenceBase<IList<Vector2Int>>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Parameterから値を取得する。
		/// </summary>
		/// <param name="parameter">Parameter</param>
		/// <returns>取得した値</returns>
#else
		/// <summary>
		/// Get value from Parameter.
		/// </summary>
		/// <param name="parameter">Parameter</param>
		/// <returns>Get the value</returns>
#endif
		protected override IList<Vector2Int> GetValue(Parameter parameter)
		{
			return parameter.GetVector2IntList();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Parameterに値を設定する。
		/// </summary>
		/// <param name="parameter">Parameter</param>
		/// <param name="value">設定する値</param>
#else
		/// <summary>
		/// Set value to Parameter.
		/// </summary>
		/// <param name="parameter">Parameter</param>
		/// <param name="value">Value to set</param>
#endif
		protected override void SetValue(Parameter parameter, IList<Vector2Int> value)
		{
			parameter.SetVector2IntList(value);
		}
	}
}