using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// ジェネリック型で値の型を指定可能なパラメータの参照の基本クラス。
	/// </summary>
	/// <typeparam name="T">値の型</typeparam>
#else
	/// <summary>
	/// Base class for parameter references that allow you to specify a value type in a generic type.
	/// </summary>
	/// <typeparam name="T">Value type</typeparam>
#endif
	[System.Serializable]
	public abstract class ParameterReferenceBase<T> : ParameterReference, IValueContainer<T>
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
		protected abstract T GetValue(Parameter parameter);

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
		protected abstract void SetValue(Parameter parameter, T value);

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
		protected virtual T GetDefault()
		{
			return default(T);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値。
		/// </summary>
#else
		/// <summary>
		/// Value of the parameter
		/// </summary>
#endif
		public T value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return GetValue(parameter);
				}

				return GetDefault();
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					SetValue(parameter, value);
				}
			}
		}

		T IValueGetter<T>.GetValue()
		{
			return value;
		}

		void IValueSetter<T>.SetValue(T value)
		{
			this.value = value;
		}
	}
}