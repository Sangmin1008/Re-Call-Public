using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureColorReplace : EditorWindow
{
    private string inputPath = "Assets/External/SlimUI/Modern Menu 1/Graphics/Frames/Panel 1920x1080px.png"; // 기본 경로
    private string outputPath = "Assets/External/SlimUI/Modern Menu 1/Graphics/Frames/1111.png";

    [MenuItem("Tools/Color Adjust/Convert to White Tone")]
    public static void ShowWindow()
    {
        GetWindow<TextureColorReplace>("Color Adjuster");
    }

    private void OnGUI()
    {
        GUILayout.Label("Convert Texture to White Tone", EditorStyles.boldLabel);

        inputPath = EditorGUILayout.TextField("Input Path", inputPath);
        outputPath = EditorGUILayout.TextField("Output Path", outputPath);

        if (GUILayout.Button("Convert"))
        {
            ConvertTexture(inputPath, outputPath);
        }
    }

    private void ConvertTexture(string path, string savePath)
    {
        Texture2D originalTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

        if (originalTexture == null)
        {
            Debug.LogError("Failed to load texture at path: " + path);
            return;
        }

        string assetPath = AssetDatabase.GetAssetPath(originalTexture);
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

        if (importer != null && !importer.isReadable)
        {
            importer.isReadable = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height);
        Color[] pixels = originalTexture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            float luminance = pixels[i].grayscale;
            pixels[i] = new Color(luminance, luminance, luminance, pixels[i].a); // 흰색톤으로 보정
        }

        newTexture.SetPixels(pixels);
        newTexture.Apply();

        // 저장
        byte[] pngData = newTexture.EncodeToPNG();
        File.WriteAllBytes(savePath, pngData);

        Debug.Log("Image converted and saved to: " + savePath);

        AssetDatabase.Refresh();
    }
}
