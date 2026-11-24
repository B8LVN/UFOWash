#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UFOWASH.Editor
{
    // Normal workflow: press Play -> title menu -> start the shift.
    // Never regenerate or save the owner's scene merely to test the game.
    [InitializeOnLoad]
    public static class UfoWashPlayModeEntry
    {
        const string MainScene = "Assets/UFO_WASH/Scenes/UFO_WASH_PowerSlice.unity";

        static UfoWashPlayModeEntry()
        {
            if (Application.isBatchMode) return; // Keep non-interactive editor startup independent.
            EditorApplication.delayCall += ConfigureStartScene;
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        static void ConfigureStartScene()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(MainScene);
            if (scene == null)
            {
                Debug.LogError("UFO WASH: main-menu scene is missing: " + MainScene);
                return;
            }
            EditorSceneManager.playModeStartScene = scene;
        }

        static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                Scene scene = SceneManager.GetActiveScene();
                if (scene.path != MainScene) return;
                try
                {
                    UfoWashPowerSliceBuilder.AddPlayModeServiceDetails(scene);
                    var frontEnd = Object.FindAnyObjectByType<UfoWashFrontEnd>();
                    if (frontEnd == null)
                        throw new System.InvalidOperationException("Main scene is missing the UFO WASH main menu.");
                    frontEnd.HideWorldSignsUntilGameplay();
                }
                catch (System.Exception exception)
                {
                    UfoWashPowerSliceBuilder.RemovePlayModeServiceDetails();
                    Debug.LogException(exception);
                    EditorApplication.isPlaying = false;
                }
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
                UfoWashPowerSliceBuilder.RemovePlayModeServiceDetails();
            else if (state == PlayModeStateChange.EnteredEditMode)
                ConfigureStartScene();
        }
    }
}
#endif
