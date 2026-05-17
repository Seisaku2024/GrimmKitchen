using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class SelectKeepUIController : MonoBehaviour
{
    // 制作者 田内
    // SelectUIControllerで選択したUIを保持するコントローラー、再度選択で解除
    // ※SelectKeepUIDataコンポーネントをアタッチすること


    [Header("UI選択コントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;

    [Header("全て選択ボタン")]
    [SerializeField]
    protected InputActionButton m_selectAllInputActionButton = null;

    [Header("全て追加するUI")]
    [SerializeField]
    private Canvas m_selectAllCanvas = null;

    [Header("全て取り除くUI")]
    [SerializeField]
    private Canvas m_removeAllCanvas = null;


    [Header("選択数が最大を超えた際に表示するUI")]
    [SerializeField]
    protected Canvas m_maxSelectableUI = null;


    [Header("初期化時全て選択")]
    [SerializeField]
    private bool m_initalizeSelectAll = false;


    // キープUIリスト
    protected List<GameObject> m_selectKeepObjectList = new();

    // 選択可能最大数(負の数であれば上限なし)
    protected int m_maxSelectableNumber = -1;

    //==============================================
    //                  実行処理
    //==============================================


    /// <summary>
    /// 初期化処理
    /// </summary>
    virtual public async UniTask OnInitialize()
    {

        SetMaxSelectableNumber();

        if (m_initalizeSelectAll) SelectAll();

        await UniTask.CompletedTask;
    }



    /// <summary>
    /// 実行処理
    /// </summary>
    virtual public async UniTask OnUpdate()
    {
        SelectAllInputActionButton();
        SelectKeep();
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 後実行処理
    /// </summary>
    virtual public void OnLateUpdate()
    {

    }


    virtual protected void SelectAllInputActionButton()
    {
        if (m_selectAllInputActionButton == null) return;

        if (m_selectAllInputActionButton.IsInputActionTrriger())
        {
            SelectAll();
        }
    }

    /// <summary>
    /// 全選択(SelectUIControllerのList順に追加)
    /// </summary>
    virtual protected void SelectAll()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        var list = m_selectUIController.GetAllUI();
        foreach (var data in list)
        {
            // 既に追加されていれば無視
            if (IsAdded(data))
            {
            }
            // まだ追加されていなければ追加
            else if (IsSelectKeep(data) == true)
            {
                // 追加
                AddSelectKeepObject(data);
            }
        }
    }


    /// <summary>
    /// 選択したUIを保存する/保存を取り除く
    /// </summary>
    virtual protected void SelectKeep()
    {

        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        // 選択されれば
        if (m_selectUIController.IsPress == false) return;

        // 選択中のUI
        var currentUI = m_selectUIController.CurrentSelectUI;
        if (currentUI == null) return;

        // 既に追加されていれば取り除く
        if (IsAdded(currentUI))
        {

            RemoveSelectKeepObject(currentUI);
            return;
        }
        // まだ追加されていなければ追加
        else
        {
            // 選択することができなければ
            if (IsSelectKeep(currentUI) == false) return;

            AddSelectKeepObject(currentUI);
            return;
        }
    }


    virtual protected void AddSelectKeepObject(GameObject _obj)
    {
        SetSelectKeepUIData(_obj, true);
        m_selectKeepObjectList.Add(_obj);

    }

    virtual protected void RemoveSelectKeepObject(GameObject _obj)
    {
        SetSelectKeepUIData(_obj, false);
        m_selectKeepObjectList.Remove(_obj);

    }


    /// <summary>
    /// 選択可能数をセット(デフォルトは無制限に選択可能)
    /// </summary>
    virtual protected void SetMaxSelectableNumber()
    {
        m_maxSelectableNumber = -1;
    }


    /// <summary>
    /// 既に追加されているかどうか確認する
    /// </summary>
    protected bool IsAdded(GameObject _obj)
    {
        foreach (var obj in m_selectKeepObjectList)
        {
            if (obj == _obj) return true;
        }
        return false;
    }



    /// <summary>
    /// 選択することができるかどうか
    /// </summary>
    virtual protected bool IsSelectKeep(GameObject _ui)
    {
        // 選択UIが最大数以上であれば追加しない
        if (0 <= m_maxSelectableNumber && m_maxSelectableNumber <= m_selectKeepObjectList.Count)
        {
            // UI作成
            MaxSelectableUI();
            return false;
        }

        return true;
    }



    /// <summary>
    /// 最大数UIを作成
    /// </summary>
    protected void MaxSelectableUI()
    {
        if (m_maxSelectableUI == null) return;
        Instantiate(m_maxSelectableUI);
    }



    /// <summary>
    /// SelectKeepUIDataがあればデータを更新する
    /// </summary>
    protected void SetSelectKeepUIData(GameObject _obj, bool _active)
    {
        if (_obj == null) return;
        if (_obj.TryGetComponent<SelectKeepUIData>(out var data))
        {
            // 引数オブジェクトを基にデータを更新する
            data.SetData(_active);
        }
    }



}
