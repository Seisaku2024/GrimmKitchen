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
	/// RectIntListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference RectIntList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.RectIntList)]
	public sealed class RectIntListParameterReference : ParameterReferenceBase<IList<RectInt>>
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
		protected override IList<RectInt> GetValue(Parameter parameter)
		{
			return parameter.GetRectIntList();
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
		protected override void SetValue(Parameter parameter, IList<RectInt> value)
		{
			parameter.SetRectIntList(value);
		}
	}
}