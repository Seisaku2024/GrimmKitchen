using CI.QuickSave;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlusMoneyGraphPresenter : MonoBehaviour
{
    [SerializeField]
    private BarGraph m_graph = null;

    private void Start()
    {
        Load();

        ShowGraph();

        Save();
    }


    public void ShowGraph()
    {
        if (m_graph == null) return;
        m_graph.AddValue(ManagementGameDataManager.instance.EarnedMoney);
        m_graph.CreateGraph();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        if (m_graph == null) return;

        var writer = QuickSaveWriter.Create("ManagementResult");
        writer.Write<List<float>>("PlusMoney", m_graph.ValueList);
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
        if (!reader.Exists("PlusMoney"))
        {
            SaveInitialValue();
        }
        else
        {
            m_graph.ValueList = reader.Read<List<float>>("PlusMoney");
        }
    }

    void SaveInitialValue()
    {
        if (m_graph == null) return;

        var writer = QuickSaveWriter.Create("ManagementResult");
        writer.Write<List<float>>("PlusMoney", new List<float> { 0, 0, 0, 0, 0 });
        writer.Commit();
    }


}
