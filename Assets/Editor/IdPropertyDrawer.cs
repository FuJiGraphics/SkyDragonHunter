using UnityEngine;
using UnityEditor;
using SkyDragonHunter.Utility;

[CustomPropertyDrawer(typeof(ID))]
public class IDPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty idProp = property.FindPropertyRelative("m_Id");
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        idProp.intValue = EditorGUI.IntField(position, idProp.intValue);
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
