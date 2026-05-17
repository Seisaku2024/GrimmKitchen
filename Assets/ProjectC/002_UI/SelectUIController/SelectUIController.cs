using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ButtonInfo;
using SelectUIInfo;

using Cysharp.Threading.Tasks;
using NaughtyAttributes;


[DefaultExecutionOrder(-1000)]
public partial class SelectUIController : MonoBehaviour
{

    // 制作者 田内
    // UIを選択するコントローラー

    //===========================================
    // 登録したUIリスト


    [System.Serializable]
    public class UIDataList
    {
        [SerializeField]
        public List<UIData> m_list = new();

        public List<UIData> List
        {
            get { return m_list; }
            set { m_list = value; }
        }
    }

    [System.Serializable]
    public class UIData
    {
        // UI本体のGameObject
        [SerializeField]
        private GameObject m_ui = null;

        public GameObject UI
        {
            get { return m_ui; }
            set { m_ui = value; }
        }

        // 選択種類
        [SerializeField]
        private SelectUIType m_selectUIType = SelectUIType.Press;

        public SelectUIType SelectUIType
        {
            get { return m_selectUIType; }
            set { m_selectUIType = value; }
        }

        [Foldout("SE")]
        [Header("PressSE")]
        [SerializeField]
        private string m_pressSoundPath = "Decide";

        public string PressSoundPath
        {
            get { return m_pressSoundPath; }
        }

        // 追加：吉田
        private ButtonData m_buttonData = null;
        public ButtonData ButtonData
        {
            get { return m_buttonData; }
            set { m_buttonData = value; }
        }
    }



    [Header("管理するUI")]
    [Header("※:上から順に選択できるようになります。順番を間違えないようにしてください。")]
    [SerializeField]
    protected List<UIDataList> m_uiList = new();


    public List<UIDataList> UIList { get { return m_uiList; } set { m_uiList = value; } }

    //========================================================================================

    [Header("ループするかどうか")]
    [SerializeField]
    protected bool m_isLoop = true;

    //========================================================================================

    protected enum SelectUIInterpolation
    {
        Exceed,         // 超えて移動
        Interpolation,  // 補間して移動
    }

    [Header("補間タイプ")]
    [SerializeField]
    protected SelectUIInterpolation m_interpolationType = SelectUIInterpolation.Exceed;

    //======================================================================

    // 常に動的に追加される場合用列挙型
    protected enum AlwaysCreateType
    {
        Disable, // 常に追加されない
        Enable,  // 常に追加される
    }


    [Header("UI生成タイプ")]
    [SerializeField]
    protected AlwaysCreateType m_alwaysCreateType = AlwaysCreateType.Disable;

    [Header("決定キーでスケールアニメーションするか")]
    [SerializeField]
    private bool m_isPressScale = true;


    //===========================================
    // 現在選択しているUI

    protected UIData m_currentSelectUIData = null;

    public UIData CurrentSelectUIData { get { return m_currentSelectUIData; } }

    public GameObject CurrentSelectUI
    {
        get
        {
            if (m_currentSelectUIData == null) return null;
            return m_currentSelectUIData.UI;
        }
    }


    //=============================================
    // 現在確認している番号

    // 横
    protected int m_currentWidth = 0;

    // 縦
    protected int m_currentHeight = 0;

    //======================
    // 現在の行数

    protected int m_currentConstraintCount = 0;

    //=============================================
    // 選択しているUIが変更されたかどうか


    protected bool m_isSelectChangeFlg = false;

    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// 現在選択しているスロットが変更されたかどうか確認する、読み取り専用プロパティ
    /// </summary>
    /// --------------------------------------
    /// <return>
    /// true - 変更された
    /// false - 変更されていない
    /// </return>
    /// --------------------------------------
    #endregion
    public bool IsSelectChangeFlg { get { return m_isSelectChangeFlg; } set { m_isSelectChangeFlg = true; } }


    //========================================================
    // スロットの列数(横移動に使用)
    //（山本）

    protected int m_listCount = 0;

    //====================
    // ボタンを押したか


    protected bool m_isPress = false;

