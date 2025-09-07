using UnityEngine;
using UnityEditor;

public class MaterialSwapper
{
    [MenuItem("Tools/Replace BaseColor Materials In Scene Only")]
    static void ReplaceMaterialAndAddScript()
    {
        string targetShaderName = "Shader Graphs/Dissolve_Direction_Metallic"; // 교체할 쉐이더 경로
        string materialPath = "Assets/Resources/Materials/Shader Graphs_Dissolve_Dissolve_Metallic.mat";
        AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        Material newMat = AssetDatabase.LoadAssetAtPath<Material>("Assets / Resources / Materials / Shader Graphs_Dissolve_Dissolve_Metallic.mat");
        if (newMat == null)
        {
            Debug.LogError("교체할 머테리얼을 찾을 수 없습니다: " + materialPath);
            return;
        }

        Renderer[] allRenderers = GameObject.FindObjectsOfType<Renderer>();
        int replacedCount = 0;

        foreach (Renderer renderer in allRenderers)
        {
            Material[] mats = renderer.sharedMaterials;

            if (mats.Length > 0 && mats[0] != null && mats[0].name == "base color")
            {
                // 머테리얼 교체 (첫 번째만)
                mats[0] = newMat;
                renderer.sharedMaterials = mats;
                replacedCount++;

                // 스크립트 추가
                if (renderer.gameObject.GetComponent<VerticalWipe>() == null)
                {
                    Undo.AddComponent<VerticalWipe>(renderer.gameObject);
                }
            }
        }

        Debug.Log($"총 {replacedCount}개의 오브젝트에 머테리얼을 교체하고 VerticalWipe 스크립트를 추가했습니다.");
    }

}
