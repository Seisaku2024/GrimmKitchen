using Arbor;
using UnityEngine;

// 炎はき状態　伊波

public class Fire : MonoBehaviour, ICondition
{
    [Header("炎を吐く時間")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ResistanceID))]
    public float[] m_maxFireTime = new float[(int)ConditionInfo.ResistanceID.ResistanceTypeNum];

    private float m_fireTime;

    [Header("発生させるプレハブ")]
    [SerializeField] private GameObject m_assetAttack;
    [SerializeField] AttackData m_attackData;

    private ConditionInfo.ConditionID m_conditionID = ConditionInfo.ConditionID.Fire;
    public ConditionInfo.ConditionID ConditionID => m_conditionID;

    public GameObject Owner { get; set; }
    private Animator m_animator;
    private ArborFSM m_arborFSM;
    private GameObject m_createdInstance;

    public bool IsEffective() { return m_fireTime > 0.0f; }
    public float DamageMulti(ConditionInfo.ResistanceID[] resistances)
    {
        return 1.0f;
    }
    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances)
    {
        Fire newFire = newCondition as Fire;
        if (newFire == null) return;
        if ((int)m_conditionID < resistances.Length)
        {
            if (!gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager)) return;
            if (m_fireTime < newFire.m_maxFireTime[(int)m_conditionManager.Resistances[(int)m_conditionID]])
            {
                m_fireTime = newFire.m_maxFireTime[(int)m_conditionManager.Resistances[(int)m_conditionID]];
            }
        }
    }

    //private void Awake()
    //{
    //}

    void Start()
    {
        Owner = gameObject;

        ParameterContainer parameterContainer;
        transform.root.TryGetComponent(out parameterContainer);

        if (!parameterContainer.TryGetComponent("Animator", out m_animator))
        {
            Debug.LogError("ParameterContainerにAnimatorがありません" + gameObject.transform.root.name);
        }
        m_animator.SetBool("IsFire", true);
        m_animator.Play("Move", -1, 0f);

        if (gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager))
        {
            m_fireTime = m_maxFireTime[(int)m_conditionManager.Resistances[(int)m_conditionID]];
        }

        // Arborの処理中断
        if (transform.root.TryGetComponent(out m_arborFSM))
        {
            m_arborFSM.Pause();
        }


        // コライダー生成
        var charaCore = m_animator.transform.GetComponentInParent<CharacterCore>();
        m_createdInstance = GameObject.Instantiate(m_assetAttack, m_animator.transform.parent);


        // 作成者情報を記憶
        var ownerInfo = m_createdInstance.AddComponent<OwnerInfoTag>();
        ownerInfo.GroupNo = charaCore.GroupNo;
        ownerInfo.Characore = charaCore;

        // 攻撃の詳細情報をセット
        var attackApplicant = m_createdInstance.AddComponent<AttackApplicant>();
        switch (m_attackData.m_attackerTypeID)
        {
            case AttackerTypeID.enemy:
                {
                    attackApplicant.SetAttackData(EnemyDataBaseManager.instance.GetEnemyData(m_attackData.m_enemyID).AttackData[m_attackData.m_attackType - 1]);
                }
                break;
            default:
                m_fireTime = 0.0f;
                break;
        }

        // コライダーの長さ調整
        if (transform.root.TryGetComponent(out CapsuleCollider capsule))
        {
            float radius = capsule.radius;
            m_createdInstance.transform.localScale = new Vector3(
                m_createdInstance.transform.localScale.x,
                m_createdInstance.transform.localScale.y,
                (radius * 3.0f) + 2);
            m_createdInstance.transform.localPosition = new Vector3(
                m_createdInstance.transform.localPosition.x,
                m_createdInstance.transform.localPosition.y,
                (radius * 3.0f + 2) * 0.5f);

        }
    }


    private void Update()
    {
        // FireState中にカウント
        for (int i = 0; i < m_animator.layerCount; ++i)
        {
            if (!m_animator.GetCurrentAnimatorStateInfo(i).IsName("Fire")) continue;
            m_fireTime -= Time.deltaTime;
            break;
        }
    }

    private void OnDestroy()
    {
        m_animator.SetBool("IsFire", false);
        m_arborFSM.Resume();
        if (m_createdInstance)
        {
            Destroy(m_createdInstance);
        }
    }
}
