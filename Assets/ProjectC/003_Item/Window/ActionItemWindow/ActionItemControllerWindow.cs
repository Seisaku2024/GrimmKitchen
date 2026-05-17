using Cysharp.Threading.Tasks;
using ItemInfo;
using PocketItemDataInfo;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

//新規アクションアイテムウィンドウ（山本）

public class ActionItemControllerWindow : BaseWindow
{
    [Header("スロット作成")]
    [SerializeField]
    private CreateActionItemSlotList m_createSlotList = null;
    public CreateActionItemSlotList CreateSlotList { get { return m_createSlotList; } }


    [Header("UI選択ウィンドウコントローラー")]
    [SerializeField]
    private SelectUIActionWindowController m_selectUIController = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("スプライトの配置場所")]
    [SerializeField]
    private GameObject m_content = null;
    public GameObject ContentTrans { get { return m_content; } }

    [Header("料理のアクションUIコントローラー")]
    [SerializeField] private ActionByFoodTypeController m_foodTypeUIController = null;

    private CharacterCore m_playerCore;

    public override async UniTask OnInitialize()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            if (m_createSlotList == null)
            {
                Debug.LogError("CreateItemSlotListコンポーネントがアタッチされていません");
                return;
            }

            // スロット作成
            await m_createSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            await UniTask.CompletedTask;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    public override async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                if (m_selectUIController == null)
                {
                    Debug.LogError("SlotContorollerコンポーネントがアタッチされていません");
                    return;
                }

                if (m_changeScrollViewPosition == null)
                {
                    Debug.LogError("ChangeScrollViewPosコンポーネントがアタッチされていません");
                    return;
                }


                //ActionItemWindowの情報更新
                SetItemInfomation();

                //スプライトとUIの削除の監視（山本）
                DeleteSprite();

                //スロットとUIリストの作成
                await m_createSlotList.CreateSlot();
                cancelToken.ThrowIfCancellationRequested();

                if (CheckNullUIgameObjList())
                {
                    //TODo
                    //時間停止中は入らないようにする（山本）
                    if (Time.timeScale != 0.0f)
                    {
                        // 投げ中は料理を変更しない
                        if (m_playerCore && !m_playerCore.m_animator.GetBool("IsThrow"))
                        {
                            // UI選択の更新
                            await m_selectUIController.OnUpdate();
                            cancelToken.ThrowIfCancellationRequested();
                        }
                    }

                    // スクロールビューの更新
                    m_changeScrollViewPosition.OnUpdateEveryTime();


                    if (Time.timeScale != 0.0f)
                    {
                        //アイテムアクションへと移行
                        SelectItemAction();
                    }
                }

                // UI選択の後処理
                m_selectUIController.OnLateUpdate();

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    //ActionItemWindowの情報登録
    private void SetItemInfomation()
    {
        foreach (var slot in m_createSlotList.SlotList)
        {
            if (slot == null) continue;
            if (slot.TryGetComponent<ItemSlotData>(out var slotData))
            {
                // 注文アイテムスロットのデータをセット
                slotData.SetItemSlotData(slotData.ItemData, PocketType.Inventory);
            }
        }
    }

    //スプライトとUIの削除の監視（山本）
    private void DeleteSprite()
    {
        //アイテムウィンドウ数の監視とスプライトの削除
        foreach (var obj in m_createSlotList.SlotList)
        {

            if (obj == null)
            {
                continue;
            }

            var _data = obj?.GetComponent<ItemSlotData>();

            if (_data == null)
            {
                break;
            }

            var inventryList = InventoryManager.instance.GetItemNum(_data.ItemData.ItemTypeID, _data.ItemData.ItemID);

            if (inventryList == 0)
            {
                //削除
                Destroy(obj.gameObject);
            }
        }

        var uiList = m_selectUIController.UIList;

        //NullのUIリストを削除する


        for (int i = 0; i < uiList.Count; i++)
        {
            if (uiList[i] == null) continue;

            //削除前のリストのカウント数
            var preDeleteListCount = uiList[i].List.Count;

            //リスト内のnullを削除
            uiList[i].List.RemoveAll(ui => ui == null);

            //削除前後でカウント数が変化していたら
            if (preDeleteListCount != uiList[i].List.Count)
            {
                m_selectUIController.CheckWithChange();
                //カーソル位置の更新
                m_selectUIController.SetUIActionWindowGameObject();
            }
        }



    }


    //UIリスト内にオブジェクトが残っているか確認する処理（山本）
    private bool CheckNullUIgameObjList()
    {
        var uiList = m_selectUIController.UIList;

        bool nullFlg = false;

        for (int i = 0; i < uiList.Count; i++)
        {
            //uiList内のuIGameObjectList内にオブジェクトが含まれていたならtrueを返す
            if (uiList[i].List.Count != 0)
            {
                nullFlg = true;
                break;
            }
        }

        return nullFlg;

    }

    private void SelectItemAction()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーが登録されていません");
            return;
        }


        //UIリストとスロットが空ならリターン
        if ((m_selectUIController.UIList.Count == 0) || (m_createSlotList.SlotList.Count == 0))
        {
            return;
        }

        // 選択しているUIがなければリターン
        if (!m_selectUIController.CurrentSelectUI)
        {
            return;
        }

        // 選択しているUIがスロットになければリターン
        var slotData = m_selectUIController.CurrentSelectUI.GetComponent<ItemSlotData>();
        if (slotData == null)
        {
            return;
        }

        // プレイヤーへアイテム情報を送る処理(吉田)
        if (!m_playerCore)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                return;
            }
            if (!player.TryGetComponent(out m_playerCore)) return;
        }

        var animator = m_playerCore.m_animator;
        if (animator == null)
        {
            return;
        }

        //選択したアイテム情報をプレイヤーに渡す
        m_playerCore.PlayerParameters.SetPutItemInfo(slotData.ItemData.ItemTypeID, slotData.ItemData.ItemID);

        // UI側に操作方法を反映
        FoodData data = ItemDataBaseManager.instance.GetItemData<FoodData>(slotData.ItemData.ItemTypeID, slotData.ItemData.ItemID);
        m_foodTypeUIController.SetFoodType(data.GetFoodType());

        //料理を置く
        if (m_selectUIController.IsPut)
        {
            if (!m_foodTypeUIController.CheckAbleFoodActionType(ActionByFoodTypeController.FoodActionType.putFood)) return;
            if (!animator.GetBool("IsPutItem"))
            {
                animator.SetBool("IsPutItem", true);
                //武器所持状態なら武器を消失
                m_playerCore.PlayerParameters.HideWeapon(animator);
                return;
            }
        }

        //料理を置く
        if (m_selectUIController.IsUse)
        {
            if (data == null)
            {
                return;
            }
            if (!m_foodTypeUIController.CheckAbleFoodActionType(ActionByFoodTypeController.FoodActionType.eatFood)) return;

            //食べられるなら食べる状態へ移行
            if (/*(data.IsFoodType(FoodData.FoodType.DebuffCondition) == false)
                    &&*/ !animator.GetBool("IsEatItem"))
            {
                animator.SetBool("IsEatItem", true);
                //武器所持状態なら武器を消失
                m_playerCore.PlayerParameters.HideWeapon(animator);
                return;
            }
            else
            {
                return;
            }
        }

        //料理を投げる
        if (m_selectUIController.IsThrow)
        {
            if (!m_foodTypeUIController.CheckAbleFoodActionType(ActionByFoodTypeController.FoodActionType.throwFood)) return;
            if (!animator.GetBool("IsThrow"))
            {
                animator.SetBool("IsThrow", true);
                //武器所持状態なら武器を消失
                m_playerCore.PlayerParameters.HideWeapon(animator);
                return;
            }
        }
    }

    private void Start()
    {
        if (!m_playerCore)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                return;
            }
            if (!player.TryGetComponent(out m_playerCore)) return;
        }
    }
}



