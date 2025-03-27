//using UnityEngine;
//using UnityEditor;
//using SkyDragonHunter.Utility;

//[CustomPropertyDrawer(typeof(AlphaUnit))]
//public class AlphaUnitPropertyDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        // inspector 표시할 변수 탐색, 해당 변수는 SerializeField 혹은 public 상태여야 함
//        SerializedProperty idProp = property.FindPropertyRelative("m_OriginNumber");

//        // 프로퍼티 드로잉을 시작
//        EditorGUI.BeginProperty(position, label, property);

//        // 레이블을 그린 후, 남은 공간에 double 필드 표시
//        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
//        idProp.doubleValue = EditorGUI.DoubleField(position, idProp.doubleValue);

//        // 프로퍼티 드로잉 종료
//        EditorGUI.EndProperty();
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        // 한 줄 높이만 사용
//        return EditorGUIUtility.singleLineHeight;
//    }
//}
