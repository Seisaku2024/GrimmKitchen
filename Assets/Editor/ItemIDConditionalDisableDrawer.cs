using UnityEngine;
using UnityEditor;
using ItemIDEditor;

[CustomPropertyDrawer(typeof(ItemIDConditionalDisableInInspectorAttribute))]
internal sealed class ItemIDConditionalDisableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

         ItemIDConditionalDisableInInspectorAttribute attr = base.attribute as ItemIDConditionalDisableInInspectorAttribute;

        //attr.a;
        
        SerializedProperty itemTypeID = property.serializedObject.FindProperty(attr.PropertyName);

        if (itemTypeID == null)
        {
            //Debug.LogError(attr.PropertyName + "プロパティは存在しません");

            
            var aaa = property.serializedObject.targetObject.GetType().GetNestedTypes();
            var pro = aaa[1].GetProperty(attr.PropertyName);
            object obj = new();
            pro.GetValue(obj);
            uint i = (uint)obj;


            //EditorGUI.PropertyField(position, property, label, true);
            //EditorGUI.EndDisabledGroup();
            return;
        }

        bool isDisable = IsDisable(attr, itemTypeID);

        EditorGUI.BeginDisabledGroup(isDisable);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();

    }

    

    // 無効にするかどうか
    private bool IsDisable(ItemIDConditionalDisableInInspectorAttribute attr, SerializedProperty prop)
    {
        bool flg = true;

        if (prop.uintValue == (uint)attr.ItemTypeID)
        {
            flg = false;
        }

        return flg;

    }
}