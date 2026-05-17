/*!
 * @file PotentialHeavyGangstersComingEvent.cs
 * @brief 大量に迷惑客が来店するイベント
 * 初期段階では、外部からの呼び出しでのみ使用する
 * @author 上甲
 */

using ManagementGameInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PotentialHeavyGangstersComingEvent : MonoBehaviour
{
    [SerializeField] CustomerPasserbyRootManager m_customerPasserbyRootManager = null;

    [SerializeField, Range(0, 20)]
    int m_appearNum=5;

    [SerializeField] GameObject m_gangsterPrefab = null;


    [ContextMenu("HeavyComing")]
    public void StartEvent()
    {
        AppearGangster();
    }


    void AppearGangster()
    {
        if (m_customerPasserbyRootManager == null)
        {
            Debug.LogError("CustomerPasserbyRootManagerがシリアライズされていません");
            return;
        }
        if (m_gangsterPrefab == null)
        {
            Debug.LogError("GangsterPrefabがシリアライズされていません");
            return;
        }

        for (int i = 0; i < m_appearNum; i++)
        {
            var obj = Instantiate(m_gangsterPrefab);

            var pos = m_customerPasserbyRootManager.RandomAppearPos;

            obj.transform.position = pos;

            // キャラクターモーターを更新する
            var core = obj.GetComponent<MyCharacterController>();
            if (core != null)
            {
                core.SetPositionMotor(pos);
            }
            else
            {
                Destroy(obj.gameObject);
            }

        }
    }
}