using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class ShotBubble : MonoBehaviour
{
    [SerializeField] private AttackData m_attackData;

    [SerializeField] private GameObject m_bubblePrefab;
    [SerializeField, Range(1, 10)] private int m_bubbleNum;
    [SerializeField, Range(0, 3)] private float m_shotTime;
    [SerializeField, Range(1, 50)] private float m_shotAreaRange;

    [SerializeField] private float m_dropTime;

    private CharacterCore charaCore;

    async void Start()
    {
        if(!m_bubblePrefab)
        {
            Debug.LogError("発射するプレハブがSerializeされていません", gameObject);
        }

        charaCore = transform.root.GetComponentInParent<CharacterCore>();

        Vector3 endPos=new();
        Vector3 pos = transform.position;
        for (int i = 0; i < m_bubbleNum; ++i)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(m_shotTime));

            endPos = pos;
            endPos.x += UnityEngine.Random.Range(-m_shotAreaRange, m_shotAreaRange);
            endPos.z += UnityEngine.Random.Range(-m_shotAreaRange, m_shotAreaRange);
            GameObject obj = CreateBubble(pos);
            Shot(obj, pos, endPos);
            obj.transform.localScale = new(0.01f, 0.01f, 0.01f);
            obj.transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
        }
    }

    private GameObject CreateBubble(Vector3 pos)
    {
        // オブジェクト生成
        GameObject obj = Instantiate(m_bubblePrefab, pos, Quaternion.identity);


        // 作成者情報を記憶
        var ownerInfo = obj.AddComponent<OwnerInfoTag>();
        ownerInfo.GroupNo = charaCore.GroupNo;
        ownerInfo.Characore = charaCore;

        // 攻撃の詳細情報をセット
        var attackApplicant = obj.AddComponent<AttackApplicant>();
        switch (m_attackData.m_attackerTypeID)
        {
            case AttackerTypeID.player:
                {
                    attackApplicant.SetAttackData(PlayerDataBaseManager.instance.DataBase.AttackData[m_attackData.m_attackType - 1]);
                }
                break;
            case AttackerTypeID.storySkill:
                {
                    attackApplicant.SetAttackData(StorySkillDataBaseManager.instance.GetStorySkillData(m_attackData.m_storySkillID).AttackDamageDatas[m_attackData.m_attackType - 1]);
                }
                break;
            case AttackerTypeID.enemy:
                {
                    attackApplicant.SetAttackData(EnemyDataBaseManager.instance.GetEnemyData(m_attackData.m_enemyID).AttackData[m_attackData.m_attackType - 1]);
                }
                break;
        }
        return obj;
    }

    private void Shot(GameObject obj, Vector3 _startpos, Vector3 _endPos)
    {
        //　開始位置と到着位置の長さを計算
        var adjacent = Vector3.Distance(
                _startpos,
                _endPos);

        //　落下距離を計算
        //　0.5 * 重力 * 到達時間の2乗
        var opposite = Mathf.Abs(
            0.5f * Physics.gravity.y * m_dropTime * m_dropTime);

        //　到達点＋上向きに落下距離 放物線の頂点
        var upPos = _endPos + Vector3.up * opposite;

        //　斜辺の長さを計算
        var hypotenuse = Vector3.Distance(
            _startpos, upPos);

        //　角度を計算
        //　余弦定理
        float theta = -Mathf.Acos(
            (Mathf.Pow(hypotenuse, 2) + Mathf.Pow(adjacent, 2) - Mathf.Pow(opposite, 2))
            / (2 * hypotenuse * adjacent));

        //　横軸のspeedから斜め方向の速さを計算
        float hypotenuseSpeed = hypotenuse / m_dropTime;

        //　一旦攻撃対象の方を見る
        obj.transform.LookAt(_endPos);
        ////　砲台の高さ向きを変える
        obj.transform.Rotate(
            Vector3.right, theta * Mathf.Rad2Deg, Space.Self);
        
  
        if(obj.TryGetComponent(out Rigidbody rigid))
        {
            rigid.constraints = RigidbodyConstraints.None;
            rigid.useGravity = true;
            rigid.AddForce(obj.transform.forward * hypotenuseSpeed, ForceMode.Impulse);
        }
    }
}
