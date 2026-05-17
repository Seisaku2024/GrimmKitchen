using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomerPasserbyRootManager))]
public class CustomerPasserbyRootManagerEditor : Editor
{
    private CustomerPasserbyRootManager m_target;

    private void Awake()
    {
        m_target = target as CustomerPasserbyRootManager;
    }

    [SerializeField] bool Child_Auto_Setting = false;

    [SerializeField] GameObject rightSideRootObject = null;
    [SerializeField] GameObject leftSideRootObject = null;

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        Child_Auto_Setting = EditorGUILayout.ToggleLeft("子オブジェクト群を自動で登録する", Child_Auto_Setting);
        if (Child_Auto_Setting)
        {
            EditorGUILayout.LabelField("ルートオブジェクトでの自動設定 既存の座標はクリアされます");
            rightSideRootObject = EditorGUILayout.ObjectField("右側座標群", rightSideRootObject, typeof(GameObject), true) as GameObject;
            leftSideRootObject = EditorGUILayout.ObjectField("左側座標群", leftSideRootObject, typeof(GameObject), true) as GameObject;

            if (GUILayout.Button("設定更新"))
            {
                m_target.m_rightSidePos.Clear();
                m_target.m_leftSidePos.Clear();
                if (rightSideRootObject != null)
                {
                    foreach (Transform child in rightSideRootObject.transform)
                    {
                        m_target.m_rightSidePos.Add(child);
                    }
                }
                if (leftSideRootObject != null)
                {
                    foreach (Transform child in leftSideRootObject.transform)
                    {
                        m_target.m_leftSidePos.Add(child);
                    }
                }
            }
        }

        base.OnInspectorGUI();

        // GUIの更新があったら実行
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(m_target);
        }
    }

}
