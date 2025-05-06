#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SkyDragonHunter.Editors {

    // 확장된 DropdownItem으로 Type 저장
    public class TypeDropdownItem : AdvancedDropdownItem
    {
        public Type type;

        public TypeDropdownItem(string name, Type type) : base(name)
        {
            this.type = type;
        }
    }

    public class ScriptableObjectTypeDropdown : AdvancedDropdown
    {
        private List<Type> soTypes;
        public Action<Type> onTypeSelected;

        public ScriptableObjectTypeDropdown(AdvancedDropdownState state) : base(state)
        {
            soTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ScriptableObject)))
                .OrderBy(t => t.FullName)
                .ToList();
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("ScriptableObject Types");

            foreach (var type in soTypes)
            {
                root.AddChild(new TypeDropdownItem(type.FullName, type));
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item is TypeDropdownItem typedItem && typedItem.type != null)
            {
                onTypeSelected?.Invoke(typedItem.type);
            }
        }
    }

    public class GameDataCSVImporter : EditorWindow
    {
        private List<string[]> csvData = new();
        private AdvancedDropdownState dropdownState;
        private Type selectedType;
        private string saveFolderPath = "Assets/ScriptableObjects";
        private string fileNameFormat = "{type}_{column}";
        private int selectedColumnIndex = 0;

        [MenuItem("SkyDragonHunter/CSV Importer")]
        public static void ShowWindow()
        {
            GetWindow<GameDataCSVImporter>("CSV Importer");
        }

        public static void DrawSeparator(float thickness = 1f, float padding = 6f)
        {
            GUILayout.Space(padding);
            Rect rect = GUILayoutUtility.GetRect(1, thickness, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, new Color(0.3f, 0.3f, 0.3f, 1f));
            GUILayout.Space(padding);
        }

        private void OnEnable()
        {
            dropdownState ??= new AdvancedDropdownState();
        }

        private void OnGUI()
        {
            DrawSeparator();
            GUILayout.Label("저장 설정", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("저장 경로", GUILayout.Width(80));
            saveFolderPath = EditorGUILayout.TextField(saveFolderPath);
            if (GUILayout.Button("선택", GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFolderPanel("폴더 선택", Application.dataPath, "");
                if (!string.IsNullOrEmpty(path) && path.StartsWith(Application.dataPath))
                {
                    saveFolderPath = "Assets" + path.Substring(Application.dataPath.Length);
                }
                else
                {
                    EditorUtility.DisplayDialog("경고", "Assets 폴더 내부 경로만 선택할 수 있습니다.", "확인");
                }
            }
            EditorGUILayout.EndHorizontal();

            fileNameFormat = EditorGUILayout.TextField("파일 이름 형식", fileNameFormat);

            if (csvData.Count > 0 && csvData[0].Length > 0)
            {
                selectedColumnIndex = EditorGUILayout.Popup("파일 이름에 사용할 컬럼", selectedColumnIndex, csvData[0]);
            }

            DrawSeparator();
            GUILayout.Label("ScriptableObject 타입 선택", EditorStyles.boldLabel);

            if (GUILayout.Button(selectedType != null ? selectedType.FullName : "타입 선택"))
            {
                var dropdown = new ScriptableObjectTypeDropdown(dropdownState);
                dropdown.onTypeSelected = type =>
                {
                    selectedType = type;
                    Repaint();
                };
                dropdown.Show(new Rect(Event.current.mousePosition, Vector2.zero));
            }

            if (selectedType != null)
            {
                GUILayout.Space(10);
                EditorGUILayout.HelpBox($"선택된 타입: {selectedType.FullName}", MessageType.Info);

                GUILayout.Space(10);
                GUILayout.Label("필드 목록:", EditorStyles.boldLabel);

                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var fields = selectedType.GetFields(flags)
                    .Where(f => f.IsPublic || f.GetCustomAttribute<SerializeField>() != null);

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("자료형", GUILayout.Width(150));
                GUILayout.Label("필드명");
                EditorGUILayout.EndHorizontal();
                Rect rect = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(rect, Color.gray);
                foreach (var field in fields)
                {
                    string typeName = field.FieldType == typeof(int) ? "int" :
                                      field.FieldType == typeof(float) ? "float" :
                                      field.FieldType == typeof(double) ? "double" :
                                      field.FieldType == typeof(bool) ? "bool" :
                                      field.FieldType == typeof(string) ? "string" :
                                      field.FieldType.Name;

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(typeName, GUILayout.Width(150));
                    GUILayout.Label(field.Name);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }

            DrawSeparator();
            if (GUILayout.Button("CSV 불러오기"))
            {
                string path = EditorUtility.OpenFilePanel("CSV 파일 선택", "", "csv");
                if (!string.IsNullOrEmpty(path))
                {
                    csvData = File.ReadAllLines(path)
                        .Where(line => !string.IsNullOrWhiteSpace(line))
                        .Select(line => line.Split(','))
                        .ToList();
                }
            }

            if (csvData.Count > 0)
            {
                GUILayout.Space(10); 
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("자료형 미리보기", GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();
                Rect rect = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(rect, Color.gray);
                Rect line = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(line, new Color(0.5f, 0.5f, 0.5f, 0.4f));
                // 데이터 타입 추정 (두 번째 줄 기준)
                if (csvData.Count > 1)
                {
                    var sampleRow = csvData[1];
                    string[] typeNames = sampleRow.Select(cell =>
                    {
                        if (int.TryParse(cell, out _)) return "int";
                        if (double.TryParse(cell, out _)) return "double";
                        if (float.TryParse(cell, out _)) return "float";
                        if (bool.TryParse(cell, out _)) return "bool";
                        return "string";
                    }).ToArray();

                    GUILayout.BeginHorizontal("box");
                    foreach (var type in typeNames)
                    {
                        GUILayout.Label(type, GUILayout.MinWidth(80));
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("필드명 미리보기", GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();
                rect = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(rect, Color.gray);
                line = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(line, new Color(0.5f, 0.5f, 0.5f, 0.4f));
                if (csvData != null && csvData.Count > 0)
                {
                    GUILayout.BeginHorizontal("box");
                    foreach (var cell in csvData[0])
                    {
                        GUILayout.Label(cell, GUILayout.MinWidth(80));
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                GUI.enabled = (csvData.Count > 1 && selectedType != null);
                if (GUILayout.Button("ScriptableObject 생성"))
                {
                    if (selectedType != null && csvData.Count > 1)
                    {
                        var header = csvData[0];
                        var dataRows = csvData.Skip(1);
                        int index = 0;
                        foreach (var row in dataRows)
                        {
                            string columnValue = row.Length > selectedColumnIndex ? row[selectedColumnIndex] : "Unnamed";
                            string finalName = fileNameFormat
                                .Replace("{type}", selectedType.Name)
                                .Replace("{index}", index.ToString("D3"))
                                .Replace("{column}", columnValue)
                                .Replace("{guid}", Guid.NewGuid().ToString("N"));

                            string fullPath = Path.Combine(saveFolderPath, finalName + ".asset");
                            Directory.CreateDirectory(saveFolderPath);

                            var existingAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(fullPath);
                            ScriptableObject instanceToUse;

                            if (existingAsset != null)
                            {
                                instanceToUse = existingAsset;
                            }
                            else
                            {
                                instanceToUse = ScriptableObject.CreateInstance(selectedType);
                                AssetDatabase.CreateAsset(instanceToUse, fullPath);
                            }

                            for (int i = 0; i < header.Length && i < row.Length; i++)
                            {
                                var field = selectedType.GetField(header[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                if (field != null)
                                {
                                    try
                                    {
                                        object value = null;
                                        string cell = row[i];

                                        if (field.FieldType == typeof(int) && int.TryParse(cell, out int iVal)) value = iVal;
                                        else if (field.FieldType == typeof(float) && float.TryParse(cell, out float fVal)) value = fVal;
                                        else if (field.FieldType == typeof(double) && double.TryParse(cell, out double dVal)) value = dVal;
                                        else if (field.FieldType == typeof(bool) && bool.TryParse(cell, out bool bVal)) value = bVal;
                                        else if (field.FieldType == typeof(string)) value = cell;
                                        else continue; // 지원하지 않는 타입은 무시

                                        field.SetValue(instanceToUse, value);
                                        EditorUtility.SetDirty(instanceToUse);
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.LogWarning($"[{field.Name}] 필드에 값 설정 중 예외 발생: {ex.Message}");
                                    }
                                }
                            }

                            index++;
                        }

                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        EditorUtility.DisplayDialog("완료", "ScriptableObject 생성 완료!", "확인");
                        
                    }
                }
            }
            else
            {
                GUILayout.Label("No CSV loaded.", EditorStyles.helpBox);
            }

            GUI.enabled = true;
        }
    }

} // namespace SkyDragonHunter.Editors
#endif