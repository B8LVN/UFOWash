using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UFOWASH.EditorTools
{
    public static class UfoWashFoundationTools
    {
        private const string Root = "Assets/UFO_WASH";
        private const string ScenePath = "Assets/UFO_WASH/Scenes/UFO_WASH_Prototype.unity";

        [MenuItem("UFO WASH/Foundation/Create Project Folders", false, 10)]
        public static void CreateFolders()
        {
            string[] folders = {
                "Art", "Art/Models", "Art/Materials", "Art/Textures", "Art/Environment",
                "Audio", "Animations", "Prefabs", "Prefabs/Player", "Prefabs/UFOs",
                "Prefabs/Equipment", "Prefabs/Environment", "Scenes", "Scripts", "Scripts/Core",
                "Scripts/Player", "Scripts/Interaction", "Scripts/Vehicles", "Scripts/Cleaning",
                "Scripts/Equipment", "Scripts/Economy", "Scripts/Progression", "Scripts/UI", "Scripts/Save",
                "ScriptableObjects", "UI", "Documentation", "Tests"
            };
            foreach (var folder in folders)
                EnsureFolder(Root + "/" + folder);
            AssetDatabase.Refresh();
            Debug.Log("UFO WASH: Project folder foundation created/verified. Existing content was not deleted.");
        }

        [MenuItem("UFO WASH/Foundation/Create Prototype Blockout Scene", false, 20)]
        public static void CreatePrototypeScene()
        {
            if (File.Exists(ScenePath))
            {
                EditorUtility.DisplayDialog("UFO WASH", "Prototype scene already exists. No files were overwritten:\n" + ScenePath, "OK");
                return;
            }

            CreateFolders();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Environment_Ground_Blockout";
            ground.transform.localScale = new Vector3(3f, 1f, 2f);
            ground.transform.position = Vector3.zero;
            SetColor(ground, new Color(0.16f, 0.19f, 0.23f));

            CreateCube("WashBay_BackWall", new Vector3(0f, 2f, 10f), new Vector3(30f, 4f, 0.5f), new Color(0.12f, 0.16f, 0.22f));
            CreateCube("WashBay_LeftWall", new Vector3(-15f, 2f, 0f), new Vector3(0.5f, 4f, 20f), new Color(0.12f, 0.16f, 0.22f));
            CreateCube("WashBay_RightWall", new Vector3(15f, 2f, 0f), new Vector3(0.5f, 4f, 20f), new Color(0.12f, 0.16f, 0.22f));
            CreateCube("WashBay_AccentStrip", new Vector3(0f, 3.8f, 9.72f), new Vector3(12f, 0.12f, 0.08f), new Color(0.1f, 0.8f, 1f));
            CreateCube("UFO_ParkingPad", new Vector3(0f, 0.03f, 1.5f), new Vector3(7f, 0.06f, 7f), new Color(0.22f, 0.25f, 0.3f));
            CreateCube("Player_Start_Marker", new Vector3(0f, 0.15f, -6f), new Vector3(0.5f, 0.3f, 0.5f), new Color(0.25f, 0.9f, 0.5f));

            var lightGo = new GameObject("Lighting_Directional");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            light.color = new Color(0.82f, 0.9f, 1f);
            lightGo.transform.rotation = Quaternion.Euler(45f, -35f, 0f);

            var cameraGo = new GameObject("Blockout_Camera");
            var camera = cameraGo.AddComponent<Camera>();
            cameraGo.tag = "MainCamera";
            cameraGo.transform.position = new Vector3(0f, 8f, -16f);
            cameraGo.transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, 2f) - cameraGo.transform.position);
            camera.fieldOfView = 60f;
            camera.clearFlags = CameraClearFlags.Skybox;

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("UFO WASH", "Prototype blockout scene created:\n" + ScenePath + "\n\nThis is a simple graybox, not final art.", "OK");
            Debug.Log("UFO WASH: Prototype blockout scene created at " + ScenePath);
        }

        private static void CreateCube(string name, Vector3 position, Vector3 scale, Color color)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.position = position;
            cube.transform.localScale = scale;
            SetColor(cube, color);
        }

        private static void SetColor(GameObject go, Color color)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer == null) return;
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) return;
            EnsureFolder(Root + "/Art/Materials");
            string safeName = go.name.Replace(" ", "_");
            string materialPath = Root + "/Art/Materials/Blockout_" + safeName + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material == null)
            {
                material = new Material(shader);
                if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
                if (material.HasProperty("_Color")) material.SetColor("_Color", color);
                AssetDatabase.CreateAsset(material, materialPath);
            }
            renderer.sharedMaterial = material;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            string leaf = Path.GetFileName(path);
            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent)) EnsureFolder(parent);
            if (!string.IsNullOrEmpty(parent)) AssetDatabase.CreateFolder(parent, leaf);
        }
    }
}
