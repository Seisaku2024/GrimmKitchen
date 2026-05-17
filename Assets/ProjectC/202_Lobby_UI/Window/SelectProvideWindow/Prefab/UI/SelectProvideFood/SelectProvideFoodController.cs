using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodInfo;

public class SelectProvideFoodController : MonoBehaviour
{

    [Header("UI選択コントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;

    [Header("追加できませんUI")]
    [SerializeField]
    protected Canvas m_unAddUI = null;

    //====================================================
    //                  実行処理
    //====================================================

    // 提供料理をセットする
    public void OnUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        // 選択されれば
        if (m_selectUIController.IsPress == false) return;
        if (m_selectUIController.CurrentSelectUI == null) return;

        // ItemSlotDataを取得
        if (m_selectUIController.CurrentSelectUI.TryGetComponent(out ProvideFoodRecipeSlotData itemSlotData))
        {
            // 作成出来ない状態であれば
            if (itemSlotData.IsProvide == false) return;

            // 既に追加されていれば
            if (ProvideFoodManager.instance.IsAddedProvideFood((FoodID)itemSlotData.ItemData.ItemID))
            {
                // 取り除く
                ProvideFoodManager.instance.RemoveProvideFoodList((FoodID)itemSlotData.ItemData.ItemID);
            }
            // まだ追加されていなければ
            else
            {
                if (ProvideFoodManager.instance.IsAddList() == false)
                {
                    Instantiate(m_unAddUI);
                    return ;
                }

                // 追加する
                ProvideFoodManager.instance.AddProvideFoodList((FoodID)itemSlotData.ItemData.ItemID);
            }
        }

        
    }


}
