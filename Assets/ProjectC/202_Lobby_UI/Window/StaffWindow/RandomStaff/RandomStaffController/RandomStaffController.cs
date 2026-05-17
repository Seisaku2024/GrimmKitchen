using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StaffInfo;

using Cysharp.Threading.Tasks;

public class RandomStaffController : ValueController
{
    // 制作者 田内
    // ランダムスタッフを操作するコントローラー


    [Header("作成するウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_windowController = null;


    [Header("スタッフ値段")]
    [SerializeField]
    [Range(0, 10000)]
    private int m_price = 100;


    [Header("================排出確率(重みで計算)================")]
    [Header("ノーマル確率")]
    [SerializeField]
    private float m_normalStaffRatio = 87.0f;

    [Header("レア確率")]
    [SerializeField]
    private float m_rareStaffRatio = 10.0f;

    [Header("激レア確率")]
    [SerializeField]
    private float m_superRareStaffRatio = 3.0f;

    //==================================
    // ランダムで取得したスタッフデータ

    private List<StaffStatusData> m_randomGetStaffStatusDataList = new();

    public List<StaffStatusData> RandomGetStaffStatusDataList
    {
        get { return m_randomGetStaffStatusDataList; }
    }

    //=====================================
    //              実行処理
    //=====================================


    /// <summary>
    /// 実行処理
    /// </summary>
    override public async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnUpdate();
            cancelToken.ThrowIfCancellationRequested();

            // 作成を行うかどうか
            await CreateRandomStaff();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    override protected void SetData()
    {
        // 呼び出せる数
        int num = (int)ManagementDataManager.instance.TotalEarnedMoney / (int)m_price;

        // 最小数を更新
        if (num <= 0) m_minValue = 0;
        else m_minValue = 1;

        // 最大数を更新
        m_maxValue = num;

        // 現在選択中の値を更新
        if (m_maxValue < m_currentValue) m_currentValue = m_maxValue;
        if (m_currentValue < m_minValue) m_currentValue = m_minValue;

        // スライダーの値更新
        SetSliderValue();

        m_isSelectChangeFlg = true;
    }

    /// <summary>
    /// 現在の使用予定金額
    /// </summary>
    public int GetTotalPrice()
    {
        float value = m_price * m_currentValue;
        return (int)value;
    }


    public override bool IsDecision()
    {
        // 呼び出せる数
        int num = (int)ManagementDataManager.instance.TotalEarnedMoney / (int)m_price;

        if (num < m_minValue || num <= 0) return false;
        if (m_maxValue < num) return false;


        // ストレージに空きがなければ
        if (StaffManager.instance.IsAddStaffStorage()) return false;


        return true;
    }


    // 作成
    private async UniTask CreateRandomStaff()
    {
        #region nullチェック
        if (m_decisionInputActionButton == null)
        {
            Debug.LogError("DecisionInputActionButtonがシリアライズされていません");
            return;
        }
        #endregion

        if (IsDecision() == false) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // 決定ボタンが押された場合
            if (m_decisionInputActionButton.IsInputActionTrriger())
            {

                // 初期化
                m_randomGetStaffStatusDataList.Clear();

                // 選択分スタッフを作成
                for (int i = 0; i < m_currentValue; ++i)
                {
                    // もし所持金を超過した場合は強制終了
                    if (ManagementDataManager.instance.TotalEarnedMoney < m_price) break;

                    // 所持金を消費
                    ManagementDataManager.instance.TotalEarnedMoney -= m_price;

                    // スタッフステータスを作成し追加
                    m_randomGetStaffStatusDataList.Add(CreateStaffStatusData());
                }

                // データを更新
                SetData();

                // ウィンドウを作成
                await CreateWindow();
                cancelToken.ThrowIfCancellationRequested();

                // 初期化
                m_randomGetStaffStatusDataList.Clear();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }



    /// <summary>
    /// スタッフステータスデータを作成
    /// </summary>
    private StaffStatusData CreateStaffStatusData()
    {
        float total = m_normalStaffRatio + m_rareStaffRatio + m_superRareStaffRatio;
        float random = Random.value * total;


        // 激レアかどうか
        if (random < m_superRareStaffRatio)
        {
            // 激レアスタッフを作成
            StaffMemberData data = StaffMemberDataBaseManager.instance.GetRandomRarityStaffMemberData(StaffRarityType.SuperRare);

            // 既に所持・既にランダムコントローラーで出現していなければ
            if (data != null && StaffManager.instance.IsAddedStaff(data.StaffID) == false && IsAddedStaff(data.StaffID) == false)
            {

                StaffStatusData staffStatusData = new();
                staffStatusData.SetData(data.StaffID);

                return staffStatusData;
            }
        }
        random -= m_superRareStaffRatio;


        // レアかどうか
        if (random < m_rareStaffRatio)
        {
            // レアスタッフを作成
            StaffMemberData data = StaffMemberDataBaseManager.instance.GetRandomRarityStaffMemberData(StaffRarityType.Rare);

            // 既に所持・既にランダムコントローラーで出現していなければ
            if (data != null && StaffManager.instance.IsAddedStaff(data.StaffID) == false && IsAddedStaff(data.StaffID) == false)
            {

                StaffStatusData staffStatusData = new();
                staffStatusData.SetData(data.StaffID);

                return staffStatusData;
            }
        }
        random -= m_rareStaffRatio;


        // それ以外であればノーマルスタッフを作成
        {
            StaffMemberData data = StaffMemberDataBaseManager.instance.GetRandomRarityStaffMemberData(StaffRarityType.Normal);

            // ランダムでステータスをセットする
            StaffStatusData staffStatusData = new();
            staffStatusData.RandomSetData(data.StaffID);

            return staffStatusData;
        }
    }


    // リスト内に引数IDのスタッフが存在するかどうか
    private bool IsAddedStaff(StaffID _id)
    {
        foreach (var staff in m_randomGetStaffStatusDataList)
        {
            // 既に追加されていればtrue
            if (staff.StaffID == _id) return true;
        }

        // 追加されていなければfalse
        return false;
    }


    private async UniTask CreateWindow()
    {
        #region nullチェック
        if (m_windowController == null)
        {
            Debug.LogError("WindowControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            var controller = Instantiate(m_windowController);
            await controller.CreateWindow<SelectRandomStaffWindow>(onBeforeInitialize: async _ =>
            {
                // データをセットする
                _.SetData(this);
                await UniTask.CompletedTask;
            });
            cancelToken.ThrowIfCancellationRequested();
            if (controller != null) Destroy(controller);


            await UniTask.CompletedTask;
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }



}
