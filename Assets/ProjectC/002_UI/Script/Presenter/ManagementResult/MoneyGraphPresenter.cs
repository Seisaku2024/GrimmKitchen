using CI.QuickSave;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyGraphPresenter : MonoBehaviour
{
    [SerializeField]
    private LineGraph m_graph = null;

    private void Start()
    {
        Load();

        ShowGraph();

        Save();
    }


    public void ShowGraph()
    {
        if (m_graph == null) return;
        m_graph.AddValue(ManagementDataManager.instance.TotalEarnedMoney);
        m_graph.CreateGraph();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        if (m_graph == null) return;

        var writer = QuickSaveWriter.Create("ManagementResult");
        writer.Write<List<float>>("TotalMoney", m_graph.ValueList);
        writer.Commit();
    }

    public void Load()
    {
        if (m_graph == null) return;
        if (!QuickSaveReader.RootExists("ManagementResult"))
        {
            Debug.LogError("セーブデータがありません:ManagementResult");
            SaveInitialValue();
        }
        QuickSaveReader reader = QuickSaveReader.Create("ManagementResult");
        if (!reader.Exists("TotalMoney"))
        {
            SaveInitialValue();
        }

        List<float> totalMoney = reader.Read<List<float>>("TotalMoney");
        if (totalMoney.Count < 5)
        {
            SaveInitialValue();
        }
        m_graph.ValueList = totalMoney;

    }

    void SaveInitialValue()
    {
        if (m_graph == null) return;

        var writer = QuickSaveWriter.Create("ManagementResult");
        writer.Write<List<float>>("TotalMoney", new List<float> { 0, 0, 0, 0, 0 });
        writer.Commit();
    }
}
