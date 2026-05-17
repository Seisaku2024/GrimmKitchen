using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using CriWare;
using Steamworks;

public class CriUnusedCueFinder : EditorWindow
{
    private string acbFileName = "YourAcbFile.acb"; // ACB ファイル名
    private List<string> unusedCues = new List<string>();
    private List<string> usedCuesList = new List<string>();
    private bool searchPerformed = false;
    private Vector2 unusedScrollPosition;
    private Vector2 usedScrollPosition;

    [MenuItem("PROJECT_C/Find Unused CriAtom Cues")]
    public static void ShowWindow()
    {
        GetWindow<CriUnusedCueFinder>("Unused Cue Finder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Unused CriAtom Cue Finder", EditorStyles.boldLabel);

        // ACB ファイル名を入力
        acbFileName = EditorGUILayout.TextField("ACB File Name", acbFileName);




        if (GUILayout.Button("Find Unused Cues") && !string.IsNullOrEmpty(acbFileName))
        {
            FindUnusedCues();
            searchPerformed = true;
        }

        if (searchPerformed)
        {
            int totalCues = unusedCues.Count + usedCuesList.Count;
            GUILayout.Label($"未使用の音源: {unusedCues.Count} / {totalCues}   使用済みの音源: {usedCuesList.Count} / {totalCues}", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();

            // 未使用音源リスト
            GUILayout.BeginVertical(GUILayout.Width(position.width / 2));
            GUILayout.Label("未使用の音源", EditorStyles.boldLabel);
            unusedScrollPosition = GUILayout.BeginScrollView(unusedScrollPosition, GUILayout.Height(300));
            foreach (var cue in unusedCues)
            {
                GUILayout.Label($"- {cue}");
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            // 使用済み音源リスト
            GUILayout.BeginVertical(GUILayout.Width(position.width / 2));
            GUILayout.Label("使用済みの音源", EditorStyles.boldLabel);
            usedScrollPosition = GUILayout.BeginScrollView(usedScrollPosition, GUILayout.Height(300));
            foreach (var cue in usedCuesList)
            {
                GUILayout.Label($"- {cue}");
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
    }

    private void FindUnusedCues()
    {
        unusedCues.Clear();
        usedCuesList.Clear();

        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager がシーンに存在しません！");
            return;
        }

        HashSet<string> usedCues = SoundManager.Instance.GetPlayedCues();

        // CriWareFileInitializer でファイルパスを取得
        string acbPath = CriWare.Common.streamingAssetsPath + "/" + acbFileName;
        string awbPath = CriWare.Common.streamingAssetsPath + "/" + acbFileName;

        CriAtomExAcb acb = CriAtomExAcb.LoadAcbFile(null, acbPath, awbPath);

        if (acb == null)
        {
            Debug.LogError($"ACBファイルのロードに失敗しました: {acbPath}");
            return;
        }

        // ACB から全 Cue を取得
        CriAtomEx.CueInfo[] cueInfos = acb.GetCueInfoList();

        List<string> allCues = new List<string>();
        foreach (var cue in cueInfos)
        {
            allCues.Add(cue.name);
        }

        // 使用済み/未使用の Cue を分類
        foreach (var cue in allCues)
        {
            if (usedCues.Contains(cue))
            {
                usedCuesList.Add(cue);
            }
            else
            {
                unusedCues.Add(cue);
            }
        }

        Debug.Log($"未使用の音源: {unusedCues.Count} / {allCues.Count}\n{string.Join(", ", unusedCues)}");
        Debug.Log($"使用済みの音源: {usedCuesList.Count} / {allCues.Count}\n{string.Join(", ", usedCuesList)}");

        acb.Dispose();
    }
}
