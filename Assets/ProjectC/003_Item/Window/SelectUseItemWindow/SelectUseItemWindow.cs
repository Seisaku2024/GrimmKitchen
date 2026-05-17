using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using SelectUIInfo;
using ItemInfo;
using SelectUseItemInfo;


namespace SelectUseItemInfo
{
    public enum SelectUseItemID
    {
        None = 0,
        Eat = 1,                        // 食べる
        Dispose = 2,                    // 捨てる
        MoveInventory = 3,              // 移動
        MoveManagementStorage = 4,      // 移動

        Exit = 100,                     // 終了
    }
}

public partial class SelectUseItemWindow : BaseWindow
{
    // 選択した作成アイテムの使用用途を決めるウィンドウ
    // 制作者(田内)

    [Header("UIコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    [Header("親とするUI")]
    [SerializeField]
    private GameObject m_parentUI = null;


    [Header("作成するボタンプレハブ")]
    [SerializeField]
    private SelectUseItemButton m_button = null;

    // 返す使用用途ID
    private SelectUseItemID m_currentSelectUseItemID = SelectUseItemID.None;


    //==========================================
    //              実行処理
    //==========================================


    /// <summary>
    /// アイテムIDをセット
    /// </summary>
    public void SetData(List<SelectUseItemID> _idList, ItemTypeID _typeID, uint _id)
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerが登録されていません");
            return;
        }
        #endregion

        var data = ItemDataBaseManager.instance.GetItemData(_typeID, _id);

        foreach (var id in _idList)
        {
            // ボタン作成
            bool isCreate = true;
            bool isUse = true;

            switch (id)
            {
                case SelectUseItemID.Eat:
                    {
                        // 料理でなければ作成しない
                        if (data == null || data is FoodData == false)
                        {
                            isCreate = false;
                            break;
                        }

                        // 回復料理でなければ使用できない
                        var foodData = data as FoodData;
                        if (foodData.IsFoodType(FoodData.FoodType.Heal) == false)
                        {
                            isUse = false;
                            break;
                        }

                        break;
                    }

                case SelectUseItemID.Dispose:
                    {
                        break;
                    }

                case SelectUseItemID.MoveInventory:
                    {
                        break;
                    }

                case SelectUseItemID.MoveManagementStorage:
                    {
                        break;
                    }

                case SelectUseItemID.Exit:
                    {
                        break;
                    }

                case SelectUseItemID.None:
                    {
                        break;
                    }
            }

            if (isCreate)
            {
                var button = Instantiate(m_button, m_parentUI.gameObject.transform);
                button.SetData(id, isUse);
                m_selectUIController.AddUI(button.gameObject, SelectUIType.Press, 1);
            }
        }
    }


    public new async UniTask<SelectUseItemID> OnUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerが登録されていません");
            return SelectUseItemID.None;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();


                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // ボタンが選択されたら
                if (IsPressButton())
                {
                    return m_currentSelectUseItemID;
                }

                // 閉じる
                if (IsClose())
                {
                    return SelectUseItemID.None;
                }

                m_selectUIController.OnLateUpdate();

                await UniTask.DelayFrame(1);

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        return default;

    }


    // ボタンが選択されたかを確認するメソッド 
    private bool IsPressButton()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerが登録されていません");
            return false;
        }

        // 選択されれば
        if (m_selectUIController.IsPress == false) return false;

        var ui = m_selectUIController.CurrentSelectUI;
        if (ui == null) return false;

        var button = ui.GetComponent<SelectUseItemButton>();
        if (button == null || button.IsCanPress == false) return false;

        // 返すIDをセット
        m_currentSelectUseItemID = button.ID;

        return true;

    }



    // 使用用途を決めるボタンを作成する
    private void AddButton(SelectUseItemID _id, bool _active)
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("UI選択コントローラーが存在しません");
            return;
        }

        if (m_parentUI == null)
        {
            Debug.LogError("親にするUIが存在しません");
            return;
        }

        if (m_button == null)
        {
            Debug.LogError("作成するボタンが存在しません");
            return;
        }

        // ボタンを作成(親指定)
        var button = Instantiate(m_button, m_parentUI.transform);
        button.SetData(_id, _active);

        // UIコントローラーに追加
        m_selectUIController.AddUI(button.gameObject, SelectUIType.Press, 1);

    }

}
