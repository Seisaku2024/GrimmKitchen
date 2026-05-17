using Arbor;
using HanselStageInfo;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class NPCParameters : MonoBehaviour
{
    // NPC用のパラメーター（山本）

    [Header("NPCの種類")]
    [SerializeField]
    private SpeakerType m_speakerType = SpeakerType.None;
    public SpeakerType SpeakerType => m_speakerType;

    [Header("カットシーンのカメラターゲット")]
    [SerializeField]
    private Transform m_camTrans;
    public Transform CameraTrans => m_camTrans;

    [Header("Arbor")]
    [SerializeField]
    private ArborFSM m_arborFSM;
    public ArborFSM Arbor { get { return m_arborFSM; } set { m_arborFSM = value; } }

    [SerializeField]
    private PathFinding m_pathFinding;
    public PathFinding PathFinding { get { return m_pathFinding; } set { m_pathFinding = value; } }


    //NPC移動速度
    private float m_moveSpeed = 0.0f;
    public float MoveSpeed { get { return m_moveSpeed; } set { m_moveSpeed = value; } }

    // リスト番号
    private int m_listNum = 0;
    public int ListNum { get { return m_listNum; } set { m_listNum = value; } }

    // ターゲット座標
    private Transform m_targetTransform = null;
    public Transform TargetTransform { get { return m_targetTransform; }set{ m_targetTransform = value; } }

    // 現在のGameStageStateを覚える
    private GameStageState m_stageState = GameStageState.Normal;
    public GameStageState StageState { get { return m_stageState; } set { m_stageState = value; } }


    // NPCが登録されているリストへ移動するかのフラグ
    private bool m_moveTargetPositionFlg = false;
    public bool MoveTargetPositionFlg
    {
        get { return m_moveTargetPositionFlg; }
        set { m_moveTargetPositionFlg = value; }
    }

    // 目的地へ到達したかのフラグ
    private bool m_arriveTargetPositionFlg = false;
    public bool ArriveTargetPositionFlg
    {
        get { return m_arriveTargetPositionFlg; }
        set { m_arriveTargetPositionFlg = value; }
    }

    [Header("NPC移動座標のリスト")]
    [SerializeField]
    private SerializableDictionary<GameStageState, List<Transform>> m_targetTransList = null;



    // リストから目標地点をセットする関する
    public void SetTargetTransform()
    {
        var list = m_targetTransList[m_stageState];
        Transform targetTrans = null;

        if (list == null)
        {
            //Nullだったら登録されてないので失敗
            Debug.LogError("指定したGameStageStateのリストがありません");
            return;
        }

        if (list.Count - 1 < m_listNum)
        {
            m_listNum = 0;
            m_moveTargetPositionFlg = false;
            return;
        }

        targetTrans = list[m_listNum];
        m_targetTransform = targetTrans;

    }

    // リスト数が登録されているリストの要素数より超えているかの確認
    public void CheckListNum()
    {
        var list = m_targetTransList[m_stageState];

        if (list == null)
        {
            //Nullだったら登録されてないので失敗
            Debug.LogError("指定したGameStageStateのリストがありません");
            return;
        }

        // 超えたらリスト数を0に戻す
        if (list.Count - 1 < m_listNum)
        {
            m_listNum = 0;
            m_moveTargetPositionFlg = false;
        }
        else 
        {
            m_listNum++;
        }
      
    }

    [Header("プレイヤーからどのくらい離れたら動きを止めるか")]
    [SerializeField]
    private float m_stopMoveDistance = 10.0f;
    public float StopMoveDistance { get { return m_stopMoveDistance; }set{ m_stopMoveDistance = value; } }

    [Header("NPCが停止するプレイヤーとの距離の初期値")]
    [SerializeField]
    private float m_initStopMoveDist = 10.0f;
    public float InitStopMoveDist => m_initStopMoveDist;

    [Header("プレイヤーがどのくらい近づいたら走り状態に移行するか")]
    [SerializeField]
    private float m_changeMoveSpeedDistance = 1.0f;
    public float ChanegMoveSpeedDist => m_changeMoveSpeedDistance;

    [Header("到着地点からどのくらい離れた位置で目的達成となるか")]
    [SerializeField]
    private float m_goalPointRadius = 1.0f;
    public float GoalPointRadius => m_goalPointRadius;

    [SerializeField]
    private Waypoint m_waypoint;
    public Waypoint Waypoint => m_waypoint;

    [Header("目印用マークのTransform")]
    [SerializeField]
    private Transform m_landmarkCanvasTrans = null;
    public Transform LandMarkCanvasTrans { get { return m_landmarkCanvasTrans; }
                                           set { m_landmarkCanvasTrans = value; } }

    [Header("NPCの感知距離")]
    [SerializeField]
    private float m_npcNoticePlayerRange = 10.0f;
    public float NPCNoticePlayerRange { get { return m_npcNoticePlayerRange; } set { m_npcNoticePlayerRange = value; } }


    // 途中の振り向きをしないで目的地へと移行するフラグ
    private bool m_noWaitPlayerFlg = false;
    public bool NoWaitPlayerFlg
    {
        get { return m_noWaitPlayerFlg; }
        set { m_noWaitPlayerFlg = value; }
    }


    // 振り向きアニメーションへと移行しないフラグ
    private bool m_bNoChangeTurnAroundFlg = false;
    public bool NoChangeTurnAroundFlg
    {
        get { return m_bNoChangeTurnAroundFlg; }
        set { m_bNoChangeTurnAroundFlg = value; }
    }

    //=================================================
    // NPCの向くキャラクター
    //=================================================
    [SerializeField]
    private SpeakerType m_npcFaceSpeakerType=SpeakerType.None;
    public SpeakerType NPCFaceSpeakerType { get { return m_npcFaceSpeakerType; }set { m_npcFaceSpeakerType = value; } }

    // 会話中に向く方向変更用
    public ReactiveProperty<bool> m_bChangetFaceSpeaker = new ReactiveProperty<bool>();


    //======================================================
    // 会話イベント時に表示するオブジェクト
    //=======================================================
    [SerializeField]
    private SerializableDictionary<string,Transform> m_eventActiveObjectList;

    // 指定オブジェクトを表示する
    public void SetActiveListObj(string keyName)
    {
        if(m_eventActiveObjectList.TryGetValue(keyName,out Transform objTrans))
        {
            objTrans.gameObject.SetActive(true);
        }
    }

    // 全てのオブジェクトを非表示にする
    public void SetNotActiveAllListObj()
    {
        foreach(var obj in m_eventActiveObjectList)
        {
            obj.Value.gameObject.SetActive(false);
        }
    }

}

