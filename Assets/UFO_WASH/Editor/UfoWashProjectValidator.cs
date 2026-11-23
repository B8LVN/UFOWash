#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace UFOWASH.Editor
{
    public static class UfoWashProjectValidator
    {
        const string ScenePath = "Assets/UFO_WASH/Scenes/UFO_WASH_PowerSlice.unity";

        [MenuItem("UFO WASH/QA/VALIDATE ACTIVE GAME SCENE")]
        public static void ValidateActiveScene()
        {
            var errors = new List<string>();
            var warnings = new List<string>();
            var scene = SceneManager.GetActiveScene();

            if (scene.path != ScenePath)
                errors.Add("Active scene is not the generated UFO WASH scene: " + scene.path);

            if (!(GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset))
                errors.Add("URP is not the active render pipeline.");

            Require<UfoWashFirstPersonController>("First-person controller", errors);
            Require<UfoWashSprayTool>("Washer tool", errors);
            Require<UfoWashPlayerInteraction>("Player interaction", errors);
            Require<UfoWashCareerLoop>("Career / contract manager", errors);
            Require<UfoWashJobBoard>("Contract terminal", errors);
            Require<UfoWashSupplyStation>("Supply station", errors);
            Require<UfoWashCustomerShip>("Customer ship docking controller", errors);
            Require<UfoWashCustomerNpc>("Interactive customer NPC", errors);
            Require<UfoWashHUD>("HUD", errors);

            var surfaces = Object.FindObjectsByType<UfoWashDirtSurface>(FindObjectsSortMode.None);
            if (surfaces.Length < 2)
                errors.Add("Expected at least two independently cleanable ship surfaces; found " + surfaces.Length + ".");

            var renderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
            int materialCount = 0;
            foreach (var renderer in renderers)
            {
                foreach (var material in renderer.sharedMaterials)
                {
                    if (material == null)
                    {
                        errors.Add("Missing material on renderer: " + renderer.name);
                        continue;
                    }

                    materialCount++;
                    if (material.shader == null)
                    {
                        errors.Add("Material has no shader: " + material.name);
                        continue;
                    }

                    string shaderName = material.shader.name;
                    bool supported = shaderName.StartsWith("Universal Render Pipeline/") ||
                        shaderName == "GUI/Text Shader" || shaderName == "UI/Default" ||
                        shaderName == "Sprites/Default";
                    if (!supported)
                        warnings.Add("Non-URP shader requires review: " + material.name + " [" + shaderName + "]");
                }
            }

            bool sceneInBuildSettings = false;
            foreach (var buildScene in EditorBuildSettings.scenes)
                if (buildScene.enabled && buildScene.path == ScenePath) sceneInBuildSettings = true;
            if (!sceneInBuildSettings)
                warnings.Add("Generated game scene is not enabled in Build Settings.");

            int missingScripts = 0;
            foreach (var root in scene.GetRootGameObjects())
                missingScripts += GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(root);
            if (missingScripts > 0)
                errors.Add("Missing MonoBehaviour scripts: " + missingScripts);

            Debug.Log("UFO WASH QA: checked " + renderers.Length + " renderers and " +
                materialCount + " material slots; " + surfaces.Length + " washable surfaces.");

            foreach (var warning in warnings) Debug.LogWarning("UFO WASH QA: " + warning);
            if (errors.Count == 0)
                Debug.Log("<color=#58d68d>UFO WASH QA PASSED structural checks.</color> Run the manual gameplay checklist as well.");
            else
            {
                foreach (var error in errors) Debug.LogError("UFO WASH QA: " + error);
                Debug.LogError("UFO WASH QA FAILED: " + errors.Count + " blocking structural issue(s).");
            }
        }

        static void Require<T>(string label, List<string> errors) where T : Component
        {
            if (Object.FindFirstObjectByType<T>() == null)
                errors.Add("Missing required scene component: " + label);
        }
    }
}
#endif
