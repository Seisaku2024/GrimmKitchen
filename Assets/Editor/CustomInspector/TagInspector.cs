using UnityEditor;
using UnityEngine;

public class TagInspector : CustomInspectorBase
{
    private string selectedTag; // 選択されたタグ
    private GameObject[] taggedObjects; // タグ付きオブジェクトのリスト

    [MenuItem("PROJECT_C/CustomInspector/TagInspector")]
    public static void ShowWindow()
    {
        GetWindow<TagInspector>("TagInspector");
    }
    protected override void LeftSideGUI_SelectObject()
    {
        GUILayout.Label("Select Tag", EditorStyles.boldLabel);
        selectedTag = EditorGUILayout.TagField("Tag", selectedTag); // タグフィールドを追加

        if (GUILayout.Button("Find Objects", GUILayout.ExpandWidth(true)))
        {
            FindTaggedObjects();
        }

        if (taggedObjects != null)
        {
            GUILayout.Label("Found Objects:", EditorStyles.boldLabel);

            foreach (var obj in taggedObjects)
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

    private void FindTaggedObjects()
    {
        // すべてのオブジェクトを取得し、選択したタグでフィルタリング
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        taggedObjects = System.Array.FindAll(allObjects, obj => obj.CompareTag(selectedTag) || (selectedTag == "Untagged" && !obj.CompareTag("Untagged")));

        ResetSelect();
    }

}