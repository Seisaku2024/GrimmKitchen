using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 制作者　吉田
/// 
/// スタッフのインベントリがいっぱいの時に表示する
/// 
/// </summary>
public class AttentionMaxStaff : WindowUpdateBase
{
    [Header("スタッフ数がMAXの時、見えるor見えない")]
    [SerializeField]
    private bool m_view = true;

    // Start is called before the first frame update
    public override void OnInitialize()
    {
        if (StaffManager.instance.IsAddStaffStorage())
        {
            gameObject.SetActive(m_view);
        }
        else
        {
            gameObject.SetActive(!m_view);
        }
    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        if (StaffManager.instance.IsAddStaffStorage())
        {
            gameObject.SetActive(m_view);
        }
        else
        {
            gameObject.SetActive(!m_view);
        }
    }

}
