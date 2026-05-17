//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Vector4パラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector4 parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector4)]
	public sealed class Vector4ParameterReference : ParameterReferenceBase<Vector4>
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
		protected override Vector4 GetValue(Parameter parameter)
		{
			return parameter.GetVector4();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// デフォルトの値を取得する。パラメータが未参照の場合に使用される。
		/// </summary>
		/// <returns>デフォルトの値</returns>
#else
		/// <summary>
		/// Get the default value. Used when the parameter is unreferenced.
		/// </summary>
		/// <returns>Default value</returns>
#endif
		protected override Vector4 GetDefault()
		{
			return Vector4.zero;
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
		protected override void SetValue(Parameter parameter, Vector4 value)
		{
			parameter.SetVector4(value);
		}
	}
}
