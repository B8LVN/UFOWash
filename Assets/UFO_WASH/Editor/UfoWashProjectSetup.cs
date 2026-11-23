#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class UfoWashProjectSetup
{
    private const string Root = "Assets/UFO_WASH";
    private static readonly string[] Folders = {
        "Art/Models", "Art/Materials", "Art/Textures", "Art/Environment",
        "Audio/Music", "Audio/SFX", "Animations",
        "Prefabs/Player", "Prefabs/UFOs", "Prefabs/Equipment", "Prefabs/Environment", "Prefabs/UI",
        "Scenes", "Scripts/Core", "Scripts/Player", "Scripts/Interaction", "Scripts/Vehicles",
        "Scripts/Cleaning", "Scripts/Equipment", "Scripts/Economy", "Scripts/Progression", "Scripts/UI", "Scripts/Save",
        "ScriptableObjects", "UI", "Settings", "QA", "Documentation"
    };

    [MenuItem("UFO WASH/Initialize Project Structure and Audit", false, 1)]
    public static void InitializeAndAudit()
    {
        foreach (var folder in Folders)
            Directory.CreateDirectory(Path.Combine(Root, folder));

        WriteGitIgnore();
        WriteAuditReport();
        AssetDatabase.Refresh();
        Debug.Log("[UFO WASH] Setup complete. Folders created (existing content preserved), .gitignore checked, and audit report written to Assets/UFO_WASH/Documentation/PROJECT_AUDIT.txt");
        EditorUtility.DisplayDialog("UFO WASH Setup Complete", "Project folders are ready. Existing content was not deleted.\n\nAudit report:\nAssets/UFO_WASH/Documentation/PROJECT_AUDIT.txt", "OK");
    }

    [MenuItem("UFO WASH/Generate Project Audit Only", false, 2)]
    public static void AuditOnly()
    {
        WriteAuditReport();
        AssetDatabase.Refresh();
        Debug.Log("[UFO WASH] Audit report generated at Assets/UFO_WASH/Documentation/PROJECT_AUDIT.txt");
    }


    private static void WriteGitIgnore()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), ".gitignore");
        const string block = "\n# UFO WASH / Unity generated files\n[Ll]ibrary/\n[Tt]emp/\n[Oo]bj/\n[Bb]uild/\n[Bb]uilds/\n[Ll]ogs/\n[Uu]ser[Ss]ettings/\n.vs/\n.idea/\n*.csproj\n*.sln\n*.slnx\n*.unityproj\n*.pidb\n*.booproj\n*.svd\n*.pdb\n*.mdb\n*.opendb\n*.VC.db\n*.VC.VC.opendb\n*.DS_Store\nThumbs.db\n";
        string existing = File.Exists(path) ? File.ReadAllText(path) : "";
        if (!existing.Contains("# UFO WASH / Unity generated files"))
            File.AppendAllText(path, block, Encoding.UTF8);
    }

    private static void WriteAuditReport()
    {
        var sb = new StringBuilder();
        sb.AppendLine("UFO WASH — AUTOMATED PROJECT AUDIT");
        sb.AppendLine("Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sb.AppendLine("Unity version: " + Application.unityVersion);
        sb.AppendLine("Project path: " + Directory.GetCurrentDirectory());
        sb.AppendLine("Platform: " + EditorUserBuildSettings.activeBuildTarget);
        sb.AppendLine("Scripting backend (current target): " + PlayerSettings.GetScriptingBackend(BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget)));
        sb.AppendLine("Color space: " + PlayerSettings.colorSpace);
        sb.AppendLine("Active Input Handling: inspect ProjectSettings.asset (API varies by Unity version) ");
        sb.AppendLine("Product name: " + PlayerSettings.productName);
        sb.AppendLine("Company name: " + PlayerSettings.companyName);
        sb.AppendLine("Graphics pipeline asset: " + (GraphicsSettings.currentRenderPipeline ? GraphicsSettings.currentRenderPipeline.name : "None (Built-in pipeline)"));
        sb.AppendLine("Quality levels:");
        foreach (var q in QualitySettings.names) sb.AppendLine(" - " + q);
        sb.AppendLine("Packages (selected):");
        foreach (var p in UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages())
            if (p.name.Contains("render-pipelines") || p.name.Contains("inputsystem") || p.name.Contains("test-framework") || p.name.Contains("android"))
                sb.AppendLine(" - " + p.name + " " + p.version);
        sb.AppendLine();
        sb.AppendLine("NOTE: This audit is a snapshot of editor settings, not a full build or runtime test.");
        var reportPath = Path.Combine(Root, "Documentation", "PROJECT_AUDIT.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(reportPath));
        File.WriteAllText(reportPath, sb.ToString(), Encoding.UTF8);
    }
}
#endif
