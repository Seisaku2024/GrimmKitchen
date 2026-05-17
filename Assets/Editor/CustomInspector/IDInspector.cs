using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;
using Arbor.Calculators;

public class IDInspector : CustomInspectorBase
{
    private int selectedInstanceID; // 選択されたインスタンスID

    [MenuItem("PROJECT_C/CustomInspector/IDInspector")]
    public static void ShowWindow()
    {
        GetWindow<IDInspector>("IDInspector");
    }
    protected override void LeftSideGUI_SelectObject()
    {
        GUILayout.Label("Select ID", EditorStyles.boldLabel);
        selectedInstanceID = EditorGUILayout.IntField("InstanceID", selectedInstanceID); // インスタンスIDフィールドを追加

        if (GUILayout.Button("Find Object", GUILayout.ExpandWidth(true)))
        {
            SelectObject(FindObjectFromInstanceID(selectedInstanceID));
        }
    }
    public static UnityEngine.GameObject FindObjectFromInstanceID(int instanceId)
    {
        try
        {
            var type = typeof(UnityEngine.GameObject);
            var flags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod;
            var ret = type.InvokeMember("FindObjectFromInstanceID", flags, null, null, new object[] { instanceId });
            return (UnityEngine.GameObject)ret;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return null;
    }
}