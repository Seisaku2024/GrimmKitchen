using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class ScriptAnalyzerWindow : EditorWindow
{
    private MonoScript selectedScript;
    private string selectedScriptPath;
    private List<string> classNames = new List<string>();
    private Dictionary<string, List<string>> usages = new Dictionary<string, List<string>>();
    private List<string> attachedPrefabs = new List<string>();
    private Vector2 scrollPosition;

    [MenuItem("PROJECT_C/Script Analyzer")]
    private static void ShowWindow()
    {
        GetWindow<ScriptAnalyzerWindow>("Script Analyzer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Script Analyzer", EditorStyles.boldLabel);

        // ドラッグ＆ドロップでスクリプトを選択
        GUILayout.Label("Drag & Drop a Script:");
        MonoScript newScript = (MonoScript)EditorGUILayout.ObjectField(selectedScript, typeof(MonoScript), false);
        if (newScript != selectedScript)
        {
            selectedScript = newScript;
            if (selectedScript != null)
            {
                selectedScriptPath = AssetDatabase.GetAssetPath(selectedScript);
                AnalyzeScript(selectedScriptPath);
            }
        }

        if (string.IsNullOrEmpty(selectedScriptPath))
        {
            GUILayout.Label("No script selected.");
            return;
        }

        GUILayout.Label($"Selected Script: {Path.GetFileName(selectedScriptPath)}", EditorStyles.boldLabel);

        // クラス一覧の表示
        GUILayout.Label("Classes Defined:");
        foreach (var className in classNames)
        {
            GUILayout.Label($"- {className}");
        }

        // 使用箇所の表示
        GUILayout.Label("Usages in Project:");
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (var usage in usages)
        {
            EditorGUILayout.LabelField(usage.Key, EditorStyles.boldLabel);
            foreach (var file in usage.Value)
            {
                // 横並びのレイアウト
                GUILayout.BeginHorizontal();

                // ◎ボタン
                var content = new GUIContent(" ◎ ", "Highlight asset");
                if (GUILayout.Button(content, GUILayout.ExpandWidth(false)))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(file);
                    EditorGUIUtility.PingObject(asset);  // アセットをハイライト
                }


                if (GUILayout.Button(file))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(file, 1);
                }

                GUILayout.EndHorizontal();
            }
        }

        // プレハブの表示
        GUILayout.Label("Attached Prefabs:");
        foreach (var prefab in attachedPrefabs)
        {
            // 横並びのレイアウト
            GUILayout.BeginHorizontal();

            // ◎ボタン
            var content = new GUIContent(" ◎ ", "Highlight asset");
            if (GUILayout.Button(content, GUILayout.ExpandWidth(false)))
            {
                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(prefab);
                EditorGUIUtility.PingObject(asset);  // アセットをハイライト
            }

            if (GUILayout.Button(prefab))
            {
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefab);
            }

            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    private void AnalyzeScript(string scriptPath)
    {
        classNames.Clear();
        usages.Clear();
        attachedPrefabs.Clear();

        string scriptContent = File.ReadAllText(scriptPath);

        // クラス名を抽出
        foreach (Match match in Regex.Matches(scriptContent, @"class\s+(\w+)", RegexOptions.Singleline))
        {
            classNames.Add(match.Groups[1].Value);
        }

        if (classNames.Count == 0)
        {
            Debug.LogWarning("No classes found in the selected script.");
            return;
        }

        // プロジェクト内での使用箇所を検索
        string[] allScripts = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
        foreach (string file in allScripts)
        {
            string content = File.ReadAllText(file);
            foreach (var className in classNames)
            {
                if (Regex.IsMatch(content, $@"\b{className}\b"))
                {
                    if (!usages.ContainsKey(className))
                    {
                        usages[className] = new List<string>();
                    }
                    usages[className].Add(file.Replace(Application.dataPath, "Assets"));
                }
            }
        }

        // プレハブを検索
        string[] prefabs = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
        foreach (string prefab in prefabs)
        {
            // Prefabをロードし、スクリプトコンポーネントをチェック
            GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefab.Replace(Application.dataPath, "Assets"));
            if (prefabAsset != null)
            {
                var components = prefabAsset.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var component in components)
                {
                    if (component != null && classNames.Contains(component.GetType().Name))
                    {
                        attachedPrefabs.Add(prefab.Replace(Application.dataPath, "Assets"));
                        break; // 一度見つけたら他のコンポーネントを確認しなくてもよい
                    }
                }
            }
        }
    }
}

#endif