using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomInspectorBase : EditorWindow
{
    protected GameObject selectedObject; // 現在選択されているオブジェクト

    private MonoBehaviour[] components; // オブジェクト内のコンポーネントリスト
    private MonoBehaviour selectedComponent; // 現在選択されているコンポーネント
    private SerializedObject serializedComponent; // シリアライズされたコンポーネント

    private Vector2 scrollPositionObjects; // オブジェクトリストのスクロール位置
    private Vector2 scrollPositionComponents; // コンポーネントリストのスクロール位置
    private Vector2 scrollPositionInspector; // インスペクターのスクロール位置

    /// <summary>
    /// オブジェクトを探す処理のみカスタム可能に
    /// </summary>
    virtual protected void LeftSideGUI_SelectObject()
    {
        return;
    }


    private void OnGUI()
    {
        // 左右に分割
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        // 左側のエリア（オブジェクトとコンポーネントの選択）
        GUILayout.BeginVertical(GUILayout.Width(position.width * 0.4f), GUILayout.ExpandHeight(true)); // 左側40%の幅
                                                                                                       // オブジェクトリストのスクロールビューを開始
        scrollPositionObjects = EditorGUILayout.BeginScrollView(scrollPositionObjects, GUILayout.ExpandHeight(true));

        LeftSideGUI_SelectObject();
        EditorGUILayout.EndScrollView(); // オブジェクトリストのスクロールビューを終了

        GUILayout.EndVertical(); // 左側のエリア終了


        // 境界線の描画===============================================================
        DrawSeparator();
        //==========================================================================


        // 右側のエリア（インスペクター表示）
        GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f); // 薄いグレーに設定
        GUILayout.BeginVertical(GUILayout.Width(position.width * 0.6f), GUILayout.ExpandHeight(true)); // 右側60%の幅

        // コンポーネントリストのスクロールビューを開始
        scrollPositionComponents = EditorGUILayout.BeginScrollView(scrollPositionComponents, GUILayout.ExpandHeight(true));

        RightUpSideGUI_SelectComponent();

        EditorGUILayout.EndScrollView(); // コンポーネントリストのスクロールビューを終了


        // 境界線を描画（コンポーネント選択とインスペクターの間）===========================
        DrawSeparator();
        //==========================================================================


        // オブジェクトリストのスクロールビューを開始
        scrollPositionObjects = EditorGUILayout.BeginScrollView(scrollPositionObjects, GUILayout.ExpandHeight(true));

        RightDownSideGUI_Inspector();

        EditorGUILayout.EndScrollView(); // オブジェクトリストのスクロールビューを終了


        GUILayout.EndVertical(); // 右側のエリア終了

        GUI.backgroundColor = Color.white; // 色をリセット
        GUILayout.EndHorizontal(); // 全体の左右レイアウト終了
    }


    private void RightDownSideGUI_Inspector()
    {
        // インスペクターのスクロールビューを開始
        scrollPositionInspector = EditorGUILayout.BeginScrollView(scrollPositionInspector, GUILayout.ExpandHeight(true));

        if (selectedComponent != null && serializedComponent != null)
        {
            GUILayout.Label("Selected Component Inspector:", EditorStyles.boldLabel);


            serializedComponent.Update();
            SerializedProperty prop = serializedComponent.GetIterator();

            while (prop.NextVisible(true))
            {
                EditorGUILayout.PropertyField(prop, true);
            }

            serializedComponent.ApplyModifiedProperties();
        }
        else
        {
            GUILayout.Label("No component selected.", EditorStyles.boldLabel);
        }

        EditorGUILayout.EndScrollView(); // インスペクターのスクロールビューを終了
    }

    private void RightUpSideGUI_SelectComponent()
    {
        if (selectedObject != null)
        {
            // ヒエラルキーで選択するボタンを追加
            if (GUILayout.Button("Select in Hierarchy"))
            {
                // 選択されているコンポーネントのゲームオブジェクトをヒエラルキーで選択
                Selection.activeGameObject = selectedComponent.gameObject;
            }

            if (components != null)
            {
                GUILayout.Label("Components on Selected Object:", EditorStyles.boldLabel);

                foreach (var comp in components)
                {
                    // 選択されているコンポーネントを強調表示
                    GUI.backgroundColor = selectedComponent == comp ? Color.cyan : Color.white;
                    if (GUILayout.Button(comp.GetType().Name, GUILayout.ExpandWidth(true)))
                    {
                        SelectComponent(comp);
                    }
                    GUI.backgroundColor = Color.white; // 色をリセット
                }
            }
        }
    }
    protected void SelectObject(GameObject obj)
    {
        selectedObject = obj;
        components = selectedObject.GetComponents<MonoBehaviour>(); // MonoBehaviourコンポーネントのリストを取得
        selectedComponent = null;
        serializedComponent = null;
    }

    protected void SelectComponent(MonoBehaviour component)
    {
        selectedComponent = component;
        serializedComponent = new SerializedObject(component); // 選択したコンポーネントのSerializedObjectを作成
    }

    protected void DrawSeparator()
    {
        GUILayout.Box("", GUILayout.Width(1), GUILayout.Height(1)); // 縦の境界線を描画
    }

    protected void ResetSelect()
    {
        selectedObject = null;
        components = null;
        selectedComponent = null;
        serializedComponent = null;
    }

}
