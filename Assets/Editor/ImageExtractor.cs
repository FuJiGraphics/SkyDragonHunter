using UnityEngine;
using UnityEditor;
using System.IO;

public class IconExtractor : EditorWindow
{
    Texture2D sourceTexture;
    string outputFolder = "Assets/Icons";

    int cropOffsetX = 0;
    int cropOffsetY = 0;
    int cropSize = 0;   // 0이면 전체 텍스처의 작은 쪽

    [MenuItem("Tools/Icon Extractor")]
    static void OpenWindow()
    {
        GetWindow<IconExtractor>("Icon Extractor");
    }

    void OnGUI()
    {
        GUILayout.Label("Sprite → PNG Cropper", EditorStyles.boldLabel);

        sourceTexture = (Texture2D)EditorGUILayout.ObjectField("Source Texture", sourceTexture, typeof(Texture2D), false);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);

        if (sourceTexture != null)
        {
            // Preview
            float ratio = (float)sourceTexture.height / sourceTexture.width;
            float previewW = position.width;
            float previewH = previewW * ratio;
            Rect previewRect = GUILayoutUtility.GetRect(previewW, previewH, GUILayout.ExpandWidth(true));
            EditorGUI.DrawTextureTransparent(previewRect, sourceTexture);

            // Click → offset
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0 && previewRect.Contains(e.mousePosition))
            {
                Vector2 local = e.mousePosition - previewRect.position;
                float u = Mathf.Clamp01(local.x / previewRect.width);
                float v = 1f - Mathf.Clamp01(local.y / previewRect.height);

                cropOffsetX = Mathf.RoundToInt(u * sourceTexture.width);
                cropOffsetY = Mathf.RoundToInt(v * sourceTexture.height);

                Repaint();
                e.Use();
            }

            // Overlay
            Handles.BeginGUI();
            {
                float pxToGuiX = previewRect.width / sourceTexture.width;
                float pxToGuiY = previewRect.height / sourceTexture.height;

                float guiX = previewRect.xMin + cropOffsetX * pxToGuiX;
                float guiY = previewRect.yMin + (1f - (cropOffsetY / (float)sourceTexture.height)) * previewRect.height;

                // Crosshair
                Handles.color = Color.red;
                float cs = 5f;
                Handles.DrawLine(new Vector3(guiX - cs, guiY), new Vector3(guiX + cs, guiY));
                Handles.DrawLine(new Vector3(guiX, guiY - cs), new Vector3(guiX, guiY + cs));

                // Crop outline
                if (cropSize > 0)
                {
                    float w = cropSize * pxToGuiX;
                    float h = cropSize * pxToGuiY;
                    Rect outline = new Rect(guiX, guiY - h, w, h);
                    Handles.DrawSolidRectangleWithOutline(outline, new Color(0, 0, 0, 0), Color.green);
                }
            }
            Handles.EndGUI();

            EditorGUILayout.LabelField($"Offset X: {cropOffsetX}", $"Y: {cropOffsetY}");
        }

        cropOffsetX = EditorGUILayout.IntField("Manual Offset X (px)", cropOffsetX);
        cropOffsetY = EditorGUILayout.IntField("Manual Offset Y (px)", cropOffsetY);
        cropSize = EditorGUILayout.IntField("Crop Size (px)", cropSize);

        if (GUILayout.Button("Extract Icon") && sourceTexture != null)
            ExtractOnce();
    }

    void ExtractOnce()
    {
        if (!AssetDatabase.IsValidFolder(outputFolder))
            Directory.CreateDirectory(outputFolder);

        // 메모리상의 텍스처 복제
        string path = AssetDatabase.GetAssetPath(sourceTexture);
        byte[] raw = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.RGBA32, false);
        tex.LoadImage(raw);
        tex.Apply();

        // 크롭 크기 계산
        int size = (cropSize > 0)
            ? Mathf.Clamp(cropSize, 1, Mathf.Min(tex.width, tex.height))
            : Mathf.Min(tex.width, tex.height);

        // 크롭 시작점 (왼쪽-아래 기준)
        int startX = Mathf.Clamp(cropOffsetX, 0, tex.width - size);
        int startY = Mathf.Clamp(cropOffsetY, 0, tex.height - size);

        // 픽셀 복사
        Color[] pixels = tex.GetPixels(startX, startY, size, size);
        Texture2D outTex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        outTex.SetPixels(pixels);
        outTex.Apply();

        // 파일명 결정: icon0.png, icon1.png ...
        int index = 0;
        string filePath;
        while (true)
        {
            filePath = Path.Combine(outputFolder, $"icon{index}.png");
            if (!File.Exists(filePath))
                break;
            index++;
        }

        // PNG 저장
        byte[] png = outTex.EncodeToPNG();
        File.WriteAllBytes(filePath, png);

        AssetDatabase.Refresh();
        Debug.Log($"[IconExtractor] Saved icon at ({startX},{startY}) size {size} to '{filePath}'");

        DestroyImmediate(tex);
        DestroyImmediate(outTex);
    }
}
