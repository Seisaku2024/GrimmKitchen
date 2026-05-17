using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CustomSceneInspector : CustomInspectorBase
{
    private string inputComponent; // ユーザーが入力中のコンポーネント名
    private List<GameObject> filteredObjects = new List<GameObject>(); // フィルタリングされたオブジェクト
    private Vector2 scrollPositionFilteredObjects; // フィルタリング結果のスクロール位置
    private HashSet<string> sceneComponentNames = new HashSet<string>(); // シーン内のコンポーネント名キャッシュ
    private List<string> suggestedComponents = new List<string>(); // 部分一致の候補リスト

    [MenuItem("PROJECT_C/CustomInspector/CustomSceneInspector")]
    public static void ShowWindow()
    {
        GetWindow<CustomSceneInspector>("Custom Scene Inspector");
    }

    private void OnEnable()
    {
        RefreshSceneComponentNames();
    }

    /// <summary>
    /// 左側のGUI（オブジェクト選択）
    /// </summary>
    protected override void LeftSideGUI_SelectObject()
    {
        GUILayout.Label("Component Search", EditorStyles.boldLabel);

        // コンポーネント名の入力
        string newInput = EditorGUILayout.TextField("Search Component:", inputComponent);

        if (newInput != inputComponent)
        {
            inputComponent = newInput;
            UpdateSuggestions();
        }

        // サジェスト候補をボタンで表示
        if (suggestedComponents.Count > 0)
        {
            foreach (var suggestion in suggestedComponents)
            {
                if (GUILayout.Button(suggestion, GUILayout.ExpandWidth(true)))
                {
                    inputComponent = suggestion;
                    suggestedComponents.Clear(); // 候補リストをクリア
                    FilterObjectsByComponent();
                }
            }
        }

        // フィルタリングされたオブジェクトのリストを表示
        GUILayout.Label("Filtered Objects:", EditorStyles.boldLabel);
        scrollPositionFilteredObjects = EditorGUILayout.BeginScrollView(scrollPositionFilteredObjects, GUILayout.ExpandHeight(true));

        foreach (var obj in filteredObjects)
        {
            // 選択されているオブジェクトを強調表示
            GUI.backgroundColor = selectedObject == obj ? Color.yellow : Color.white;
            if (GUILayout.Button(obj.name, GUILayout.ExpandWidth(true)))
            {
                SelectObject(obj);
            }
        }

        GUI.backgroundColor = Color.white; // 色をリセット
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// シーン内のコンポーネント名をキャッシュ
    /// </summary>
    private void RefreshSceneComponentNames()
    {
        sceneComponentNames.Clear();

        // シーン内のすべてのオブジェクトを取得
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        // オブジェクトにアタッチされているコンポーネント名を取得
        foreach (var obj in allObjects)
        {
            var components = obj.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component != null)
                {
                    sceneComponentNames.Add(component.GetType().Name);
                }
            }
        }
    }

    /// <summary>
    /// コンポーネント検索の候補を更新
    /// </summary>
    private void UpdateSuggestions()
    {
        if (string.IsNullOrEmpty(inputComponent))
        {
            suggestedComponents.Clear();
        }
        else
        {
            // 入力値と部分一致するコンポーネント名を取得
            suggestedComponents = sceneComponentNames
                .Where(name => name.IndexOf(inputComponent, System.StringComparison.OrdinalIgnoreCase) >= 0)
                .Take(10) // 最大10件に制限
                .ToList();
        }
    }

    /// <summary>
    /// 指定されたコンポーネントを持つオブジェクトをフィルタリング
    /// </summary>
    private void FilterObjectsByComponent()
    {
        if (string.IsNullOrEmpty(inputComponent))
        {
            filteredObjects.Clear();
            return;
        }

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        filteredObjects = allObjects
            .Where(obj => obj.GetComponent(inputComponent) != null)
            .ToList();
    }
}
