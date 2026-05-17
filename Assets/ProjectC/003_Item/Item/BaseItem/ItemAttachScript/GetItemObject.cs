using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;


public class GetItemObject : MonoBehaviour
{

    // 制作者(田内)
    // フラグが変わった時のUI表示（吉田）


    [Header("取得不可UI")]
    [SerializeField]
    private Canvas m_unGetCanvas = null;

    //=================================================
    // 現在採取可能かどうか


    private BoolReactiveProperty m_isGet = new();


    public bool IsGet { get { return m_isGet.Value; } }


    //========================================================
    // フラグが変わった時のUI表示

    [Header("DOスピード")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_doSpead = 0.2f;

    [SerializeField]
    private AppearanceItemName m_appearName = null;

    [SerializeField]
    private AppearanceItemOutline m_appearOutline = null;

    private float m_getDistance = 0.0f;
    private Transform m_player;

    //========================================================
    //                  実行処理
    //========================================================

    private void Start()
    {
        /*a.OnTriggerEnterAsObservable()
            .Where(_ => enabled)
            .Subscribe(collider =>
            {
                GameObject otherObj = collider.gameObject;
            }

        a.OnTriggerExitAsObservable()
      .Where(_ => enabled)
      .Subscribe(collider =>
      {
          GameObject otherObj = collider.gameObject;
      }*/

        m_isGet.Value = false;// 初期化

        if (m_appearName == null)
        {
            Debug.LogError("AppearanceItemNameコンポーネントが登録されていません");
        }
        else
        {
            // 名前表示
            SwitchItemName();
        }

        if (m_appearOutline == null)
        {
            Debug.LogError("AppearanceItemOutlineコンポーネントが登録されていません");
        }
        else
        {
            // アウトライン表示
            SwitchOutline();
        }


        // アイテム取得範囲計算　伊波
        if (TryGetComponent(out Collider myCol))
        {
            m_getDistance = Mathf.Sqrt((myCol.bounds.size.x * myCol.bounds.size.x) + (myCol.bounds.size.z - myCol.bounds.size.z));
        }
        else
        {
            Debug.LogError("アイテムにコライダーが設定されていません" + gameObject.name);
        }

        // プレイヤーの半径追加
        CharacterMeta meta = IMetaAI<CharacterCore>.Instance as CharacterMeta;
        if (meta != null)
        {
            m_player = meta.Player.transform;
            m_getDistance += meta.Player.CharaCtrl.Motor.Capsule.radius;
        }
        else
        {
            Debug.LogError("プレイヤーがCharacterMetaからとれませんでした");
        }

        // TODO:アイテム取得範囲の広さ決め打ちしててよくない
        m_getDistance += 0.8f;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    // 所持できれば
    //    if (!InventoryManager.instance.IsInList()) return;

    //    // 範囲内にいるのがPlayerであれば採取可能に
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        m_isGet.Value = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    // 範囲内から出たのがPlayerであれば採取不可能に
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        m_isGet.Value = false;
    //    }
    //}


    private void Update()
    {
        if (Vector3.Distance(transform.position, m_player.position) <= m_getDistance)
        {
            m_isGet.Value = true;
        }
        else
        {
            m_isGet.Value = false;
        }
    }


    public void GetItem()
    {
        var component = gameObject.GetComponentInParent<AssignItemID>();
        if (component == null)
        {
            Debug.LogError("AssignIngredientIDが存在しないのでIDを取得できません");
            return;
        }


        // インベントリに加える
        if (InventoryManager.instance.AddItem(component.ItemTypeID, component.ItemID))
        {
            // 加えられたらオブジェクトを削除する
            Destroy(gameObject);
        }
        else
        {
            if (m_unGetCanvas != null)
            {
                Instantiate(m_unGetCanvas);
            }
        }
    }


    // 名前表示・非表示切り替え（吉田）
    private void SwitchItemName()
    {
        m_isGet.Subscribe(x =>
        {
            if (x)
            {
                m_appearName.OnShow(m_doSpead);
            }
            else
            {
                m_appearName.OnHide(m_doSpead);
            }
        });
    }

    // アウトライン表示（吉田）
    private void SwitchOutline()
    {
        m_isGet.Subscribe(x =>
        {
            if (x)
            {
                m_appearOutline.OnShow(m_doSpead);
            }
            else
            {
                m_appearOutline.OnHide(m_doSpead);
            }
        });
    }

}
