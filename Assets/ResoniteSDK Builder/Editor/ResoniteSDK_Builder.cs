using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class ResoniteSDK_Builder
{
    [MenuItem("Resonite SDK/Internal/Build UnityPackage")]
    public static void BuildUnityPackage()
    {
        var version = "0.0.0";
        JsonUtility.FromJsonOverwrite(File.ReadAllText("Assets/ResoniteSDK/package.json"), version);
        
        var files = Directory.EnumerateFiles("Assets/ResoniteSDK/", "*", SearchOption.AllDirectories)
            .Where(file => Path.GetExtension(file) != ".meta").ToArray();

        Debug.Log($"Building UnityPackage from files:\n{string.Join("\n", files)}");

        Directory.CreateDirectory("Builds");

        AssetDatabase.ExportPackage(files, $"Builds/ResoniteSDK-V{version}.unitypackage", ExportPackageOptions.IncludeDependencies);

        Debug.Log("ResoniteSDK UnityPackage built!");
    }
    
    [MenuItem("Resonite SDK/Internal/Build UPM package")]
    public static void BuildUpmPackage()
    {
        EditorUtility.DisplayProgressBar("ResoniteSDK","Building UPM package", 0);

        var pk = Client.Pack("Assets/ResoniteSDK/", "Builds");
        
        while (!pk.IsCompleted) { }
        
        Debug.Log("ResoniteSDK UPM built!");  
        
        if (pk.Status == StatusCode.Failure)
            Debug.LogError(pk.Error);

        EditorUtility.ClearProgressBar();
    }
}
