using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public partial class ItemDescription : MonoBehaviour
{
    // 制作者 田内
    // ポケットアイテム

    [Foldout("ポケット")]
    [Header("-------------------------------------------------------")]
    [Header("ポケット所持数Text")]
    [SerializeField]
    protected TextMeshProUGUI m_pocketNumText = null;


    [Foldout("ポケット")]
    [Header("表示/非表示用")]
    [SerializeField]
    protected List<GameObject> m_pocketNumList = new();


    //==========================================================
    //                      実行処理
    //==========================================================

    /// <summary>
    /// 説明セット
    /// </summary>
    private void SetPocketDescription()
    {
        SetPocketNumText();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void InitilizePocketDescription()
    {
        SetPocketNumText(false);
    }


    /// <summary>
    /// アクティブ
    /// </summary>
    private void SetPocketActiveList()
    {
        UIExtensions.CheckToSetActiveGameObjectList(m_pocketNumText, m_pocketNumList);
    }


    /// <summary>
    /// ポケットの所持数表示
    /// </summary>
    private void SetPocketNumText(bool _active = true)
    {
        if (m_pocketNumText == null) return;

        m_pocketNumText.gameObject.SetActive(false);

        if (_active == false) return;
        if (m_pocketItemData == null) return;

        m_pocketNumText.text = m_pocketItemData.Num.ToString();

        m_pocketNumText.gameObject.SetActive(true);

    }


}
