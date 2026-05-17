using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SelectUIInfo;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(GridLayoutGroup))]
public class BaseCreateSlotList : MonoBehaviour
{

    // 制作者(田内)

    //===================================================================================

    protected enum CreateSlotType
    {
        AddSelectUIController,      // コントローラーに追加する
        NotAddSelectUIController,   // コントローラーに追加しない
    }

    [Header("UIコントローラー種類")]
    [SerializeField]
    protected CreateSlotType m_createSlotType = CreateSlotType.AddSelectUIController;

    [Header("UI選択コントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;

    //==========================
    // 改行値
    private int m_lineBreak = 1;

    //====================================================================================


    [Header("作成するスロットプレハブ")]
    [SerializeField]
    protected GameObject m_slot = null;


    //===================================================================================

    // 作成したスロット
    protected List<GameObject> m_slotList = new();

    public List<GameObject> SlotList { get { return m_slotList; } }


    //==========================================================
    //                   実行処理
    //==========================================================

    virtual public async UniTask OnInitialize()
    {
        var cancelToken = this.destroyCancellationToken;
        try
        {
            // 改行値をセット
            SetLineBreak();

            // スロットを作成
            await CreateSlot();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    public async UniTask CreateSlot()
    {
        var cancelToken = this.destroyCancellationToken;
        try
        {
            // ここでスロットを作成
            await CreateSlotInstance();
            cancelToken.ThrowIfCancellationRequested();

            // Nullのオブジェクトを取り除く
            RemoveNullSlotList();
            await UniTask.CompletedTask;
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    virtual public void DestroyItemSlotData(GameObject _data)
    {
        if (_data == null) return;

        // 当てはまるデータを取り除く
        m_slotList.Remove(_data);

        // スロットを削除する
        Destroy(_data.gameObject);

    }


    virtual public void RemoveItemSlotData(GameObject _data)
    {
        if (_data == null) return;

        // 当てはまるデータを取り除く
        m_slotList.Remove(_data);
    }


    // Nullのスロットを削除する
    virtual protected void RemoveNullSlotList()
    {
        // nullのスロットをすべて取り除く
        m_slotList.RemoveAll(slot => slot == null);
    }


    // 改行値をセット
    virtual protected void SetLineBreak()
    {
        if (m_createSlotType != CreateSlotType.AddSelectUIController) return;
        if (m_selectUIController == null)
        {
            Debug.LogError("追加するSelectUIControllerがシリアライズされていません");
            return;
        }

        if (gameObject.TryGetComponent<GridLayoutGroup>(out var grid))
        {
            m_lineBreak = grid.constraintCount;
        }
    }


    // 作成したスロットをコントローラーに追加する
    virtual protected void AddSelectUIControler(GameObject _addObject)
    {
        if (_addObject == null) return;

        if (m_createSlotType != CreateSlotType.AddSelectUIController) return;
        if (m_selectUIController == null)
        {
            Debug.LogError("追加するSelectUIControllerがシリアライズされていません");
            return;
        }

        // スロットコントローラーにセット
        m_selectUIController.AddUI(_addObject, SelectUIType.Press, m_lineBreak);

    }


    public void DestroySlotList()
    {// スロットが削除されないところで終わってるよ

        foreach (var data in m_slotList)
        {
            if (data == null) continue;

            // 削除する
            Destroy(data.gameObject);
        }

        // 初期化
        m_slotList.Clear();

    }

    // スロットのinstanceを作成する
    // 派生クラスはこのメソッドをoverrideしてください
    virtual protected async UniTask CreateSlotInstance()
    {
        Debug.LogError("オーバーライドしてください");
        await UniTask.CompletedTask;
    }

}

