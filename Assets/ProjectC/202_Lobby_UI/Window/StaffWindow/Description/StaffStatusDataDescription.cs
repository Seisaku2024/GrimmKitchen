using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UniRx;

public class StaffStatusDataDescription : MonoBehaviour
{
    // スタッフステータスの説明文表示


    [Header("=================================")]
    [Header("スタッフポイント画像")]
    [SerializeField]
    private Image m_staffPointImage = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_staffPointImageList = new();

    [Header("=================================")]
    [Header("スタッフ共通名")]
    [SerializeField]
    private TextMeshProUGUI m_staffCommonNameText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_staffCommonNameTextList = new();

    [Header("=================================")]
    [Header("スタッフ名")]
    [SerializeField]
    private TextMeshProUGUI m_staffNameText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_staffNameTextList = new();

    [Header("=================================")]
    [Header("スタッフ画像")]
    [SerializeField]
    private Image m_staffImage = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_staffImageList = new();

    [Header("=================================")]
    [Header("スタッフの背景画像")]
    [SerializeField]
    private Image m_staffBackImage = null;

    [Header("各レアリティの背景色")]
    [SerializeField]
    private Color[] m_rarityColor = new Color[3];


    [Header("=================================")]
    [Header("スタッフ無の警告文")]
    [SerializeField]
    private TextMeshProUGUI m_noStaffText = null;


    [Header("=================================")]
    [Header("雇用金額")]
    [SerializeField]
    private TextMeshProUGUI m_employmentPriceText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_employmentPriceTextList = new();

    [Header("=================================")]
    [Header("給料金額")]
    [SerializeField]
    private TextMeshProUGUI m_salaryPriceText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_salaryPriceTextList = new();

    [Header("=================================")]
    [Header("スタッフ 配膳値")]
    [SerializeField]
    private TextMeshProUGUI m_staffProvideValueText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_staffProvideValueTextList = new();


    [Header("=================================")]
    [Header("スタッフ 料理値")]
    [SerializeField]
    private TextMeshProUGUI m_staffCookingValueText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_staffCookingValueTextList = new();


    [Header("=================================")]
    [Header("スタッフ 接客値")]
    [SerializeField]
    private TextMeshProUGUI m_staffServiceValueText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_staffServiceValueTextList = new();

    [Header("=================================")]
    [Header("スタッフのガチャエフェクト")]
    [SerializeField]
    private Image m_effectImage = null;

    [Header("レアリティに応じたマテリアル")]
    [SerializeField]
    private Material[] m_effectMaterials = new Material[3];


    // 選択中のステータスデータ
    protected StaffStatusData m_currentSelectStaffStatusData = null;




    //==============================================
    //              実行処理
    //==============================================

    private void Start()
    {
        // 更新があれば
        MessageBroker.Default.Receive<StaffStatusData.GlobalChangeStaffStatusDataEvent>().Subscribe(_ =>
        {
            // 一致すれば更新
            if (_.StaffStatusData == m_currentSelectStaffStatusData)
            {
                UpdateDescription(m_currentSelectStaffStatusData);
            }

        }).AddTo(this);
    }


    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void UpdateDescription(StaffStatusData _data)
    {
        // ステータスをセット
        m_currentSelectStaffStatusData = _data;

        // 説明文を更新
        SetDescription();

        SetActiveGameObjectList();
    }


    // 説明文のテキストをセットする
    virtual protected void SetDescription()
    {
        // 共通スタッフ名をセット
        SetStaffCommonNameText();

        // スタッフ名をセット
        SetStaffNameText();

        // スタッフ画像をセット
        SetStaffImage();

        // ポイント画像
        SetStaffPointImage();

        // 雇用金額をセット
        SetEmploymentPriceText();

        // 給料金額をセット
        SetSalaryPriceText();

        // 配膳割合をセット
        SetStaffProvideValueText();

        // 料理割合をセット
        SetStaffCookingValueText();

        // 接客割合をセット
        SetStaffServiceValueText();
    }


    virtual protected void SetActiveGameObjectList()
    {
        UIExtensions.CheckToSetActiveGameObjectList(m_staffCommonNameText, m_staffCommonNameTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_staffNameText, m_staffNameTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_staffImage, m_staffImageList);
        UIExtensions.CheckToSetActiveGameObjectList(m_employmentPriceText, m_employmentPriceTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_salaryPriceText, m_salaryPriceTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_staffProvideValueText, m_staffProvideValueTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_staffCookingValueText, m_staffCookingValueTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_staffServiceValueText, m_staffServiceValueTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_staffPointImage, m_staffPointImageList);
    }


