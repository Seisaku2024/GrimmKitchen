using Arbor.Calculators;
using nTools.PrefabPainter;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingNPCCharacter : MonoBehaviour
{
    [Header("設置するNPCのストーリータイプ")]
    [SerializeField]
    private StoryType m_storyType = StoryType.None;

    [Header("会話フェーズごとの出現確立")]
    [SerializeField]
    private SerializableDictionary<TalkingFase, int> m_faseRandomNumList;

    [Header("出現する位置座標のList")]
    [Header("*このリスト内から抽選される")]
    [SerializeField]
    private List<Transform> m_transformsList = new List<Transform>();

    private void Start()
    {
        Setting();
    }

    private void Setting()
    {
        var data = ConversationDataBaseManager.instance.GetConvaersationData(m_storyType);
        if (data == null) return;

        if (m_faseRandomNumList.ContainsKey(data.TalkingFase) == false) return;

        var randomNum = m_faseRandomNumList[data.TalkingFase];
        if (randomNum < Random.Range(0, 100)) return;

        GameObject npc = data.NPCPrefab;
        if (npc == null) return;

        RaycastHit hit = new();

        if (m_transformsList.Count == 0) return;
        int maxListNum = m_transformsList.Count;
        int randomListNum =Random.Range(0, maxListNum);

        Transform transform = m_transformsList[randomListNum];

        if (RayHitPosition(transform.position, out hit))
        {
            // インスタンスを作成する
            npc = Instantiate(npc, hit.point,Quaternion.identity);

            if(npc.TryGetComponent(out CharacterCore characterCore))
            {
                characterCore.CharaCtrl.SetPositionMotor(hit.point);
            }

        }
        else
        {
            Debug.LogError("設置できませんでした");
        }



    }

    private bool RayHitPosition(Vector3 _startPos, out RaycastHit _hit)
    {
        // 方向
        Vector3 dir = Vector3.down;

        // レイ作成
        Ray ray = new Ray(_startPos, dir);
        if (Physics.Raycast(ray, out _hit, 100.0f))
        {
            //Debug.Log("当たり判定しました");
            return true;
        }
        else
        {
            //Debug.Log("当たり判定できませんでした");
            return false;
        }

    }


}
