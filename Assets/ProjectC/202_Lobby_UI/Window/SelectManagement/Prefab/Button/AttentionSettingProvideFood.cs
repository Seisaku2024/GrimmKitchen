using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 制作者　吉田
/// ProvideFoodDataList が空の時に表示する
/// </summary>
public class AttentionSettingProvideFood : WindowUpdateBase
{
    public override void OnInitialize()
    {
        if (ProvideFoodManager.instance.IsSettingProvideFood() == false)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public override void OnUpdate()
    {
        if (ProvideFoodManager.instance.IsSettingProvideFood() == false)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
