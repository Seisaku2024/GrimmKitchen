using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(SceneTransitionManager))]
public class SceneTransitionManagerEditor : Editor
{
    private List<string> m_sceneNames = new List<string>();
    private int m_selectedSceneIndex;

    private void OnEnable()
    {
        InitializeSceneSelection();
    }

    private void InitializeSceneSelection()
    {
        SceneTransitionManager script = (SceneTransitionManager)target;

        // ビルド設定に登録されているシーンを取得
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        int sceneIndex = -1;

        for (int i = 0; i < sceneCount; ++i)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (script.m_sceneName == name)
            {
                sceneIndex = i;
            }
            m_sceneNames.Add(name);
        }

        m_selectedSceneIndex = sceneIndex;
    }

    public override void OnInspectorGUI()
    {
        SceneTransitionManager script = (SceneTransitionManager)target;

        EditorGUI.BeginChangeCheck();
        // シーン選択ドロップダウン
        m_selectedSceneIndex = EditorGUILayout.Popup("Select Scene", m_selectedSceneIndex, m_sceneNames.ToArray());
        if (!EditorGUI.EndChangeCheck())
        {
            return;
        }
        if (m_selectedSceneIndex >= 0 && m_selectedSceneIndex < m_sceneNames.Count)
        {
            script.m_sceneName = m_sceneNames[m_selectedSceneIndex];
            EditorUtility.SetDirty(target);
        }
    }
}