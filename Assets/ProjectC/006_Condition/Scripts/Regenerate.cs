using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// リジェネ状態

public class Regenerate : MonoBehaviour, ICondition
{
    [Header("効果時間")]
    [SerializeField] private float m_time;

    [Header("回復するまでの時間")]
    [SerializeField] private float m_regeneTime;

    [Header("HP回復割合")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ResistanceID))]
    public float[] m_maxCureRate = new float[(int)ConditionInfo.ResistanceID.ResistanceTypeNum];

    [Header("エフェクト")]
    [SerializeField] private GameObject m_effectAssetPrefab;
    private GameObject m_effect;

    //ShareNode存在時、指定名称の部位からエフェクトを発生させる（山本）
    [Header("ShareNodeに登録された部位の名称")]
    [SerializeField] private string m_parent;

    private ConditionInfo.ConditionID m_conditionID = ConditionInfo.ConditionID.Regenerarte;
    public ConditionInfo.ConditionID ConditionID => m_conditionID;

    
    private float m_conditionTime = 0.0f;

    public GameObject Owner { get; set; }
    private ConditionManager m_conditionManager;

    public bool IsEffective() 
    {
        if (m_time > 0)
            return true;

        return false;
    }

    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances)
    {
        Regenerate regenerate = newCondition as Regenerate;
        if (regenerate == null) return;

        if ((int)m_conditionID < resistances.Length)
        {
            // NullCheck（山本）
            if (m_conditionManager == null)
            {
                return;
            }

            float nowSumRegene = m_maxCureRate[(int)m_conditionManager.Resistances[(int)m_conditionID]];
            float newSumRegebe = regenerate.m_maxCureRate[(int)m_conditionManager.Resistances[(int)m_conditionID]];
            if (nowSumRegene < newSumRegebe)
            {
                m_time = regenerate.m_time;
                m_maxCureRate = regenerate.m_maxCureRate;
                m_regeneTime = regenerate.m_regeneTime;
            }
        }

    }


    public float DamageMulti(ConditionInfo.ResistanceID[] resistances) { return 1.0f; }

    void Start()
    {
        Owner = gameObject;
        gameObject.transform.parent.TryGetComponent(out m_conditionManager);

        // エフェクトを出す
        // 一番親オブジェクト(敵本体)に出す
        var rootTrans = transform.root;

        //ShareNodeが存在する場合はShareNodeの指定部位の座標にエフェクトを出現させる（山本）
        if (m_parent.Length != 0)
        {
            if (rootTrans.TryGetComponent(out ShareNodes comp))
            {
                if (comp.Nodes.TryGetValue(m_parent, out Transform parent))
                {
                    m_effect = Instantiate(m_effectAssetPrefab, parent);
                }
                else
                {
                    Debug.LogError("指定名称がShareNodeに存在しません。登録してください。");
                }
            }
            else
            {
                Debug.LogError("ShareNodesコンポーネントが見つかりませんでした");
            }
        }
        else
        {
            m_effect = Instantiate(m_effectAssetPrefab, rootTrans);
        }

    }

    public void Update()
    {
        if (!IsEffective())
        {
            return;
        }
        m_time -= Time.deltaTime;

        m_conditionTime += Time.deltaTime;

        if (m_conditionTime > m_regeneTime)
        {
            m_conditionTime = 0.0f;
            if(m_conditionManager.transform.root.TryGetComponent(out CharacterCore characterCore))
            {
                var cureRate = m_maxCureRate[(int)m_conditionManager.Resistances[(int)m_conditionID]];
                characterCore.Heal(cureRate * characterCore.Status.MaxHP.Value);
            }
        }

    }

    private void OnDestroy()
    {
        //毒エフェクトを残さないように削除（山本）
        if (m_effect)
        {
            Destroy(m_effect);
        }

    }

}
