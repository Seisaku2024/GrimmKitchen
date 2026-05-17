//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なリスト型を扱うクラス。
	/// 使用する場合は、Tにリストの要素の型を指定して継承してください。
	/// </summary>
	/// <typeparam name="T">シリアライズ可能なリストの要素の型</typeparam>
#else
	/// <summary>
	/// A class that handles flexible list types that have multiple reference methods.
	/// If you want to use it, specify the type of the list element in T and inherit it.
	/// </summary>
	/// <typeparam name="T">Serializable list element type</typeparam>
#endif
	[System.Serializable]
	public class FlexibleList<T> : FlexibleFieldBase, IValueGetter<IList<T>>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 固定値
		/// </summary>
#else
		/// <summary>
		/// Constant value
		/// </summary>
#endif
		[SerializeField]
		protected List<T> _Value = new List<T>();

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータ参照
		/// </summary>
#else
		/// <summary>
		/// Parameter reference
		/// </summary>
#endif
		[SerializeField]
		[ClassGenericArgumentList(0)]
		protected AnyParameterReference _Parameter = new AnyParameterReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// データ入力スロット
		/// </summary>
#else
		/// <summary>
		/// Data input slot
		/// </summary>
#endif
		[SerializeField]
		[ClassGenericArgumentList(0)]
		protected InputSlotAny _Slot = new InputSlotAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドの型を返す。
		/// </summary>
#else
		/// <summary>
		/// It returns a field type.
		/// </summary>
#endif
		public override System.Type fieldType
		{
			get
			{
				return typeof(IList<T>);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Parameterを返す。TypeがParameter以外の場合はnull。
		/// </summary>
#else
		/// <summary>
		/// It return a Paramter. It is null if Type is other than Parameter.
		/// </summary>
#endif
		public Parameter parameter
		{
			get
			{
				if (_Type == FlexibleType.Parameter)
				{
					return _Parameter.parameter;
				}
				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を返す
		/// </summary>
#else
		/// <summary>
		/// It returns a value
		/// </summary>
#endif
		public IList<T> value
		{
			get
			{
				IList<T> value = null;
				switch (_Type)
				{
					case FlexibleType.Constant:
						value = _Value;
						break;
					case FlexibleType.Parameter:
						try
						{
							if (_Parameter != null)
							{
								value = _Parameter.GetValue<IList<T>>();
							}
						}
						catch (System.InvalidCastException ex)
						{
							Debug.LogException(ex);
						}
						break;
					case FlexibleType.DataSlot:
						_Slot.TryGetValue<IList<T>>(out value);
						break;
				}

				return value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値をobjectで返す。
		/// </summary>
		/// <returns>値のobject</returns>
#else
		/// <summary>
		/// Return the value as object.
		/// </summary>
		/// <returns>The value object</returns>
#endif
		public override object GetValueObject()
		{
			return value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleListデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleList default constructor
		/// </summary>
#endif
		public FlexibleList()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleListコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleList constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleList(IList<T> value)
		{
			_Type = FlexibleType.Constant;
			_Value = value?.ToList() ?? new List<T>();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleListコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleList constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleList(AnyParameterReference parameter)
		{
			_Type = FlexibleType.Parameter;
			_Parameter = parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleListコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleList constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleList(InputSlotAny slot)
		{
			_Type = FlexibleType.DataSlot;
			_Slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// データスロットの接続を切断する。
		/// </summary>
#else
		/// <summary>
		/// Disconnect the data slot.
		/// </summary>
#endif
		public override void Disconnect()
		{
			switch (_Type)
			{
				case FlexibleType.Parameter:
					_Parameter.Disconnect();
					break;
				case FlexibleType.DataSlot:
					_Slot.Disconnect();
					break;
			}
		}

		internal void SetSlot(InputSlotBase slot)
		{
			_Type = FlexibleType.DataSlot;
			_Slot.Copy(slot);
		}

		IList<T> IValueGetter<IList<T>>.GetValue()
		{
			return value;
		}
	}
}