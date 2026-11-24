#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class UfoWashProjectSetup
{
    private const string Root = "Assets/UFO_WASH";
    private static readonly string[] Folders = {
        "Art/Models", "Art/Materials", "Art/Textures", "Art/Environment",
        "Audio/Music", "Audio/SFX", "Animations",
        "Prefabs/Player", "Prefabs/UFOs", "Prefabs/Equipment", "Prefabs/Environment", "Prefabs/UI",
        "Scenes", "Scripts/Core", "Scripts/Player", "Scripts/Interaction", "Scripts/Vehicles",
        "Scripts/Cleaning", "Scripts/Equipment", "Scripts/Economy", "Scripts/Progression", "Scripts/UI", "Scripts/Save",
        "ScriptableObjects", "UI", "Settings"
    };

    [MenuItem("UFO WASH/Initialize Project Structure", false, 1)]
    public static void InitializeProjectStructure()
    {
        foreach (var folder in Folders)
            Directory.CreateDirectory(Path.Combine(Root, folder));

        WriteGitIgnore();
        AssetDatabase.Refresh();
        Debug.Log("[UFO WASH] Project folders are ready. Existing content was preserved and .gitignore was checked.");
        EditorUtility.DisplayDialog("UFO WASH Setup Complete", "Project folders are ready. Existing content was not deleted.", "OK");
    }


    private static void WriteGitIgnore()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), ".gitignore");
        const string block = "\n# UFO WASH / Unity generated files\n[Ll]ibrary/\n[Tt]emp/\n[Oo]bj/\n[Bb]uild/\n[Bb]uilds/\n[Ll]ogs/\n[Uu]ser[Ss]ettings/\n.vs/\n.idea/\n*.csproj\n*.sln\n*.slnx\n*.unityproj\n*.pidb\n*.booproj\n*.svd\n*.pdb\n*.mdb\n*.opendb\n*.VC.db\n*.VC.VC.opendb\n*.DS_Store\nThumbs.db\n";
        string existing = File.Exists(path) ? File.ReadAllText(path) : "";
        if (!existing.Contains("# UFO WASH / Unity generated files"))
            File.AppendAllText(path, block, Encoding.UTF8);
    }
}
#endif
