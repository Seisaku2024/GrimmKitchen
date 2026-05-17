using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuSelectUIController : SelectUIController
{
    // 制作者 田内
    // セーブデータの有無による初期UIセット


    [Header("セーブデータがある状態の初期UI")]
    [SerializeField]
    private GameObject m_save = null;

    [Header("セーブデータが無い状態の初期UI")]
    [SerializeField]
    private GameObject m_unSave = null;

    //===============================
    //          実行処理
    //===============================

    // 初期選択UIをセット
    override public void SetHeadUIGameObject()
    {
        // 既にセットされていれば
        if (m_currentSelectUIData != null) return;

        if (SaveManager.instance.IsSave())
        {
            foreach (var uiList in m_uiList)
            {
                foreach (var uiData in uiList.List)
                {
                    if (uiData.UI != m_save) continue;

                    // UIをセット
                    m_currentSelectUIData = uiData;

                    m_currentHeight = m_uiList.IndexOf(uiList);
                    m_currentWidth = uiList.List.IndexOf(uiData);

                    ButtonDataSelectUI();
                    ButtonDataUnselectUI();

                    // 変更
                    m_isSelectChangeFlg = true;

                    return;
                }

            }
        }
        else
        {
            foreach (var uiList in m_uiList)
            {
                foreach (var uiData in uiList.List)
                {
                    if (uiData.UI != m_unSave) continue;

                    // UIをセット
                    m_currentSelectUIData = uiData;

                    m_currentHeight = m_uiList.IndexOf(uiList);
                    m_currentWidth = uiList.List.IndexOf(uiData);

                    ButtonDataSelectUI();
                    ButtonDataUnselectUI();

                    // 変更
                    m_isSelectChangeFlg = true;

                    return;
                }
            }
        }

        base.SetHeadUIGameObject();
    }

}
