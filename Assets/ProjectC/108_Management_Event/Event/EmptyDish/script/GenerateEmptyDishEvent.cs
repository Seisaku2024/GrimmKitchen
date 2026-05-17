using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomerStateInfo;

public class GenerateEmptyDishEvent : BaseManagementEvent
{

    [Header("空皿のプレハブ")]
    [SerializeField] private List<GameObject> m_emptyDishPrefab = new List<GameObject>();

    // ターゲットにしているテーブルセット
    private TableSetData m_tragetTableSetData = null;

    // 基の座っていたオブジェクト
    private GameObject m_targetSitObject = null;

    // 作成した空皿オブジェクト
    private GameObject m_emptyDishInstance = null;

    //=======================================================
    //                      実行処理
    //=======================================================

    /// <summary>
    /// 空皿イベントに必要なデータをセットする
    /// </summary>
    public void SetData(GameObject _sitObject, TableSetData _tableSetData)
    {
        // ターゲットにする座っていたオブジェクト
        m_targetSitObject = _sitObject;

        // ターゲットにするテーブルセット
        m_tragetTableSetData = _tableSetData;

        // 座標を更新
        gameObject.transform.position = _tableSetData.TablePoint.position;
    }

    public override void OnStart()
    {
        if (m_emptyDishPrefab == null)
        {
            return;
        }
        int i = Random.Range(0, m_emptyDishPrefab.Count);

        if (m_emptyDishPrefab[i] == null)
        {
            return;
        }

        m_emptyDishInstance = Instantiate(m_emptyDishPrefab[i]);
        m_emptyDishInstance.transform.position = transform.position;
    }

    public override void OnUpdate()
    {
        // 掃除されれば
        if (m_emptyDishInstance == null)
        {
            // 成功
            SetEventEnd(ManagementGameInfo.EventSolutionType.Solution);
        }


        // 座っているオブジェクトが存在しなければ残したまま
        if (m_tragetTableSetData?.SitObject == null) return;

        // ターゲットのテーブルに座っているオブジェクトが基のオブジェクトと異なっていれば
        if (m_tragetTableSetData.SitObject != m_targetSitObject)
        {
            // 座ってるオブジェクトが客であれば
            if (m_tragetTableSetData.SitObject.TryGetComponent<CustomerData>(out var data))
            {
                // 料理を待つ状態であれば
                if (data.CurrentCustomerState == CustomerState.WaitFood)
                {
                    // 失敗
                    SetEventEnd(ManagementGameInfo.EventSolutionType.UnSolution);
                }
            }
            // それ以外であれば
            else
            {
                // 失敗
                SetEventEnd(ManagementGameInfo.EventSolutionType.UnSolution);
            }
        }
    }



    public override void OnExit()
    {
        if (m_emptyDishInstance != null)
        {
            Destroy(m_emptyDishInstance);
        }

        base.OnExit();
    }

}