    public bool IsPress { get { return m_isPress; } }

    //========================================================
    //                          実行処理
    //========================================================


    virtual public void Start()
    {
        CheckButtonData();

        // 初期選択UIをセット
        SetHeadUIGameObject();
    }


    virtual public async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_alwaysCreateType == AlwaysCreateType.Enable)
            {
                NullCheck();
            }

            // UIを選択する
            SelectUI();

            // UIのキー操作判定
            await OnUpdateInput();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    virtual public void OnLateUpdate()
    {
        m_isSelectChangeFlg = false;
        m_isPress = false;
    }

    //============================================
    //              実行処理
    //============================================


    /// <summary>
    /// 選択したUIにボタンIDがあれば返す
    /// </summary>
    public ButtonID IsPressButton()
    {
        ButtonID id = ButtonID.None;

        if (!m_isPress) return id;

        // ボタン情報を取得
        if (m_currentSelectUIData.UI.TryGetComponent<ButtonData>(out var buttonData) == false)
        {
            // 無ければNone
            return ButtonID.None;
        }

        // 使用可能であれば
        if (buttonData.IsUse)
        {
            id = buttonData.ButtonID;
        }

        return id;

    }


    #region メソッド説明
    ///--------------------------------------
    /// <summary>
    /// スロットリストに引数リストを追加する
    /// </summary>
    /// --------------------------------------
    #endregion
    virtual public void AddUI(GameObject _ui, SelectUIType _type, int _lineBreak)
    {
        if (_ui == null) return;


        // 行数を超えたら
        if (m_uiList.Count == 0 || _lineBreak <= m_uiList[m_uiList.Count - 1].List.Count)
        {
            m_uiList.Add(new());
        }


        // リストに追加
        UIData data = new();
        data.UI = _ui;
        data.SelectUIType = _type;

        m_uiList[m_uiList.Count - 1].List.Add(data);


        SetHeadUIGameObject();

    }

    /// <summary>
    /// 一段下にする（追加：吉田）
    /// どこかで処理あったきがする 
    /// </summary>
    public void AddBreakUI()
    {
        m_uiList.Add(new());
    }

    public void ResetUIList()
    {
        m_uiList.Clear();
    }

    /// <summary>
    /// 全てのUIを取得
    /// </summary>
    public List<GameObject> GetAllUI()
    {
        List<GameObject> list = new();

        foreach (var height in m_uiList)
        {
            if (height == null || height.List == null) continue;
            foreach (var width in height.List)
            {
                if (width == null || width.UI == null) continue;
                list.Add(width.UI);
            }
        }
        return list;
    }


    // 初期選択UIをセット
    virtual public void SetHeadUIGameObject()
    {
        // 既にセットされていれば
        if (m_currentSelectUIData != null) return; ;


        foreach (var uiList in m_uiList)
        {
            foreach (var uiData in uiList.List)
            {
                // UIをセット
                m_currentSelectUIData = uiData;

                m_currentHeight = m_uiList.IndexOf(uiList);
                m_currentWidth = uiList.List.IndexOf(uiData);

                ButtonDataSelectUI();
                ButtonDataUnselectUI();

                // 変更
                m_isSelectChangeFlg = true;

                return;
            }

        }
    }


    public void SetUIActionWindowGameObject()
    {
        if (m_currentSelectUIData != null)
        {
            return;
        }

        if (m_uiList[0].List.Count > m_currentWidth)
        {
            m_currentSelectUIData = m_uiList[0].List[m_currentWidth];
        }
        else
        {
            SetHeadUIGameObject();
        }
    }




    /// <summary>
    /// nullのUIを取り除き、現在選択中のUIが配列外でないか確認する
    /// </summary>
    public void NullCheck()
    {
        // 存在しなければ
        if (m_uiList.Count <= 0) return;

        // 横
        foreach (var uiList in m_uiList)
        {
            // nullのUIを全て取り除く
            uiList.List.RemoveAll(_ => _ == null || _.UI == null);

        }

        // 縦
        m_uiList.RemoveAll(_ => _ == null || _.List.Count <= 0);

        CurrentUISelectCheck();

    }

    private void CurrentUISelectCheck()
    {
        try
        {
            // 存在しなければ
            if (m_uiList.Count <= 0) return;


            // 選択中の高さがリストの数を越えていれば
            int heightCount = m_uiList.Count;
            if (heightCount <= m_currentHeight)
            {
                m_currentHeight = heightCount - 1;
                m_isSelectChangeFlg = true;
            }

            // 選択中の横がリストの数を越えていれば
            int widthCount = m_uiList[m_currentHeight].List.Count;
            if (widthCount <= m_currentWidth)
            {
                m_currentWidth = widthCount - 1;
                m_isSelectChangeFlg = true;
            }


            m_currentSelectUIData = m_uiList[m_currentHeight].List[m_currentWidth];

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // --- ButtonDataからの処理（吉田） -----------------------------------

    /// <summary>
    /// ButtonDataを取得する（吉田）
    /// なくても問題ないので、警告は出さない
    /// </summary>
    private void CheckButtonData()
    {
        foreach (var uiList in m_uiList)
        {
            foreach (var uiData in uiList.List)
            {
                if (uiData.UI == null) continue;

                if (uiData.UI.TryGetComponent<ButtonData>(out var ret))
                {
                    uiData.ButtonData = ret;
                }
            }
        }
    }

    /// <summary>
    /// 選択されたButtonDataのSelectUpdateを呼び出す（吉田）
    /// </summary>
    protected void ButtonDataSelectUI()
    {
        if (m_currentSelectUIData.ButtonData == null) return;

        if (m_currentSelectUIData.ButtonData.IsUse)
        {
            m_currentSelectUIData.ButtonData.OnSelectUpdate();
        }
    }

    /// <summary>
    /// 選択されていないButtonDataのUnSelectUpdateを呼び出す（吉田）
    /// </summary>
    protected void ButtonDataUnselectUI()
    {
        foreach (var uiList in m_uiList)
        {
            foreach (var uiData in uiList.List)
            {
                if (uiData == m_currentSelectUIData) continue;
                if (uiData.ButtonData == null) continue;

                if (uiData.ButtonData.IsUse)
                {
                    uiData.ButtonData.OnUnselectUpdate();
                }
            }
        }
    }

    /// <summary>
    /// すべてのボタンを非選択にする（吉田）
    /// </summary>
    public void ButtonDataUnselectAllUI()
    {
        foreach (var uiList in m_uiList)
        {
            foreach (var uiData in uiList.List)
            {
                if (uiData.ButtonData == null) continue;

                if (uiData.ButtonData.IsUse)
                {
                    uiData.ButtonData.OnUnselectUpdate();
                }
            }
        }
    }


    // スライダー選択時にスライダーの増減を制御できるようにする処理（山本）
    public async void UpdateSlider()
    {
        if (m_currentSelectUIData == null) return;

        if (m_currentSelectUIData.UI.TryGetComponent<ValueController>(out var valueController) == false)
        {
            return;
        }

        // 選択されたUIのImageの色変更
        if (m_currentSelectUIData.UI.TryGetComponent(out ChangeSelectImageColor imageColor))
        {
            imageColor.ChangeSelectColor();
        }

        // 選択されてないUIのImageの色変更
        foreach (var uiList in m_uiList)
        {
            foreach (var uiData in uiList.List)
            {
                if (uiData == m_currentSelectUIData) continue;
                if (uiData.UI.TryGetComponent<ChangeSelectImageColor>(out var color) == false) continue;

                color.ChangeNotSelectColor();
            }
        }

        await valueController.OnUpdate();

    }

    // スライダーの色を一括で選択してない色へと変化させる処理（山本）
    public void SetSliderNoSelectColor()
    {
        // 選択されてないUIのImageの色変更
        foreach (var uiList in m_uiList)
        {
            foreach (var uiData in uiList.List)
            {
                if (uiData.UI.TryGetComponent<ChangeSelectImageColor>(out var color) == false) continue;

                color.ChangeNotSelectColor();
            }
        }
    }


}
