using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;

public class PocketItemValueController : ValueController
{
    // 制作者 田内
    // アイテム数を選択する

    protected PocketType m_pocketType = PocketType.Inventory;

    protected PocketItemData m_pocketItemData = null;

    //============================================
    //              実行処理
    //============================================

    public void SetData(PocketType _type, PocketItemData _data)
    {
        m_pocketType = _type;
        m_pocketItemData = _data;

        SetData();
    }

    protected override void SetData()
    {
        int itemNum = 0;

        if (m_pocketItemData != null)
        {
            itemNum = m_pocketItemData.Num;
        }

        // 最小作成数を更新
        if (itemNum <= 0) m_minValue = 0;
        else m_minValue = 1;

        // 最大作成数を更新
        m_maxValue = itemNum;

        // 現在選択中の値を更新
        if (m_maxValue < m_currentValue) m_currentValue = m_maxValue;
        if (m_currentValue < m_minValue) m_currentValue = m_minValue;

        // スライダーの値更新
        SetSliderValue();

        m_isSelectChangeFlg = true;

    }

    public override bool IsDecision()
    {
        if (m_currentValue <= 0 || m_currentValue < m_minValue || m_maxValue < m_currentValue)
        {
            return false;
        }

        return true;
    }
}
