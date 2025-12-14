using UnityEngine;
using UnityEditor;

public class FixAllMaterials : EditorWindow
{
    [MenuItem("Tools/Fix All Materials")]
    public static void FixMaterials()
    {
        string[] guids = AssetDatabase.FindAssets("t:Material");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat == null) continue;

            // Shader setzen
            Shader urpShader = Shader.Find("Universal Render Pipeline/Lit");
            if (urpShader != null) mat.shader = urpShader;

            // Emission ausschalten
            if (mat.IsKeywordEnabled("_EMISSION"))
            {
                mat.DisableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.black);
            }

            // Normal Map fix
            if (mat.HasProperty("_BumpMap"))
            {
                Texture bump = mat.GetTexture("_BumpMap");
                if (bump != null) mat.EnableKeyword("_NORMALMAP");
            }

            EditorUtility.SetDirty(mat);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Alle Materialien automatisch repariert!");
    }
}
