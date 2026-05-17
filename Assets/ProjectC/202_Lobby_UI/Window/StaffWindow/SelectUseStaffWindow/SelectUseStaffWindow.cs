using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelectUseStaffInfo;
using SelectUIInfo;
using Cysharp.Threading.Tasks;

namespace SelectUseStaffInfo
{
    public enum SelectUseStaffID
    {
        None = 0,

        Dismissal = 1,  // 解雇
        StatusUp = 2,    // 強化

        Exit = 100,                     // 終了
    }
}

public class SelectUseStaffWindow : BaseWindow
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
    private SelectUseStaffButton m_button = null;

    // 返す使用用途ID
    private SelectUseStaffID m_currentSelectUseStaffID = SelectUseStaffID.None;

    //==========================================
    //              実行処理
    //==========================================


    /// <summary>
    /// アイテムIDをセット
    /// </summary>
    public void SetData(List<SelectUseStaffID> _idList, StaffStatusData _data)
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerが登録されていません");
            return;
        }
        #endregion

        if (_data == null) return;

        foreach (var id in _idList)
        {
            // ボタン作成
            bool isCreate = true;
            bool isUse = true;

            switch (id)
            {
                case SelectUseStaffID.Dismissal:
                    {
                        if ((_data.IsStatusType & StaffStatusData.StatusType.IsDismissal) != 0) isUse = false;
                        break;
                    }

                case SelectUseStaffID.StatusUp:
                    {
                        if ((_data.IsStatusType & StaffStatusData.StatusType.IsStatusUp) != 0) isUse = false;
                        break;
                    }

                default:
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


    public new async UniTask<SelectUseStaffID> OnUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerが登録されていません");
            return SelectUseStaffID.None;
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
                    return m_currentSelectUseStaffID;
                }

                // 閉じる
                if (IsClose()) return SelectUseStaffID.None;

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

        var button = ui.GetComponent<SelectUseStaffButton>();
        if (button == null || button.IsCanPress == false) return false;

        // 返すIDをセット
        m_currentSelectUseStaffID = button.ID;

        return true;

    }

}
