#if UNITY_EDITOR
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace UFOWASH.Editor
{
    public static class UfoWashRealAssetImporter
    {
        const string DownloadUrl = "https://kenney.nl/media/pages/assets/space-kit/20874c75ac-1677698978/kenney_space-kit.zip";
        const string ImportFolder = "Assets/UFO_WASH/Art/Imported/KenneySpaceKit";

        [MenuItem("UFO WASH/1 - IMPORT REAL CC0 3D ASSETS", false, 0)]
        public static void Import()
        {
            string temp = Path.Combine(Path.GetTempPath(), "UFOWash_KenneySpaceKit");
            string zip = temp + ".zip";
            try
            {
                EditorUtility.DisplayProgressBar("UFO WASH", "Downloading Kenney Space Kit (CC0)...", .15f);
                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "UFO-WASH-Unity-Asset-Importer");
                    client.DownloadFile(DownloadUrl, zip);
                }

                if (File.Exists(temp)) Directory.Delete(temp, true);
                Directory.CreateDirectory(temp);
                EditorUtility.DisplayProgressBar("UFO WASH", "Extracting real FBX models and textures...", .55f);
                ZipFile.ExtractToDirectory(zip, temp);

                if (!Directory.Exists(ImportFolder)) Directory.CreateDirectory(ImportFolder);
                string[] files = Directory.GetFiles(temp, "*", SearchOption.AllDirectories);
                int copied = 0;
                foreach (string source in files)
                {
                    string relative = source.Substring(temp.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    string destination = Path.Combine(ImportFolder, relative);
                    Directory.CreateDirectory(Path.GetDirectoryName(destination));
                    File.Copy(source, destination, true);
                    copied++;
                }

                EditorUtility.DisplayProgressBar("UFO WASH", "Importing models into Unity...", .9f);
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("UFO WASH - REAL ASSETS IMPORTED",
                    "Imported " + copied + " files from Kenney Space Kit.\n\nIncludes actual spacecraft, alien characters, station props and textures.\nLicense: CC0 (commercial use allowed).\n\nNext: run BUILD POWER SLICE to place the imported models.",
                    "OK");
            }
            catch (Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError("UFO WASH asset import failed: " + e);
                EditorUtility.DisplayDialog("Asset import failed", e.Message + "\n\nNo project files were deleted.", "OK");
            }
            finally
            {
                if (File.Exists(zip)) File.Delete(zip);
                if (Directory.Exists(temp)) Directory.Delete(temp, true);
            }
        }
    }
}
#endif
