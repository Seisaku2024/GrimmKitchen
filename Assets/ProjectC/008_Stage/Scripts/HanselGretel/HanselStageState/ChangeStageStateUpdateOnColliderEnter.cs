using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// コライダーと接触した際にManagerへと接触を報告する処理（山本）
public class ChangeStageStateUpdateOnColliderEnter : MonoBehaviour
{
    private bool m_isColliderEnter = false;
    public bool IsColliderEnter => m_isColliderEnter;



    private void OnTriggerEnter(Collider other)
    {
        //プレイヤー以外は早期リターン
        if (other.transform.tag != "Player")
        {
            return;
        }


        if (m_isColliderEnter == false)
        {
            m_isColliderEnter = true;
        }
    }

}
