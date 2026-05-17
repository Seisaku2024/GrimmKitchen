using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectKeepUIData : MonoBehaviour
{
    // 制作者 田内
    // SelectKeepUIControllerで選択されたときに行う処理

    [Header("フレーム")]
    [SerializeField]
    private GameObject m_frame = null;

    //==============================================
    //                  実行処理
    //==============================================

    /// <summary>
    /// SelectKeepUIController用のデータをセットする
    /// </summary>
    public void SetData(bool _active)
    {
        // フレーム
        SetFrame(_active);
    }


    // フレームのアクティブを更新
    private void SetFrame(bool _active)
    {
        if (m_frame == null) return;

        m_frame.SetActive(_active);
    }



}
