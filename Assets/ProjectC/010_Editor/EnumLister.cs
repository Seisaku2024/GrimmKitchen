
#if UNITY_EDITOR

using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;  // 非同期タスク用の名前空間

public class EnumLister : EditorWindow
{
    private class FolderNode
    {
        public string Name;
        public Dictionary<string, FolderNode> SubFolders = new Dictionary<string, FolderNode>();
        public List<EnumInfo> Enums = new List<EnumInfo>();
    }

    private class EnumInfo
    {
        public string EnumName;
        public List<string> Elements;

        public EnumInfo(string enumName, List<string> elements)
        {
            EnumName = enumName;
            Elements = elements;
        }
    }

    private FolderNode rootFolder = new FolderNode { Name = "Assets" };
    private FolderNode filteredRootFolder;
    private string searchQuery = string.Empty;
    private Vector2 scrollPosition;
    private HashSet<object> expandedNodes = new HashSet<object>();
    private bool isProcessing = false;  // 非同期処理中かどうかのフラグ
    private string processingStatus = "Ready";  // 処理中のステータス表示

    [MenuItem("PROJECT_C/Enum Lister")]
    private static void ShowWindow()
    {
        GetWindow<EnumLister>("Enum Lister");
    }

    private void OnEnable()
    {
        RefreshEnumListAsync();  // 非同期でリストを更新
    }

    private void OnGUI()
    {
        GUILayout.Label("Project Enums (Hierarchical View)", EditorStyles.boldLabel);

        // Search bar
        GUILayout.BeginHorizontal();
        GUILayout.Label("Search:", GUILayout.Width(50));
        string newSearchQuery = GUILayout.TextField(searchQuery).ToLower();
        if (newSearchQuery != searchQuery)
        {
            searchQuery = newSearchQuery;
            ApplySearchFilter();
        }
        GUILayout.EndHorizontal();

        if (isProcessing)
        {
            GUILayout.Label($"Processing... {processingStatus}");
        }
        else
        {
            if (filteredRootFolder == null || (filteredRootFolder.SubFolders.Count == 0 && filteredRootFolder.Enums.Count == 0))
            {
                GUILayout.Label("No enums found.");
                if (GUILayout.Button("Refresh"))
                    RefreshEnumListAsync();  // 非同期で再読み込み
                return;
            }

            if (GUILayout.Button("Refresh"))
                RefreshEnumListAsync();  // 非同期で再読み込み

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawFolderNode(filteredRootFolder, 0);

            EditorGUILayout.EndScrollView();
        }
    }

    private void DrawFolderNode(FolderNode folder, int indentLevel)
    {
        // Draw folder header
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(indentLevel * 20);
        if (GUILayout.Button(folder.Name, EditorStyles.foldoutHeader))
        {
            if (expandedNodes.Contains(folder))
                expandedNodes.Remove(folder);
            else
                expandedNodes.Add(folder);
        }
        EditorGUILayout.EndHorizontal();

        // If folder is expanded, draw subfolders and enums
        if (expandedNodes.Contains(folder))
        {
            foreach (var subFolder in folder.SubFolders.Values)
            {
                DrawFolderNode(subFolder, indentLevel + 1);
            }

            foreach (var enumInfo in folder.Enums)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space((indentLevel + 1) * 20);
                if (GUILayout.Button(enumInfo.EnumName, EditorStyles.foldoutHeader))
                {
                    if (expandedNodes.Contains(enumInfo))
                        expandedNodes.Remove(enumInfo);
                    else
                        expandedNodes.Add(enumInfo);
                }
                EditorGUILayout.EndHorizontal();

                if (expandedNodes.Contains(enumInfo))
                {
                    foreach (var element in enumInfo.Elements)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space((indentLevel + 2) * 20);
                        GUILayout.Label($"- {element}");
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
    }

    private async void RefreshEnumListAsync()
    {
        isProcessing = true;
        processingStatus = "Loading enums...";

        // 非同期タスクを開始
        await Task.Run(() => RefreshEnumList());

        // メインスレッドに戻ってUIを更新
        isProcessing = false;
        processingStatus = "Done!";
        Repaint();
    }

    private void RefreshEnumList()
    {
        rootFolder = new FolderNode { Name = "Assets" };
        expandedNodes.Clear();

        string[] scripts = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
        foreach (string scriptPath in scripts)
        {
            string scriptText = File.ReadAllText(scriptPath);
            string relativePath = "Assets" + scriptPath.Replace(Application.dataPath, "").Replace("\\", "/");
            string folderPath = Path.GetDirectoryName(relativePath).Replace("\\", "/");

            // Parse enums in the script
            foreach (Match match in Regex.Matches(scriptText, @"enum\s+(\w+)\s*{([^}]*)}", RegexOptions.Singleline))
            {
                string enumName = match.Groups[1].Value;
                string enumBody = match.Groups[2].Value;

                // Parse enum elements
                List<string> elements = enumBody.Split(new[] { ',', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(e => e.Trim())
                    .Where(e => !string.IsNullOrEmpty(e))
                    .ToList();

                // Add enum to folder structure
                FolderNode currentFolder = rootFolder;
                foreach (string folder in folderPath.Split('/'))
                {
                    if (!currentFolder.SubFolders.ContainsKey(folder))
                        currentFolder.SubFolders[folder] = new FolderNode { Name = folder };
                    currentFolder = currentFolder.SubFolders[folder];
                }
                currentFolder.Enums.Add(new EnumInfo(enumName, elements));
            }
        }

        ApplySearchFilter();
    }

    private void ApplySearchFilter()
    {
        if (string.IsNullOrEmpty(searchQuery))
        {
            filteredRootFolder = rootFolder;
        }
        else
        {
            filteredRootFolder = FilterFolder(rootFolder, searchQuery);
        }
    }

    private FolderNode FilterFolder(FolderNode folder, string query)
    {
        FolderNode result = new FolderNode { Name = folder.Name };

        foreach (var subFolder in folder.SubFolders.Values)
        {
            var filteredSubFolder = FilterFolder(subFolder, query);
            if (filteredSubFolder.SubFolders.Count > 0 || filteredSubFolder.Enums.Count > 0)
            {
                result.SubFolders[filteredSubFolder.Name] = filteredSubFolder;
            }
        }

        foreach (var enumInfo in folder.Enums)
        {
            if (enumInfo.EnumName.ToLower().Contains(query) ||
                enumInfo.Elements.Any(e => e.ToLower().Contains(query)))
            {
                result.Enums.Add(enumInfo);
            }
        }

        return result;
    }
}

#endif