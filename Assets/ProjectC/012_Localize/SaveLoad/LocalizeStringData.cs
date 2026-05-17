using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;

[System.Serializable]
public class LocalizeStringData
{
    // 制作者 田内
    // ローカライズ用

    public LocalizeStringData()
    {
    }

    public LocalizeStringData(string _table, long _entry)
    {
        m_tableName = _table;
        m_entry = _entry;

        m_localizedString = LocalizedString();
    }

    public LocalizeStringData(LocalizedString _localizeString)
    {
        if (_localizeString == null || _localizeString.TableReference == null) return;
        m_tableName = _localizeString.TableReference.TableCollectionName;
        m_entry = _localizeString.TableEntryReference;

        m_localizedString = LocalizedString();
    }

    [Header("初期セットローカライズ")]
    [SerializeField]
    private LocalizedString m_localizedString = null;


    //============================
    // テーブル名
    private string m_tableName = "";

    public string TableName
    {
        get
        {
            if (m_localizedString != null) m_tableName = m_localizedString.TableReference.TableCollectionName;
            return m_tableName;
        }

        set { m_tableName = value; }
    }


    //==============================
    // キーID
    private long m_entry = 0;

    public long Entry
    {
        get
        {
            if (m_localizedString != null) m_entry = m_localizedString.TableEntryReference;
            return m_entry;
        }
        set { m_entry = value; }
    }

    //===========================================================
    //                      実行処理
    //===========================================================

    /// <summary>
    /// ローカライズテキストを取得
    /// </summary>
    public LocalizedString LocalizedString()
    {

        if (m_localizedString != null)
        {
            return m_localizedString;
        }

        // テーブルを取得
        var table = LocalizationSettings.StringDatabase.GetTable(m_tableName);

        // テーブルが存在しない or キーが存在しない場合は null を返す
        if (table == null || table.GetEntry(m_entry) == null)
        {
            m_localizedString = null;
            Debug.LogError(" Table " + m_tableName + " : " + " Entry " + m_entry + " : このLocalizedStringは存在しません");

            return null;
        }

        return new LocalizedString(m_tableName, m_entry);
    }
}
