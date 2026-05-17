using CI.QuickSave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveKeyConfig : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset m_inputActions = null;

    // Start is called before the first frame update
    void Awake()
    {
        Load();
    }

    private void OnDestroy()
    {
        Save();
    }

    private void Load()
    {
        if(m_inputActions == null) return;

        if (!QuickSaveReader.RootExists("KeyConfig"))
        {
            Debug.LogError("セーブデータがありません:KeyConfig");
            SaveInitialValue();
        }
        QuickSaveReader reader = QuickSaveReader.Create("KeyConfig");
        if (!reader.Exists("Binding"))
        {
            SaveInitialValue();
        }

        string bindData = reader.Read<string>("Binding");
        m_inputActions.LoadBindingOverridesFromJson(bindData);

        //Debug.Log("LoadKeyConfig");
    }

    void SaveInitialValue()
    {
        if (m_inputActions == null) return;

        var writer = QuickSaveWriter.Create("KeyConfig");
        // InputActionAssetの上書き情報の保存
        var json = m_inputActions.SaveBindingOverridesAsJson();
        writer.Write("Binding", json);
        writer.Commit();

        //Debug.Log("SaveInitKeyConfig");
    }

    private void Save()
    {
        if (m_inputActions == null) return;

        // InputActionAssetの上書き情報の保存
        var writer = QuickSaveWriter.Create("KeyConfig");
        var json = m_inputActions.SaveBindingOverridesAsJson();
        writer.Write("Binding", json);
        writer.Commit();

        //Debug.Log("SaveKeyConfig");
    }
}
