#if UNITY_EDITOR
using System;
using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEngine;

namespace UFOWASH.Editor
{
    public static class UfoWashRealAssetImporter
    {
        const string DownloadPage = "https://kenney.nl/assets/space-kit";
        const string ImportFolder = "Assets/UFO_WASH/Art/Imported/KenneySpaceKit";

        [MenuItem("UFO WASH/1 - IMPORT REAL CC0 3D ASSETS", false, 0)]
        public static void Import()
        {
            EditorUtility.DisplayDialog(
                "Download Kenney Space Kit",
                "The previous automatic download returned an invalid ZIP on some connections.\n\n" +
                "Your browser will open Kenney's official download page. Click Download, then Continue without donating. " +
                "After the ZIP finishes downloading, select that ZIP in the file picker.",
                "Open download page");

            Application.OpenURL(DownloadPage);
            string zip = EditorUtility.OpenFilePanel("Select kenney_spaceKit.zip", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads", "zip");
            if (string.IsNullOrEmpty(zip)) return;

            if (!IsValidZip(zip, out string validationError))
            {
                EditorUtility.DisplayDialog("Invalid asset archive",
                    "The selected file is not a valid ZIP archive.\n\n" + validationError +
                    "\n\nDownload the ZIP from the official Kenney page and select the .zip file, not the webpage.",
                    "OK");
                return;
            }

            string temp = Path.Combine(Path.GetTempPath(), "UFOWash_KenneySpaceKit_Extract");
            try
            {
                if (Directory.Exists(temp)) Directory.Delete(temp, true);
                Directory.CreateDirectory(temp);
                EditorUtility.DisplayProgressBar("UFO WASH", "Extracting verified Space Kit archive...", .35f);

                using (ZipArchive archive = ZipFile.OpenRead(zip))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string destination = Path.GetFullPath(Path.Combine(temp, entry.FullName));
                        string root = Path.GetFullPath(temp) + Path.DirectorySeparatorChar;
                        if (!destination.StartsWith(root, StringComparison.OrdinalIgnoreCase))
                            throw new InvalidDataException("Unsafe path inside ZIP: " + entry.FullName);

                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            Directory.CreateDirectory(destination);
                            continue;
                        }

                        Directory.CreateDirectory(Path.GetDirectoryName(destination));
                        entry.ExtractToFile(destination, true);
                    }
                }

                string[] files = Directory.GetFiles(temp, "*", SearchOption.AllDirectories);
                int modelCount = 0;
                foreach (string source in files)
                {
                    string ext = Path.GetExtension(source).ToLowerInvariant();
                    if (ext == ".fbx" || ext == ".obj" || ext == ".gltf" || ext == ".glb" || ext == ".dae") modelCount++;
                }

                if (modelCount == 0)
                    throw new InvalidDataException("The archive contains no supported 3D model files (FBX/OBJ/glTF/GLB/DAE).");

                if (!Directory.Exists(ImportFolder)) Directory.CreateDirectory(ImportFolder);
                foreach (string source in files)
                {
                    string relative = source.Substring(temp.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    string destination = Path.Combine(ImportFolder, relative);
                    Directory.CreateDirectory(Path.GetDirectoryName(destination));
                    File.Copy(source, destination, true);
                }

                EditorUtility.DisplayProgressBar("UFO WASH", "Refreshing Unity asset database...", .9f);
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("REAL 3D ASSETS IMPORTED",
                    "Imported " + files.Length + " files, including " + modelCount + " 3D model files.\n\n" +
                    "Source: Kenney Space Kit (CC0).\n\nNext run: UFO WASH / BUILD POWER SLICE - FULL ART PASS",
                    "OK");
            }
            catch (Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError("UFO WASH asset import failed: " + e);
                EditorUtility.DisplayDialog("Asset import failed", e.Message + "\n\nThe selected ZIP and existing project files were not deleted.", "OK");
            }
            finally
            {
                if (Directory.Exists(temp)) Directory.Delete(temp, true);
            }
        }

        static bool IsValidZip(string path, out string error)
        {
            error = null;
            try
            {
                using (var stream = File.OpenRead(path))
                {
                    if (stream.Length < 4) throw new InvalidDataException("File is too small to be a ZIP archive.");
                    byte[] signature = new byte[4];
                    stream.Read(signature, 0, 4);
                    bool valid = signature[0] == 0x50 && signature[1] == 0x4B &&
                        ((signature[2] == 0x03 && signature[3] == 0x04) ||
                         (signature[2] == 0x05 && signature[3] == 0x06) ||
                         (signature[2] == 0x07 && signature[3] == 0x08));
                    if (!valid)
                    {
                        stream.Position = 0;
                        byte[] preview = new byte[Math.Min(160, (int)stream.Length)];
                        stream.Read(preview, 0, preview.Length);
                        string text = System.Text.Encoding.UTF8.GetString(preview);
                        throw new InvalidDataException("File signature is not ZIP (PK). It may be an HTML error page or an incomplete download. File begins: " + text.Replace("\r", " ").Replace("\n", " "));
                    }
                }
                using (ZipFile.OpenRead(path)) { }
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }
    }
}
#endif
