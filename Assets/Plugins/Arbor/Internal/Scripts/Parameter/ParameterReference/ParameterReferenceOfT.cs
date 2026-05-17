using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ジェネリック型で値の型を指定可能なパラメータ参照用クラス。
	/// </summary>
	/// <typeparam name="T">値の型</typeparam>
#else
	/// <summary>
	/// A parameter reference class that allows you to specify the value type with a generic type.
	/// </summary>
	/// <typeparam name="T">Value type</typeparam>
#endif
	[System.Serializable]
	public sealed class ParameterReference<T> : ParameterReferenceBase<T>
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
		protected override T GetValue(Parameter parameter)
		{
			return parameter.GetValue<T>();
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
		protected override void SetValue(Parameter parameter, T value)
		{
			parameter.SetValue<T>(value);
		}
	}
}