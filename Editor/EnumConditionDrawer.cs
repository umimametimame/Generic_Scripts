using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FlagConditionalDisableInInspectorAttribute))]
internal sealed class FlagConditionalDisableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = base.attribute as FlagConditionalDisableInInspectorAttribute;
        var prop = property.serializedObject.FindProperty(attr.FlagVariableName);
        if (prop == null)
        {
            Debug.LogError($"Not found '{attr.FlagVariableName}' property");
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndDisabledGroup();
        }
        var isDisable = IsDisable(attr, prop);
        if (attr.ConditionalInvisible && isDisable)
        {
            return;
        }
        EditorGUI.BeginDisabledGroup(isDisable);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = base.attribute as FlagConditionalDisableInInspectorAttribute;
        var prop = property.serializedObject.FindProperty(attr.FlagVariableName);
        if (attr.ConditionalInvisible && IsDisable(attr, prop))
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
        return EditorGUI.GetPropertyHeight(property, true);
    }

    private bool IsDisable(FlagConditionalDisableInInspectorAttribute attr, SerializedProperty prop)
    {
        return attr.TrueThenDisable ? prop.boolValue : !prop.boolValue;
    }
}