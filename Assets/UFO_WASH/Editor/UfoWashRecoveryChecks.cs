#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UFOWASH.Editor
{
    // Run ONLY in a disposable project copy: this is a rule-level Play Mode
    // fixture, not a substitute for the authored bay, input or screenshot QA.
    [InitializeOnLoad]
    public static class UfoWashRecoveryChecks
    {
        const string RunningKey = "UFO_WASH_RECOVERY_CHECKS";
        static int phase;
        static double deadline;
        static int creditsAfterFirst;

        static UfoWashRecoveryChecks()
        {
            if (SessionState.GetBool(RunningKey, false)) EditorApplication.update += Tick;
        }

        public static void RunBatch()
        {
            if (!Application.isBatchMode) throw new InvalidOperationException("Use a disposable batch-mode project copy.");
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            var root = new GameObject("CHECKS // CUSTOMER");
            root.SetActive(false);
            for (int i = 0; i < 2; i++)
            {
                var panel = GameObject.CreatePrimitive(PrimitiveType.Plane);
                panel.name = "Check Panel " + i;
                panel.transform.SetParent(root.transform, false);
                panel.transform.localPosition = new Vector3(i * 3, 1, 0);
                panel.transform.localScale = Vector3.one * 0.1f;
                panel.AddComponent<UfoWashDirtSurface>();
            }
            root.AddComponent<UfoWashCustomerShip>();
            root.SetActive(true);
            new GameObject("CHECKS // CAREER").AddComponent<UfoWashCareerLoop>();
            var player = new GameObject("CHECKS // TOOL");
            new GameObject("CHECKS // CAMERA").transform.SetParent(player.transform);
            player.GetComponentInChildren<Transform>().gameObject.AddComponent<Camera>();
            player.AddComponent<UfoWashSprayTool>().enabled = false;
            new GameObject("CHECKS // UPGRADE").AddComponent<UfoWashUpgradeStation>();
            // Saving only the disposable fixture allows Unity's domain reload.
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/UFO_WASH/Scenes/RecoveryChecks.unity");
            SessionState.SetBool(RunningKey, true);
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
            EditorApplication.isPlaying = true;
        }

        static void Tick()
        {
            if (!EditorApplication.isPlaying) return;
            if (deadline == 0) deadline = EditorApplication.timeSinceStartup + 45;
            try
            {
                if (EditorApplication.timeSinceStartup > deadline) throw new Exception("Timed out waiting for ship turnaround.");
                var career = UfoWashCareerLoop.Instance;
                if (career == null || !career.CanAcceptJob) return;
                var ship = UnityEngine.Object.FindAnyObjectByType<UfoWashCustomerShip>();
                var panels = ship.ServiceSurfaces;
                if (phase == 0)
                {
                    UfoWashHUD.Credits = 750;
                    career.AcceptJob(0);
                    Check(career.JobAccepted, "first contract accepted");
                    float chemical = career.Chemical;
                    Check(!Apply(career, panels[0], UfoWashToolMode.Foam, 1f) && career.Chemical == chemical,
                        "wrong stage consumes no chemical");
                    Apply(career, panels[0], UfoWashToolMode.PreRinse, 1000f);
                    Check(career.Water == 0f && career.CurrentStage == UfoWashToolMode.PreRinse,
                        "untouched panel holds stage; water clamps at zero");
                    Check(!Apply(career, panels[1], UfoWashToolMode.PreRinse, 1f), "empty tank rejects work");
                    career.RefillSupplies();
                    Check(UfoWashHUD.Credits == 685 && career.Water == 100f, "refill charges exactly 65");
                    Finish(career, panels);
                    int expected = 685 + 250 + Mathf.RoundToInt(250 * 0.25f) + 20;
                    Check(UfoWashHUD.Credits == expected && career.CompletedJobs == 1, "first receipt pays disclosed amounts");
                    Check(Mathf.Abs(career.Reputation - 2.35f) < 0.001f, "perfect first job awards reputation");
                    career.CompleteJob();
                    Check(UfoWashHUD.Credits == expected && career.CompletedJobs == 1, "duplicate inspection cannot pay twice");
                    career.AcceptJob(1);
                    Check(!career.JobAccepted, "cannot accept during departure");
                    var tool = UnityEngine.Object.FindAnyObjectByType<UfoWashSprayTool>();
                    var station = UnityEngine.Object.FindAnyObjectByType<UfoWashUpgradeStation>();
                    station.Interact(tool.gameObject);
                    Check(tool.FanNozzleInstalled && tool.FanNozzleActive && tool.ActiveRadius >= 1.05f &&
                        UfoWashHUD.Credits == expected - 200, "purchase unlocks switchable wide fan");
                    creditsAfterFirst = UfoWashHUD.Credits;
                    phase = 1;
                }
                else
                {
                    career.AcceptJob(1);
                    Check(career.JobAccepted && panels[0].foam == 0f && Mathf.Abs(panels[0].dirt - 0.9f) < 0.001f,
                        "second offer resets both local coatings after turnaround");
                    Finish(career, panels);
                    Check(career.CompletedJobs == 2 && UfoWashHUD.Credits > creditsAfterFirst && career.Reputation > 2.35f,
                        "second contract pays and raises reputation");
                    Debug.Log("UFO WASH RECOVERY CHECKS PASSED // Rule fixture only; authored-bay input/visual QA still required.");
                    SessionState.SetBool(RunningKey, false);
                    EditorApplication.Exit(0);
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                SessionState.SetBool(RunningKey, false);
                EditorApplication.Exit(1);
            }
        }

        static bool Apply(UfoWashCareerLoop career, UfoWashDirtSurface panel, UfoWashToolMode mode, float seconds)
            => career.ApplyTool(panel, mode, seconds, panel.transform.position, panel.transform.up, 1.5f);

        static void Finish(UfoWashCareerLoop career, UfoWashDirtSurface[] panels)
        {
            foreach (var panel in panels) Apply(career, panel, UfoWashToolMode.PreRinse, 1f);
            Check(career.CurrentStage == UfoWashToolMode.Foam, "each panel passes pre-rinse");
            Apply(career, panels[0], UfoWashToolMode.Foam, 0.6f);
            Check(career.CurrentStage == UfoWashToolMode.Foam, "unfoamed panel holds stage");
            Apply(career, panels[1], UfoWashToolMode.Foam, 0.6f);
            Check(career.CurrentStage == UfoWashToolMode.FinalRinse, "each panel passes foam");
            Apply(career, panels[0], UfoWashToolMode.FinalRinse, 3f);
            Check(!career.CanSubmit, "unrinsed panel blocks inspection");
            Apply(career, panels[1], UfoWashToolMode.FinalRinse, 3f);
            Check(career.CanSubmit, "each panel passes final rinse");
            career.CompleteJob();
        }

        static void Check(bool condition, string message)
        {
            if (!condition) throw new Exception("RECOVERY CHECK FAILED // " + message);
            Debug.Log("RECOVERY CHECK PASS // " + message);
        }
    }
}
#endif
