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
	/// AssetObjectListパラメータの参照。
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// AssetObjectListパラメータの型はIList&lt;T&gt;として扱われているため <code>[SlotType(typeof(IList&lt;AudioClip&gt;))]</code>というように指定してください。
	/// </remarks>
#else
	/// <summary>
	/// Reference AssetObjectList parameters.
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// Since the type of AssetObjectList parameter is handled as IList&lt;T&gt;, specify it as <code>[SlotType(typeof(IList&lt;AudioClip&gt;))]</code>.
	/// </remarks>
#endif
	[System.Serializable]
	[Internal.Constraintable(typeof(Object), isList = true)]
	[Internal.ParameterType(Parameter.Type.AssetObjectList)]
	public sealed class AssetObjectListParameterReference : ParameterReferenceBase<IList<Object>>
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
		protected override IList<Object> GetValue(Parameter parameter)
		{
			return parameter.GetAssetObjectList<Object>();
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
		protected override void SetValue(Parameter parameter, IList<Object> value)
		{
			parameter.SetAssetObjectList(value);
		}
	}
}
