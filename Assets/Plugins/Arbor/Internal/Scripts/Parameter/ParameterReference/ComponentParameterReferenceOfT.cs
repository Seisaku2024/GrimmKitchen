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
	/// Componentパラメータの参照(ジェネリック)。
	/// </summary>
	/// <typeparam name="T">参照するコンポーネントの型</typeparam>
#else
	/// <summary>
	/// Reference Component parameters(Generic).
	/// </summary>
	/// <typeparam name="T">Type of component to reference</typeparam>
#endif
	public class ComponentParameterReference<T> : ParameterReferenceBase<T> where T : Component
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
			return parameter.GetComponent<T>();
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
			parameter.SetComponent<T>(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="ComponentParameterReference{T}"/>を<see cref="ComponentParameterReference"/>にキャスト。
		/// </summary>
		/// <param name="parameterReference"><see cref="ComponentParameterReference{T}"/></param>
		/// <returns>ComponentParameterReferenceにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast <see cref="ComponentParameterReference{T}"/> to <see cref="ComponentParameterReference"/>.
		/// </summary>
		/// <param name="parameterReference"><see cref="ComponentParameterReference{T}"/></param>
		/// <returns>Returns the result of casting to ComponentParameterReference.</returns>
#endif
		public static explicit operator ComponentParameterReference(ComponentParameterReference<T> parameterReference)
		{
			ComponentParameterReference componentParameterReference = new ComponentParameterReference();
			componentParameterReference.Copy(parameterReference);
			return componentParameterReference;
		}
	}
}