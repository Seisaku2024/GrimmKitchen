using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;


public class BaseProvideFoodUI : BaseWindowUI
{
    // 制作者 田内
    // 提供料理UIの基底クラス


    [Header("UI選択コントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;

    override public async UniTask OnLateUpdate()
    {
        #region nullチェック

        if (m_selectUIController == null)
        {
            Debug.LogError("選択コントローラーがシリアライズされていません");
            return;
        }
        #endregion

        // 後処理
        m_selectUIController.OnLateUpdate();

        await UniTask.CompletedTask;
    }

}