    // 共通スタッフ名
    private void SetStaffCommonNameText(bool _active = true)
    {
        if (m_staffCommonNameText == null) return;

        m_staffCommonNameText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusData == null) return;

        var data = StaffNameDataBaseManager.instance.GetStaffNameData(m_currentSelectStaffStatusData.StaffNameID);
        if (data == null) return;

        // 共通スタッフ名をセット
        m_staffCommonNameText.text = data.StaffCommonName.GetLocalizedString();
        m_staffCommonNameText.gameObject.SetActive(true);
    }

    private void SetStaffPointImage(bool _active = true)
    {
        if (m_staffPointImage == null) return;

        m_staffPointImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusData == null) return;

        if (StaffManager.instance.IsSettingStaffPoint(m_currentSelectStaffStatusData) == false) return;

        // スタッフポイント画像をセット
        m_staffPointImage.gameObject.SetActive(true);
    }


    // スタッフ名
    private void SetStaffNameText(bool _active = true)
    {
        if (m_staffNameText == null) return;

        m_staffNameText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusData == null) return;

        var data = StaffNameDataBaseManager.instance.GetStaffNameData(m_currentSelectStaffStatusData.StaffNameID);
        if (data == null) return;

        // スタッフ名をセット
        m_staffNameText.text = data.StaffName.GetLocalizedString();
        m_staffNameText.gameObject.SetActive(true);
    }


    private void SetStaffImage(bool _active = true)
    {
        if (m_staffImage == null) return;

        m_staffImage.gameObject.SetActive(false);
        if (m_noStaffText) m_noStaffText.gameObject.SetActive(true);
        if (m_staffBackImage) m_staffBackImage.color = m_rarityColor[0];
        if (m_effectImage) m_effectImage.material = null;

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusData == null) return;
        var data = StaffMemberDataBaseManager.instance.GetStaffMemberData(m_currentSelectStaffStatusData.StaffID);
        if (m_currentSelectStaffStatusData == null) return;

        // スタッフ名をセット
        m_staffImage.sprite = data.StaffSprite;
        m_staffImage.gameObject.SetActive(true);
        if (m_noStaffText) m_noStaffText.gameObject.SetActive(false);
        if (m_staffBackImage) m_staffBackImage.color = m_rarityColor[(int)StaffMemberDataBaseManager.instance.GetStaffRarity(m_currentSelectStaffStatusData.StaffID)];
        if (m_effectImage) m_effectImage.material = m_effectMaterials[(int)StaffMemberDataBaseManager.instance.GetStaffRarity(m_currentSelectStaffStatusData.StaffID)];
    }



    private void SetEmploymentPriceText(bool _active = true)
    {
        if (m_employmentPriceText == null) return;

        m_employmentPriceText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusData == null) return;

        // 雇用金額をセット
        m_employmentPriceText.text = m_currentSelectStaffStatusData.EmploymentPrice().ToString();
        m_employmentPriceText.gameObject.SetActive(true);
    }


    private void SetSalaryPriceText(bool _active = true)
    {
        if (m_salaryPriceText == null) return;

        m_salaryPriceText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusData == null) return;

        // 給料金額をセット
        m_salaryPriceText.text = m_currentSelectStaffStatusData.SalaryPrice().ToString();
        m_salaryPriceText.gameObject.SetActive(true);
    }

    private void SetStaffProvideValueText(bool _active = true)
    {
        if (m_staffProvideValueText == null) return;

        m_staffProvideValueText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusData == null) return;

        // 配膳値をセット
        m_staffProvideValueText.text = m_currentSelectStaffStatusData.ProvideValue.ToString("000");
        m_staffProvideValueText.gameObject.SetActive(true);
    }


    private void SetStaffCookingValueText(bool _active = true)
    {
        if (m_staffCookingValueText == null) return;

        m_staffCookingValueText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusData == null) return;

        // 料理値をセット
        m_staffCookingValueText.text = m_currentSelectStaffStatusData.CookingValue.ToString("000");
        m_staffCookingValueText.gameObject.SetActive(true);
    }

    private void SetStaffServiceValueText(bool _active = true)
    {
        if (m_staffServiceValueText == null) return;

        m_staffServiceValueText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusData == null) return;

        // 接客値をセット
        m_staffServiceValueText.text = m_currentSelectStaffStatusData.ServiceValue.ToString("000");
        m_staffServiceValueText.gameObject.SetActive(true);
    }
}
