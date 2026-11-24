using System;
using System.Collections.Generic;
using UnityEngine;

namespace UFOWASH
{
    public sealed class UfoWashForensicReportManager : MonoBehaviour
    {
        public static UfoWashForensicReportManager Instance { get; private set; }
        const int MaxReports = 20;
        readonly List<ForensicReport> reportHistory = new List<ForensicReport>(MaxReports);

        public int ReportCount => reportHistory.Count;
        public ForensicReport LastReport => reportHistory.Count > 0 ? reportHistory[reportHistory.Count - 1] : null;

        public ForensicReport GetReport(int index)
        {
            if (index < 0 || index >= reportHistory.Count) return null;
            return reportHistory[index];
        }

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public ForensicReport GenerateReport(
            string clientName,
            string shipClass,
            string contaminationType,
            UfoWashDirtSurface[] contractSurfaces,
            int basePayout, int qualityBonus, int tip, int rushBonus,
            float reputationBefore, float reputationAfter,
            float waterUsedPercent, float chemicalUsedPercent, float timeSeconds)
        {
            var career = UfoWashCareerLoop.Instance;
            if (career == null) return null;

            float quality = career.Quality;
            string grade = career.QualityGrade;
            int gross = basePayout + qualityBonus + tip + rushBonus;
            int net = gross;

            var report = new ForensicReport
            {
                jobId = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpperInvariant(),
                clientName = clientName,
                shipClass = shipClass,
                contaminationType = contaminationType,
                grade = grade,
                qualityPercent = quality * 100f,
                basePayout = basePayout,
                qualityBonus = qualityBonus,
                tip = tip,
                rushBonus = rushBonus,
                gross = gross,
                net = net,
                reputationBefore = reputationBefore,
                reputationAfter = reputationAfter,
                reputationDelta = reputationAfter - reputationBefore,
                avgContamination = UfoWashDirtSurface.AverageDirt(contractSurfaces),
                avgFoam = UfoWashDirtSurface.AverageFoam(contractSurfaces),
                waterUsedPercent = waterUsedPercent,
                chemicalUsedPercent = chemicalUsedPercent,
                timeSeconds = timeSeconds,
            };

            BuildCauses(report, contractSurfaces, quality);

            reportHistory.Add(report);
            while (reportHistory.Count > MaxReports) reportHistory.RemoveAt(0);

            return report;
        }

        static bool AllZonesAtMost(UfoWashDirtSurface[] surfaces, float dirtLimit)
        {
            if (surfaces == null || surfaces.Length == 0) return false;
            for (int i = 0; i < surfaces.Length; i++)
                if (surfaces[i] == null || surfaces[i].dirt > dirtLimit) return false;
            return true;
        }

        static bool AllZonesAtLeastFoam(UfoWashDirtSurface[] surfaces, float foamLimit)
        {
            if (surfaces == null || surfaces.Length == 0) return false;
            for (int i = 0; i < surfaces.Length; i++)
                if (surfaces[i] == null || surfaces[i].foam < foamLimit) return false;
            return true;
        }

        void BuildCauses(ForensicReport report, UfoWashDirtSurface[] surfaces, float quality)
        {
            report.causes.Clear();
            if (surfaces == null) return;

            for (int i = 0; i < surfaces.Length; i++)
            {
                var s = surfaces[i];
                if (s == null) continue;

                string zone = s.gameObject.name.Replace(" - contaminated", string.Empty);

                if (s.dirt > 0.15f)
                {
                    report.causes.Add(new CauseEntry
                    {
                        zoneName = zone,
                        description = zone + " at " + (s.dirt * 100f).ToString("0") +
                            "% contamination; final limit 15%.",
                        suggestion = "Re-rinse this zone until below 15%.",
                        severity = Mathf.Clamp01(s.dirt)
                    });
                }

                if (s.foam > 0.10f)
                {
                    report.causes.Add(new CauseEntry
                    {
                        zoneName = zone,
                        description = zone + " at " + (s.foam * 100f).ToString("0") +
                            "% residual foam; final limit 10%.",
                        suggestion = "Rinse longer to remove foam before final inspection.",
                        severity = Mathf.Clamp01(s.foam)
                    });
                }
            }

            if (quality < 0.84f)
            {
                report.causes.Add(new CauseEntry
                {
                    zoneName = "Overall",
                    description = "Quality " + (quality * 100f).ToString("0") +
                        "% below 84% — no tip and reputation penalty.",
                    suggestion = "Improve cleaning thoroughness; target at least 84%.",
                    severity = 1f
                });
            }
            else if (quality < 0.90f)
            {
                report.causes.Add(new CauseEntry
                {
                    zoneName = "Overall",
                    description = "Quality " + (quality * 100f).ToString("0") +
                        "% below 90% — reduced tip tier.",
                    suggestion = "Clean more thoroughly to reach 90%+ for a better tip.",
                    severity = 0.7f
                });
            }
            else if (quality < 0.985f)
            {
                report.causes.Add(new CauseEntry
                {
                    zoneName = "Overall",
                    description = "Quality " + (quality * 100f).ToString("0") +
                        "% below 98.5% — missed VIP tip tier (35%).",
                    suggestion = "Use precision tools and ion polish for the 98.5% VIP finish.",
                    severity = 0.5f
                });
            }

            report.causes.Sort((a, b) => b.severity.CompareTo(a.severity));
            while (report.causes.Count > 5) report.causes.RemoveAt(5);
        }

        void OnGUI()
        {
            if (LastReport == null) return;
            if (UnityEngine.InputSystem.Keyboard.current != null &&
                UnityEngine.InputSystem.Keyboard.current.f1Key.wasPressedThisFrame)
                showReport = !showReport;
            if (!showReport) return;

            var r = LastReport;
            GUI.Box(new Rect(10, 10, 500, 400), "FORENSIC REPORT // JOB " + r.jobId);
            GUI.Label(new Rect(20, 30, 480, 20), "Client: " + r.clientName + "  |  Ship: " + r.shipClass);
            GUI.Label(new Rect(20, 50, 480, 20), "Grade: " + r.grade + "  |  Quality: " + r.qualityPercent.ToString("0") + "%");
            GUI.Label(new Rect(20, 70, 480, 20), "Payout: " + r.gross + " GC  |  Net: " + r.net + " GC");
            GUI.Label(new Rect(20, 90, 480, 20), "Rep: " + r.reputationBefore.ToString("0.0") + " → " + r.reputationAfter.ToString("0.0"));
            GUI.Label(new Rect(20, 110, 480, 20), "Causes (" + r.causes.Count + "):");
            int y = 130;
            for (int i = 0; i < r.causes.Count && i < 5; i++)
            {
                GUI.Label(new Rect(30, y, 460, 20), "- " + r.causes[i].description);
                y += 20;
            }
            GUI.Label(new Rect(20, 370, 480, 20), "Press F1 to close.");
        }

        bool showReport;
    }
}