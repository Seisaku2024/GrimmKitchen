using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using Cysharp.Threading.Tasks;

public class CreateRandomStaffSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // ランダムなステータスのスタッフスロットを作成する
    // 徐々に作成する


    [Header("作成で待機する時間")]
    [SerializeField]
    private float m_createDelayTime = 0.1f;

    [Header("DOSpead")]
    [SerializeField]
    private float m_duration = 1.0f;

    [Header("Ease")]
    [SerializeField]
    private Ease m_ease = Ease.OutBounce;



    // コントローラー
    private RandomStaffController m_randomStaffController = null;

    //===================================================
    //                  実行処理
    //===================================================

    public void SetData(RandomStaffController _controller)
    {
        m_randomStaffController = _controller;
    }



    protected override async UniTask CreateSlotInstance()
    {
        #region nullチェック
        if (m_slot == null)
        {
            Debug.LogError("スロットがシリアライズされていません");
            return;
        }
        if (m_randomStaffController == null)
        {
            Debug.LogError("StaffControllerがシリアライズされていません");
            return;
        }
        #endregion

        // 削除する
        DestroySlotList();


        try
        {
            foreach (var data in m_randomStaffController.RandomGetStaffStatusDataList)
            {
                if (data == null) continue;

                // 子オブジェクトにスロット作成
                var slot = Instantiate(m_slot, gameObject.transform);


                if (slot.TryGetComponent<StaffStatusSlotData>(out var slotData))
                {
                    slotData.SetStaffStatusData(data);
                }
                else
                {
                    Debug.LogError("StaffPointSlotDataがシリアライズされていません");
                }

                m_slotList.Add(slot);

                // コントローラーに追加
                AddSelectUIControler(slot);

            }


            // DoScaleで徐々に出現させる
            int i = 0;
            foreach (var slot in m_slotList)
            {
                // アニメーションさせる
                if (slot.TryGetComponent<RectTransform>(out var rect))
                {
                    _ = rect.DOScale(endValue: Vector3.zero, duration: m_duration).
                        SetDelay(m_createDelayTime * i).
                        SetLink(rect.gameObject).
                        SetEase(m_ease).
                        From().
                        SetUpdate(true);
                }

                i++;
            }


        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }


}

