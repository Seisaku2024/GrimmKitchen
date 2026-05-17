using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;


public class StaffPointSlotData : StaffStatusSlotData
{
    // 制作者 田内
    // スタッフのスロット

    [Header("スタッフタイプ")]
    [SerializeField]
    private TextMeshProUGUI m_staffTypeText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_staffTypeTextList = new();


    // このスロットのポイントデータ
    private StaffPointData m_staffPointData = null;

    public StaffPointData StaffPointData
    {
        get { return m_staffPointData; }
    }

    //========================================
    //              実行処理
    //========================================


    private void Start()
    {
        // スタッフポイントデータ変更イベントを受信
        MessageBroker.Default.Receive<GlobalChangeStaffPointData>().Subscribe(_ =>
        {
            // 更新
            SetStaffPointData(m_staffPointData);
            SetStaffStatusData(m_staffStatusData);

        }).AddTo(this);
    }



    /// <summary>
    /// データをセット/更新する
    /// </summary>
    public void SetStaffPointData(StaffPointData _data)
    {
        // スタッフポイントデータをセットする
        m_staffPointData = _data;

        // スタッフポイントデータが持っているスタッフステータスをセットする
        if (m_staffPointData != null)
        {
            m_staffStatusData = m_staffPointData.StaffStatusData;
        }
        else
        {
            m_staffStatusData = null;
        }

        SetStaffTypeText();
        SetActiveGameObjectList();
    }


    private void SetActiveGameObjectList()
    {
        UIExtensions.CheckToSetActiveGameObjectList(m_staffTypeText, m_staffTypeTextList);
    }


    private void SetStaffTypeText(bool _active = true)
    {
        if (m_staffTypeText == null) return;

        m_staffTypeText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_staffPointData == null) return;

        var data = StaffTypeDataBaseManager.instance.GetStaffTypeData(m_staffPointData.SetStaffType);
        if (data == null) return;

        // スタッフタイプ名をセット
        m_staffTypeText.text = data.StaffTypeName;
        m_staffTypeText.gameObject.SetActive(true);
    }


}
