using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUIController : FoodWindowUpdateBase
{
    [SerializeField]
    private Image m_image = null;

    [SerializeField]
    private CreateConditionImage m_createConditionImage = null;

    [SerializeField]
    private TextMeshProUGUI m_name = null;

    [SerializeField]
    private TextMeshProUGUI m_level = null;

    public override void OnInitialize()
    {

    }

    public override void OnUpdate(FoodData _foodData)
    {
        gameObject.SetActive(false);
        if (_foodData == null) return;

        //FoodTypeがHeal以外は表示しない
        if (!_foodData.IsFoodType(FoodData.FoodType.DebuffCondition)) return;

        var conditionData = 
            ConditionDataBaseManager.instance.GetConditionData(_foodData.ConditionID);
        if (conditionData == null) return;

        // 表示
        gameObject.SetActive(true);
        uint level = _foodData.ItemLevel;
        SetLevel((float)level);
        SetImage(conditionData.ConditionSprite);
        SetImages(_foodData);
        SetName(conditionData.ConditionName.GetLocalizedString());
    }

    public void SetImage(Sprite _sprite)
    {
        if (m_image != null)
        {
            m_image.sprite = _sprite;
        }
    }

    public void SetImages(BaseItemData _baseItem)
    {
        if (m_createConditionImage != null)
        {
            m_createConditionImage.CreateImage(_baseItem);
        }
    }

    public void SetName(string _name)
    {
        if (m_name != null)
        {
            m_name.text = _name;
        }
    }

    public void SetLevel(float _value)
    {
        if (m_level != null)
        {
            m_level.text = _value.ToString();
        }
    }
}
