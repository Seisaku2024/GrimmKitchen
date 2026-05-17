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
	/// ComponentListパラメータの参照。
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// ComponentListパラメータの型はIList&lt;T&gt;として扱われているため <code>[SlotType(typeof(IList&lt;Rigidbody&gt;))]</code>というように指定してください。
	/// </remarks>
#else
	/// <summary>
	/// Reference ComponentList parameters.
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// Since the type of ComponentList parameter is handled as IList&lt;T&gt;, specify it as <code>[SlotType(typeof(IList&lt;Rigidbody&gt;))]</code>.
	/// </remarks>
#endif
	[System.Serializable]
	[Internal.Constraintable(typeof(Component), isList = true)]
	public sealed class ComponentListParameterReference : ParameterReferenceBase<IList<Component>>
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
		protected override IList<Component> GetValue(Parameter parameter)
		{
			return parameter.GetComponentList<Component>();
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
		protected override void SetValue(Parameter parameter, IList<Component> value)
		{
			parameter.SetComponentList(value);
		}
	}
}
