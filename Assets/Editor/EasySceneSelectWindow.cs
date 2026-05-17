/*!
 * @file EasySceneSelectWindow.cs
 * @brief ProjectC用シーン変更簡易化ウィンドウ
 * @author 上甲
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using MackySoft.Navigathena.SceneManagement;


/// <summary>
/// @brief シーン変更簡易化ウィンドウ
/// </summary>
public class EasySceneSelectWindow : EditorWindow
{
    [MenuItem("PROJECT_C/SceneSelector")]
    public static void ShowMyEditor()
    {
        EditorWindow.GetWindow(typeof(EasySceneSelectWindow));
    }

    private readonly string forderPath = "Assets/ProjectC";
    private Vector2 scrollPos; // スクロール用の変数
    private string[] scenePaths; // シーンのパスを格納する配列
    private List<string> favoriteScenes = new List<string>(); // お気に入りシーンリスト
    private readonly float buttonWidth = 200f; // ボタンの幅を制限
    private const string FavoriteScenesKey = "FavoriteScenes"; // お気に入りシーンのEditorPrefs用キー
    private bool isTransitioning = false;
    private void OpenScene(string scenePath)
    {
        if (Application.isPlaying)
        {
            // ランタイム時の処理
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            ExecuteRuntimeSceneChange(sceneName);
        }
        else
        {
            // エディタ時の処理（保存していない変更がある場合は確認）
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }

    private async void ExecuteRuntimeSceneChange(string sceneName)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        if (GlobalSceneNavigator.Instance != null)
        {
            try
            {
                ISceneIdentifier identifier = new BuiltInSceneIdentifier(sceneName);
                // シーン名を保存
                SceneNameManager.instance.ChangeSceneName(sceneName);

                await GlobalSceneNavigator.Instance.Change(
                     identifier,
                     new NextPageTransitionDirector(new BuiltInSceneIdentifier("Loading"))
                 );
                isTransitioning = false;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"シーン遷移中にエラーが発生しました: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("GlobalSceneNavigator.Instance が存在しません。");
        }
    }




    private void OnEnable()
    {
        LoadFavorites();
        OnSceneEnable();
    }
    private void OnGUI()
    {
        SceneOnGUI();
    }

    private void OnSceneEnable()
    {
        // 指定フォルダ内のシーンを検索
        string[] guids = AssetDatabase.FindAssets("t:Scene", new[] { forderPath });
        scenePaths = new string[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            scenePaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
        }

    }

    private void SceneOnGUI()
    {
        // お気に入りシーンのリストを表示
        if (favoriteScenes.Count > 0)
        {
            EditorGUILayout.LabelField("Favorite Scenes:");
            GUI.enabled = !isTransitioning;
            foreach (var favScene in favoriteScenes)
            {
                string sceneName = Path.GetFileNameWithoutExtension(favScene);
                if (GUILayout.Button(sceneName, GUILayout.Width(buttonWidth)))
                {
                    OpenScene(favScene);
                }
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
        }

        // スクロール開始
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // シーンリスト表示
        foreach (var scenePath in scenePaths)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            EditorGUILayout.BeginHorizontal();

            // お気に入りチェックボックス
            bool isFavorite = favoriteScenes.Contains(scenePath);
            bool newIsFavorite = EditorGUILayout.Toggle(isFavorite, GUILayout.Width(20));

            if (newIsFavorite != isFavorite)
            {
                if (newIsFavorite)
                    favoriteScenes.Add(scenePath); // お気に入りに追加
                else
                    favoriteScenes.Remove(scenePath); // お気に入りから削除

                SaveFavorites();　// 変更があればお気に入りをセーブ
            }
            GUI.enabled = !isTransitioning;
            // シーン名のボタン表示
            if (GUILayout.Button(sceneName, GUILayout.Width(buttonWidth)))
            {
                OpenScene(scenePath);
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndScrollView();
    }

    private void SaveFavorites()
    {
        string favoritesString = string.Join(";", favoriteScenes.ToArray());
        EditorPrefs.SetString(FavoriteScenesKey, favoritesString);
    }

    private void LoadFavorites()
    {
        favoriteScenes.Clear();
        // 保存された文字列を取得、分割してリストに格納
        if (EditorPrefs.HasKey(FavoriteScenesKey))
        {
            string favoriteString = EditorPrefs.GetString(FavoriteScenesKey);
            var savedFavorites = favoriteString.Split(';');

            foreach (var scenePath in savedFavorites)
            {
                if (File.Exists(scenePath))
                {
                    favoriteScenes.Add(scenePath);
                }
            }
            SaveFavorites(); // 存在しないシーンがあればリストを更新
        }
    }
}
