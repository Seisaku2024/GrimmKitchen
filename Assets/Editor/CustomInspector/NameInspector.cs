using UnityEditor;
using UnityEngine;

public class NameInspector : CustomInspectorBase
{
    private string searchName; // 検索するオブジェクトの名称
    private GameObject[] namedObjects; // 名前付きオブジェクトのリスト

    [MenuItem("PROJECT_C/CustomInspector/NameInspector")]
    public static void ShowWindow()
    {
        GetWindow<NameInspector>("NameInspector");
    }

    protected override void LeftSideGUI_SelectObject()
    {

        GUILayout.Label("Search by Object Name", EditorStyles.boldLabel);
        searchName = EditorGUILayout.TextField("Object Name", searchName); // オブジェクト名フィールドを追加

        if (GUILayout.Button("Find Objects", GUILayout.ExpandWidth(true)))
        {
            FindNamedObjects();
        }

        if (namedObjects != null)
        {
            GUILayout.Label("Found Objects:", EditorStyles.boldLabel);

            foreach (var obj in namedObjects)
            {
                // 選択されているオブジェクトを強調表示
                GUI.backgroundColor = selectedObject == obj ? Color.yellow : Color.white;
                if (GUILayout.Button(obj.name, GUILayout.ExpandWidth(true)))
                {
                    SelectObject(obj);
                }
                GUI.backgroundColor = Color.white; // 色をリセット
            }
        }
    }

    private void FindNamedObjects()
    {
        // すべてのオブジェクトを取得し、指定された名称でフィルタリング（大文字小文字を区別しない）
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        namedObjects = System.Array.FindAll(allObjects, obj => obj.name.ToLower().Contains(searchName.ToLower()));

        ResetSelect();
    }
}
