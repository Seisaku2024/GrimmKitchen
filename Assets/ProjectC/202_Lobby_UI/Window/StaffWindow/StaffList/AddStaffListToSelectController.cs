using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SelectUIController にスタッフリストを、ホールとシェフ分けて追加する（吉田）
/// </summary>

public class AddStaffListToSelectController : MonoBehaviour
{
    [SerializeField]
    private SelectUIController m_selectUIController;

    [SerializeField]
    private CreateStaffPointSlotList m_createStaffHallSlotList;

    [SerializeField]
    private CreateStaffPointSlotList m_createStaffChefSlotList;

    // Start is called before the first frame update
    virtual public void OnInitialize()
    {
        // 一回だけ実行
        if(SetSelectUIController())
        {
            DestroyImmediate(this);
        }
    }

    private bool SetSelectUIController()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIController がセットされていません");
            return false;
        }
        if (m_createStaffHallSlotList == null)
        {
            Debug.LogError("CreateStaffHallSlotList がセットされていません");
            return false;
        }
        if (m_createStaffChefSlotList == null)
        {
            Debug.LogError("CreateStaffChefSlotList がセットされていません");
            return false;
        }
        #endregion

        // listが空でないかチェック
        if (m_createStaffHallSlotList.SlotList.Count <= 0)
        {
            return false;
        }
        if (m_createStaffChefSlotList.SlotList.Count <= 0)
        {
            return false;
        }

        // ホールとシェフを分けて追加
        foreach (var slot in m_createStaffHallSlotList.SlotList)
        {
            m_selectUIController.AddUI(slot, SelectUIInfo.SelectUIType.Press, 5);
        }
        m_selectUIController.AddBreakUI();
        foreach (var slot in m_createStaffChefSlotList.SlotList)
        {
            m_selectUIController.AddUI(slot, SelectUIInfo.SelectUIType.Press, 5);
        }

        return true;
    }
}
