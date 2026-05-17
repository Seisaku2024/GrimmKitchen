using UnityEngine;
using UnityEditor;

// BlendAnimationCurve をカスタム表示する PropertyDrawer
[CustomPropertyDrawer(typeof(BlendAnimationCurve))]
public class BlendAnimationCurveDrawer : PropertyDrawer
{
    private AnimationCurve _blendedCurve = new AnimationCurve();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // タイトルのラベル
        SerializedProperty curveWeightPairs = property.FindPropertyRelative("_curveWeightPairs");
        position.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(position, label, EditorStyles.boldLabel);
        position.y += EditorGUIUtility.singleLineHeight + 2;

        // デフォルトの List<> UI をそのまま描画
        EditorGUI.PropertyField(position, curveWeightPairs, new GUIContent("Curve List"), true);
        position.y += EditorGUI.GetPropertyHeight(curveWeightPairs, true) + 5;

        // ブレンド後の AnimationCurve を計算
        BlendAnimationCurve blendCurve = fieldInfo.GetValue(property.serializedObject.targetObject) as BlendAnimationCurve;
        if (blendCurve != null)
        {
            _blendedCurve = blendCurve.GetBlendedCurve();
        }

        // ブレンドカーブの表示
        position.height = EditorGUIUtility.singleLineHeight * 2;
        _blendedCurve = EditorGUI.CurveField(position, "Blended Curve", _blendedCurve);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty curveWeightPairs = property.FindPropertyRelative("_curveWeightPairs");
        float listHeight = EditorGUI.GetPropertyHeight(curveWeightPairs, true);
        return EditorGUIUtility.singleLineHeight * 2 + listHeight + 10;
    }
}
