using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>Adiciona todas as cenas de Assets/Scenes ao Build Settings (Menu primeiro, depois Map).</summary>
public static class AdicionarCenasAoBuild
{
    [MenuItem("Tools/Adicionar cenas do jogo ao Build Settings")]
    static void Adicionar()
    {
        string[] ordem = { "Menu", "Map", "Battle", "Event", "Loja" };
        var guids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        var caminhos = guids.Select(AssetDatabase.GUIDToAssetPath).ToList();

        caminhos = caminhos
            .OrderBy(p => { int i = System.Array.IndexOf(ordem, System.IO.Path.GetFileNameWithoutExtension(p)); return i < 0 ? 99 : i; })
            .ToList();

        EditorBuildSettings.scenes = caminhos.Select(p => new EditorBuildSettingsScene(p, true)).ToArray();
        Debug.Log("Build Settings atualizado: " + string.Join(", ", caminhos));
    }
}
