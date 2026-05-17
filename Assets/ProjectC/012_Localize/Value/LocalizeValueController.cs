using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

[System.Serializable]
public class LocalizeValueController
{
    // 制作者 田内
    // インスペクターでValueをセットする

    [System.Serializable]
    private class ValueLocalizedString
    {
        [SerializeField]
        public string ValueName = "variable";

        [Header("優先度:1")]
        [SerializeField]
        public LocalizedString LocalizedString = null;

        [Header("優先度:2")]
        [SerializeField]
        public string String = "Not Reference";
    }

    [Header("============LocalizedString============")]
    [SerializeField]
    private LocalizedString m_localizedString = null;


    [Header("============Valueに挿入するLocalizedString(nullでも可)============")]
    [SerializeField]
    private List<ValueLocalizedString> m_valueLocalizedStringList = null;

    //=======================================
    //              実行処理
    //=======================================

    public string GetLocalizedString()
    {
        try
        {

            if (m_localizedString == null || m_localizedString.IsEmpty)
            {
                Debug.LogError("LocalizedStringは未設定または空です");
                return "None Parent";
            }


            var stringData = new LocalizedString(tableReference: m_localizedString.TableReference, entryReference: m_localizedString.TableEntryReference);

            foreach (var data in m_valueLocalizedStringList)
            {
                if (data.LocalizedString == null || data.LocalizedString.IsEmpty)
                {
                    stringData[data.ValueName] = new StringVariable { Value = data.String };
                }
                else
                {
                    stringData[data.ValueName] = new StringVariable { Value = data.LocalizedString.GetLocalizedString() };
                }
            }
    
            return stringData.GetLocalizedString();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
            return "Try Error";
        }
    }

}
