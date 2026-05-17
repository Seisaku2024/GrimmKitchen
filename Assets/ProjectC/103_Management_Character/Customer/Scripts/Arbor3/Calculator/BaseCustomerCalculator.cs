using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class BaseCustomerCalculator : Calculator 
{
	// 制作者　田内

	//======================================
	// 客情報

	[Header("客情報")]
	[SerializeField]
	protected FlexibleCustomerDataVariable m_flexibleCustomerDataVariable = null;


	//=========================================
	// 出力

	[Header("出力:座標")]
	[SerializeField]
	protected OutputSlotVector3 m_outputPos = new();


	#region メソッド説明
	/// <summary>
	/// 客情報が存在するかのチェック
	/// </summary>
	#endregion
	protected CustomerData GetCustomerData()
	{

		// 客情報が存在するか確認

		if (m_flexibleCustomerDataVariable == null)
		{
			Debug.LogError("客情報がシリアライズされていません");
			return null;
		}

		if (m_flexibleCustomerDataVariable.value.CustomerData == null)
		{
			Debug.LogError("客情報が存在しません");
			return null;
		}

		// 存在する
		return m_flexibleCustomerDataVariable.value.CustomerData;

	}


	// 客オブジェクトを取得する
	protected GameObject GetCustomerGameObject()
	{
		var data = GetCustomerData();
		if (data == null) return null;

		// 客のゲームオブジェクトを取得
		return data.gameObject;
	}

}
