using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddStatusUIController : FoodWindowUpdateBase
{
    [SerializeField]
    private Image m_image = null;

    [SerializeField]
    private TextMeshProUGUI m_text = null;

    [SerializeField]
    private TextMeshProUGUI m_num = null;

    public override void OnInitialize()
    {

    }

    public override void OnUpdate(FoodData _foodData)
    {
        gameObject.SetActive(false);
        if(_foodData == null) return;

        //FoodTypeがStrengtheningStatus以外は表示しない
        if (!_foodData.IsFoodType(FoodData.FoodType.StrengtheningStatus)) return;

        FoodData.AddStatus addStatusTypeData = 
            _foodData.AddPlayerStatus;
        if(addStatusTypeData == null) return;

        // 表示
        gameObject.SetActive(true);
        AddStatusTypeData addStatusTypeData1 = 
            AddStatusTypeDataBaseManager.instance.GetData(addStatusTypeData.AddType);
        SetAddStatusData(addStatusTypeData1);
        SetAddValue(addStatusTypeData.AddValue);
    }

    public void SetAddStatusData(AddStatusTypeData _statusData)
    {
        if (_statusData == null) return;

        if(m_image != null)
        {
            m_image.sprite = _statusData.IconSprite;
        }
        if(m_text != null)
        {
            m_text.text = _statusData.Name.GetLocalizedString();
        }
    }

    public void SetAddValue(float _value)
    {
        if(m_num != null)
        {
            m_num.text = _value.ToString();
        }
    }
}
