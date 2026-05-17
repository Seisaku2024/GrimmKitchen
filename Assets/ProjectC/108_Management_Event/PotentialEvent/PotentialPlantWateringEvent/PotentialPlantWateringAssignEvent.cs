/*!
 * @file PotentialPlantWateringAssignEvent.cs
 * @brief 植物の水やりイベントの当たり判定系担当
 * @author 上甲
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantWateringAssignEvent : BaseAssignEventObject
{
    [SerializeField] private GameObject m_hpBarinstance = null;

    [Header("水分ゲージの表示を離れたときに消すかどうか")]
    [SerializeField] private bool m_isShowToggle = false;

    public bool IsWatered { get; set; } = false;

    //! @brief 接触時のイベント 水分ゲージを表示/キーバインド表示
    protected override void OnCollisionTriggerEvent()
    {
        if (m_isShowToggle && m_hpBarinstance != null)
        {
            m_hpBarinstance.SetActive(true);
        }
    }

    //! @brief 接触終了時のイベント 水分ゲージを非表示/キーバインド非表示
    protected override void OnCollisionTriggerExitEvent()
    {
        if (m_isShowToggle && m_hpBarinstance != null)
        {
            m_hpBarinstance.SetActive(false);
        }
    }

    //! @brief 接触後アクセスされたかどうかの定義 Gathering(Eキー)を押すとアクセスされる
    public override bool IsAccessed(ref IInputProvider input, GameObject player)
    {
        return input.Cleanning;
    }

    //! @brief アクセス時のイベント　水分補給完了
    public override void OnCollisionAccessEvent()
    {
        //料理所持状態なら入らないようにする
        if (m_cCore.PlayerParameters?.m_isFoodHold == true) return;

        IsWatered = true;
    }
}
