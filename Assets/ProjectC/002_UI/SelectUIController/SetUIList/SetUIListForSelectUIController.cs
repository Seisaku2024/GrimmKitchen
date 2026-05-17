using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaintsField;
using SaintsField.Playa;

/// <summary>
/// 制作者　吉田
/// 一つのオブジェクトの子供にあるUIを全て取得し、
/// SelectUIController へ 選択されるUIをセットする
/// インスペクタ上でボタンを押す or コンテキストメニューで実行 or Start関数で実行
/// 
/// 実行は一度のみなので、Start関数で削除しています。
/// </summary>
public class SetUIListForSelectUIController : MonoBehaviour
{

    [Header("※以下 nullの場合は実行されない エラー出してます")]

    [Header("UIListを登録する SelectUIController")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("UIListに登録される 親オブジェクト")]
    [SerializeField]
    private GameObject m_parentObject = null;

    [BelowButton(nameof(SettingUIList))]

    [Header("横 falseの場合は縦")]
    [SerializeField]
    private bool m_isHorizontal = false;


    // Start is called before the first frame update
    void Start()
    {
        SettingUIList();
        Destroy(this);
    }

    [ContextMenu("SettingUIList")]
    private void SettingUIList()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがnullです" +
                "\nGameObject：" + gameObject.name);
            return;
        }
        if (m_parentObject == null)
        {
            Debug.LogError("ParentObjectがnullです" +
                "\nGameObject：" + gameObject.name);
            return;
        }

        m_selectUIController.ResetUIList();


        // 子供をUIListに登録
        int childCount = m_parentObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = m_parentObject.transform.GetChild(i).gameObject;

            if (m_isHorizontal)
            {
                m_selectUIController.AddUI(child, SelectUIInfo.SelectUIType.Press, childCount);
            }
            else
            {
                m_selectUIController.AddUI(child, SelectUIInfo.SelectUIType.Press, 0);
            }
        }

    }


}
