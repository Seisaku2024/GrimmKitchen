using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SelectKeepRandomStaffController : SelectKeepUIController
{
    // 制作者 田内
    // ランダムで作成したスタッフから、どのスタッフを取得するか

    [Header("決定ボタン")]
    [SerializeField]
    private InputActionButton m_inputActionButton = null;


    // 選択中のスタッフ総雇用金額
    private int m_totalEmploymentPrice = 0;

    public int TotalEmploymentPrice
    {
        get { return m_totalEmploymentPrice; }
    }


    //=========================================================
    //                  実行処理
    //=========================================================

    protected override void SetMaxSelectableNumber()
    {
        // 現在のスタッフストレージ空き数のみ選択可能にする
        m_maxSelectableNumber = StaffManager.instance.GetFreeSpaceStaffStorageNum();
    }


    protected override void AddSelectKeepObject(GameObject _obj)
    {
        base.AddSelectKeepObject(_obj);

        if (_obj.TryGetComponent<StaffStatusSlotData>(out var slotData))
        {
            // 総額に加算
            m_totalEmploymentPrice += slotData.StaffStatusData.EmploymentPrice();
        }
    }

    protected override void RemoveSelectKeepObject(GameObject _obj)
    {
        base.RemoveSelectKeepObject(_obj);

        if (_obj.TryGetComponent<StaffStatusSlotData>(out var slotData))
        {
            // 総額から減算
            m_totalEmploymentPrice -= slotData.StaffStatusData.EmploymentPrice();
        }
    }

    /// <summary>
    /// マネージャーにデータを追加する
    /// </summary>
    public async UniTask<bool> AddManagerData()
    {
        #region nullチェック
        if (m_inputActionButton == null)
        {
            Debug.LogError("InputActionButtonがシリアライズされていません");
            return false;
        }
        #endregion

        if (m_inputActionButton.IsInputActionTrriger())
        {
            var cancelToken = this.destroyCancellationToken;
            try
            {
                // マネージャーにデータを追加する
                foreach (var obj in m_selectKeepObjectList)
                {
                    if (obj.TryGetComponent<StaffStatusSlotData>(out var slotData))
                    {
                        if (slotData.StaffStatusData == null) continue;

                        // スタッフを追加
                        StaffManager.instance.AddStaffStorage(slotData.StaffStatusData);

                        // 雇用金額を減算
                        ManagementDataManager.instance.TotalEarnedMoney -= slotData.StaffStatusData.EmploymentPrice();
                    }
                }

                // 初期化
                m_selectKeepObjectList.Clear();

                await UniTask.CompletedTask;

                return true;
            }
            catch (System.OperationCanceledException ex)
            {
                Debug.Log(ex);
            }

        }

        return false;
    }


}
