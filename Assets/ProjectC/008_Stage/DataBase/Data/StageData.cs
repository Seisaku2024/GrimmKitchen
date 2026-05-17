using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageInfo;

[CreateAssetMenu]
[System.Serializable]
public class StageData : ScriptableObject
{
    // 制作者　田内

    //=============================================================
    // ID

    [Header("ステージID")]
    [SerializeField]
    private StageID m_stageID = StageID.Stage01;

    public StageID StageID { get { return m_stageID; } }

    //=============================================================
    // 移動シーン名

    [Header("移動したいシーン名")]
    [SerializeField]
    private string m_sceneName = "GameScene";

    public string SceneName { get { return m_sceneName; } }

    //====================================================
    // ステージ名

    [Header("ステージ名")]
    [SerializeField]
    private UnityEngine.Localization.LocalizedString m_stageNameText;

    public UnityEngine.Localization.LocalizedString StageNameText { get { return m_stageNameText; } }

    //========================================
    // 説明文

    [Header("ステージ説明文")]
    [SerializeField]
    private UnityEngine.Localization.LocalizedString m_stageDescriptionText;

    public UnityEngine.Localization.LocalizedString StageDescriptionText { get { return m_stageDescriptionText; } }

    //========================================
    // 画像

    [Header("ステージイメージ画像")]
    [SerializeField]
    private Sprite m_stageSprite = null;

    public Sprite StageSprite { get { return m_stageSprite; } }

    //============================
    // 初期ロック

    [Header("初期ロック")]
    [SerializeField]
    private bool m_initializeLock = false;

    public bool InitializeLock { get { return m_initializeLock; } }


    //======================
    // クリアデータ

    private ClearStageData m_clearStageData = null;

    public ClearStageData ClearStageData
    {
        get
        {
            if (m_clearStageData == null) m_clearStageData = new(m_stageID);
            return m_clearStageData;
        }
    }

    //=============================================================

    [Header("難易度設定")]
    [SerializeField] private StageEnemyStatus m_easyStatus;
    public StageEnemyStatus EasyStatus { get { return m_easyStatus; } }

    [SerializeField] private StageEnemyStatus m_normalStatus;
    public StageEnemyStatus NormalStatus { get { return m_normalStatus; } }

    [SerializeField] private StageEnemyStatus m_hardStatus;
    public StageEnemyStatus HardStatus { get { return m_hardStatus; } }


    //========================================
    //              実行処理
    //========================================

    public void Load(ClearStageDataSaveLoad _data)
    {
        if (_data == null) return;
        m_clearStageData = new(m_stageID);
        m_clearStageData.IsClear = _data.IsClear;
        m_clearStageData.IsLock = _data.IsLock;
    }

    public void SetClear()
    {
        if (m_clearStageData == null) m_clearStageData = new(m_stageID);

        m_clearStageData.IsClear = true;
    }

    public void SetUnLock()
    {
        if (m_clearStageData == null) m_clearStageData = new(m_stageID);

        m_clearStageData.IsLock = false;
    }
}


/// <summary>
/// 読み込み/書き込み用ステージデータ
/// </summary>
[System.Serializable]
public class ClearStageDataSaveLoad
{
    // 制作者 田内
    public ClearStageDataSaveLoad(StageID _id)
    {
        var data = StageDataBaseManager.instance.GetStageData(_id);
        if (data == null) return;

        IsLock = data.InitializeLock;
    }

    public ClearStageDataSaveLoad(StageID _id, ClearStageData _data)
    {
        if (_data == null) return;
        StageID = _id;
        IsClear = _data.IsClear;
        IsLock = _data.IsLock;
    }

    public StageID StageID = StageID.Stage01;

    public bool IsClear = false;

    public bool IsLock = false;
}

[System.Serializable]
public class ClearStageData
{
    // 制作者 田内
    // ステージのクリア情報

    public ClearStageData(StageID _id)
    {
        var data = StageDataBaseManager.instance.GetStageData(_id);
        if (data == null) return;

        IsLock = data.InitializeLock;
        IsClear = false;
    }

    public bool IsLock = false;
    public bool IsClear = false;

}
