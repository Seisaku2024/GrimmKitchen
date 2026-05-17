using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionProvideFoodNum : WindowUpdateBase
{
    public override void OnInitialize()
    {
        // 料理セットされていなければ表示しない
        if (ProvideFoodManager.instance.IsSettingProvideFood()==false)
        {
            gameObject.SetActive(false);
            return;
        }

        if (ProvideFoodManager.instance.IsProvideAll() == false)
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
        // 料理セットされていなければ表示しない
        if (ProvideFoodManager.instance.IsSettingProvideFood() == false)
        {
            gameObject.SetActive(false);
            return;
        }

        if (ProvideFoodManager.instance.IsProvideAll() == false)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
