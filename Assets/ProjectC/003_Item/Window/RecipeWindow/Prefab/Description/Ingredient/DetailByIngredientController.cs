using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailByIngredientController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_levelValueText = null;

    [SerializeField]
    private CreateConditionImage m_createConditionImage = null;

    [SerializeField]
    private TextMeshProUGUI m_conditionText = null;

    public void SetDetailByIngredient(IngredientData _data)
    {
        if (_data == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        SetLevelValue((int)_data.ItemLevel);
        SetConditionList(_data);
        SetConditionText(_data);
    }

    private void SetLevelValue(int _level)
    {
        if (m_levelValueText == null) return;

        m_levelValueText.text = _level.ToString();
    }

    private void SetConditionList(BaseItemData _itemData)
    {
        if (m_createConditionImage == null) return;

        // 画像を作成
        if (m_createConditionImage.CreateImage(_itemData))
        {
            // 表示
            m_createConditionImage.gameObject.SetActive(true);
        }
    }

    public void SetConditionText(BaseItemData _itemData)
    {
        if (m_conditionText == null) return;

        var data = ConditionDataBaseManager.instance.GetConditionData(_itemData.ConditionID);
        if (data == null) return;

        m_conditionText.text = data.ConditionName.GetLocalizedString();
    }
}
