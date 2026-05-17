using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

// キャラクターを監視できるMetaAI（伊波）

public class CharacterMeta : MonoBehaviour, IMetaAI<CharacterCore>
{
    private void Awake()
    {
        if (IMetaAI<CharacterCore>.Instance == null)
        {
            IMetaAI<CharacterCore>.Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    HashSet<CharacterCore> _characterList = new();

    public HashSet<CharacterCore> ObjectList => _characterList;

    HashSet<CharacterCore> _bossList = new();
    public HashSet<CharacterCore> BossList => _bossList;

    HashSet<CharacterCore> _enemyList = new();
    public HashSet<CharacterCore> EnemyList => _enemyList;

    private CharacterCore m_player;
    public CharacterCore Player => m_player;

    public void RegisterObject(CharacterCore chara)
    {
        if (chara == null) { return; }
        if (_characterList.Add(chara))
        {
            // 敵の移動優先順位を決定 (伊波)
            // TODO:シーン切替時にpriorityリセットしたい
            if (chara.TryGetComponent(out NavMeshAgent agent))
            {
                if (chara.TryGetComponent(out EnemyParameters eParam))
                {
                    if (eParam.IsBoss)
                    {
                        _bossList.Add(chara);
                        agent.avoidancePriority = _bossList.Count;
                    }
                    else
                    {
                        // priorityは0～99が有効
                        int priority = _characterList.Count + 10;
                        while (priority >= 100) priority += -(100 - _bossList.Count);
                        agent.avoidancePriority = priority;
                    }
                    _enemyList.Add(chara);
                }
            }

            // プレイヤーにアクセスしやすいようにキャッシュ
            if (chara.TryGetComponent(out PlayerParameters playerParam))
            {
                m_player = chara;
            }

            chara.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    _characterList.Remove(chara);
                    if (_bossList.Contains(chara)) _bossList.Remove(chara);
                    if (_enemyList.Contains(chara)) _enemyList.Remove(chara);

                }).AddTo(this);
        }
    }
}
