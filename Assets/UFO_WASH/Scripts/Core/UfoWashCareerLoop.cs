using UnityEngine;

namespace UFOWASH
{
    public sealed class UfoWashCareerLoop : MonoBehaviour
    {
        public static UfoWashCareerLoop Instance { get; private set; }

        static readonly string[] OfferTitles = { "QUICK DECONTAMINATION", "DEEP HULL CLEAN", "VIP SHOWROOM FINISH" };
        static readonly string[] OfferClients = { "ZORP // ORION TRADERS", "NYX-7 // DEEP SPACE COURIER", "CAPTAIN VEX // OUTER RIM" };
        static readonly string[] OfferBriefs = {
            "A routine route has left a dusty film across the scout. Standard foam treatment and a clean rinse will do.",
            "Engine residue and asteroid sludge have baked onto the hull. Thorough coverage is required before inspection.",
            "A VIP pilot needs a spotless finish before departure. Take your time and leave no residue behind."
        };
        static readonly string[] OfferContamination = {
            "ASTEROID DUST + IONIC SLUDGE",
            "PLASMA SOOT + ENGINE RESIDUE",
            "BIO-LUMINESCENT SPACE MOLD"
        };
        static readonly Color[] ContaminationColors = {
            new Color(0.68f, 0.36f, 0.16f),
            new Color(0.30f, 0.25f, 0.48f),
            new Color(0.24f, 0.52f, 0.30f)
        };
        static readonly float[] OfferStartingDirt = { 0.72f, 0.9f, 1f };
        static readonly int[] OfferPayouts = { 250, 420, 600 };
        static readonly string[] OfferTraits = { "RELIABLE COURIER", "SCRAP PROSPECTOR", "PERFECTIONIST NOBLE" };
        static readonly float[] OfferTipRates = { 0.08f, 0.16f, 0.35f };
        static readonly float[] OfferTipQuality = { 0.84f, 0.90f, 0.985f };
        static readonly float[] OfferReputationRequirements = { 1f, 2.1f, 3.8f };
        static readonly float[] OfferRushWindows = { 300f, 0f, 0f };
        static readonly int[] OfferRushBonuses = { 45, 0, 0 };

        public bool JobAccepted { get; private set; }
        public int CompletedJobs { get; private set; }
        public UfoWashDirtSurface[] ContractSurfaces => contractSurfaces;
        public string LastMessage { get; private set; } = "A new contract is waiting";
        public UfoWashToolMode CurrentStage { get; private set; } = UfoWashToolMode.PreRinse;
        public float Water { get; private set; } = 100f;
        public float Chemical { get; private set; } = 100f;
        public string LastReceipt { get; private set; } = string.Empty;
        public int LastPayout { get; private set; }
        UfoWashCustomerShip customerShip;
        UfoWashCustomerNpc customerNpc;
        UfoWashDirtSurface[] contractSurfaces = System.Array.Empty<UfoWashDirtSurface>();
        int activeOfferIndex = -1;
        float activeJobStartedAt;
        float supplyUseMultiplier = 1f;
        public float Reputation { get; private set; } = 2.1f;
        public int SupplyEfficiencyPercent => Mathf.RoundToInt((1f - supplyUseMultiplier) * 100f);
        public bool CanAcceptJob => !JobAccepted && contractSurfaces.Length > 0 && (customerShip == null || customerShip.IsDocked);
        public float Quality => Mathf.Clamp01(1f - UfoWashDirtSurface.AverageDirt(contractSurfaces) -
            UfoWashDirtSurface.AverageFoam(contractSurfaces) * 0.18f);
        public float StageProgress
        {
            get
            {
                if (!JobAccepted || contractSurfaces.Length == 0) return 0f;
                float sum = 0f;
                int count = 0;
                float startingDirt = OfferStartingDirt[Mathf.Clamp(activeOfferIndex, 0, OfferStartingDirt.Length - 1)];
                foreach (var surface in contractSurfaces)
                {
                    if (surface == null) continue;
                    float progress = CurrentStage == UfoWashToolMode.PreRinse
                        ? Mathf.InverseLerp(startingDirt, 0.58f, surface.dirt)
                        : CurrentStage == UfoWashToolMode.Foam
                            ? Mathf.Clamp01(surface.foam / 0.62f)
                            : Mathf.Clamp01((0.62f - surface.foam) / 0.52f) *
                              Mathf.Clamp01((0.58f - surface.dirt) / 0.43f);
                    sum += progress;
                    count++;
                }
                return count == 0 ? 0f : sum / count;
            }
        }
        public bool CanSubmit => JobAccepted && CurrentStage == UfoWashToolMode.FinalRinse &&
            AllZonesAtMost(0.15f, 0.10f);
        public string QualityGrade => Quality >= 0.985f ? "S" : Quality >= 0.95f ? "A" :
            Quality >= 0.90f ? "B" : Quality >= 0.85f ? "C" : "D";

        public int OfferCount => OfferTitles.Length;
        public string Client => activeOfferIndex >= 0 ? OfferClients[activeOfferIndex] : "NO ACTIVE CLIENT";
        public string Ship => "CLASS-IV SCOUT";
        public string ContaminationType => activeOfferIndex >= 0 ? OfferContamination[activeOfferIndex] : OfferContamination[CompletedJobs % OfferContamination.Length];
        public int Reward => activeOfferIndex >= 0 ? OfferPayouts[activeOfferIndex] : OfferPayouts[0];
        public string GetOfferTitle(int index) => OfferTitles[Mathf.Clamp(index, 0, OfferTitles.Length - 1)];
        public string GetOfferClient(int index) => OfferClients[Mathf.Clamp(index, 0, OfferClients.Length - 1)];
        public string GetOfferBrief(int index) => OfferBriefs[Mathf.Clamp(index, 0, OfferBriefs.Length - 1)];
        public string GetOfferContamination(int index) => OfferContamination[Mathf.Clamp(index, 0, OfferContamination.Length - 1)];
        public int GetOfferReward(int index) => OfferPayouts[Mathf.Clamp(index, 0, OfferPayouts.Length - 1)];
        public string GetOfferTrait(int index) => OfferTraits[Mathf.Clamp(index, 0, OfferTraits.Length - 1)];
        public string GetOfferRushSummary(int index)
        {
            int safeIndex = Mathf.Clamp(index, 0, OfferRushWindows.Length - 1);
            if (OfferRushWindows[safeIndex] <= 0f) return "NO DEADLINE // WORK AT YOUR OWN PACE";
            int seconds = Mathf.CeilToInt(OfferRushWindows[safeIndex]);
            return "EXPRESS BONUS // " + (seconds / 60).ToString("00") + ":" +
                (seconds % 60).ToString("00") + " FOR +" + OfferRushBonuses[safeIndex] + " CR // NO LATE PENALTY";
        }
        public int GetOfferPotentialTip(int index)
        {
            int safeIndex = Mathf.Clamp(index, 0, OfferPayouts.Length - 1);
            return Mathf.RoundToInt(OfferPayouts[safeIndex] * OfferTipRates[safeIndex]);
        }
        public float GetOfferTipQuality(int index) => OfferTipQuality[Mathf.Clamp(index, 0, OfferTipQuality.Length - 1)];
        public bool GetOfferUnlocked(int index) => Reputation >= OfferReputationRequirements[Mathf.Clamp(index, 0, OfferReputationRequirements.Length - 1)];
        public string GetOfferUnlockRequirement(int index) => "REPUTATION " + OfferReputationRequirements[Mathf.Clamp(index, 0, OfferReputationRequirements.Length - 1)].ToString("0.0") + " STARS";
        public string StageLabel => CurrentStage == UfoWashToolMode.PreRinse ? "01 / PRE-RINSE" :
            CurrentStage == UfoWashToolMode.Foam ? "02 / FOAM COVERAGE" : "03 / FINAL RINSE";
        public bool HasExpressWindow => JobAccepted && activeOfferIndex >= 0 &&
            OfferRushWindows[activeOfferIndex] > 0f;
        public int RushBonus => HasExpressWindow &&
            Time.time - activeJobStartedAt <= OfferRushWindows[activeOfferIndex]
                ? OfferRushBonuses[activeOfferIndex] : 0;
        public string RushStatus
        {
            get
            {
                if (!HasExpressWindow) return string.Empty;
                if (RushBonus <= 0) return "EXPRESS WINDOW MISSED  //  NO PAYOUT PENALTY";
                int seconds = Mathf.CeilToInt(Mathf.Max(0f,
                    OfferRushWindows[activeOfferIndex] - (Time.time - activeJobStartedAt)));
                return "EXPRESS BONUS  //  " + (seconds / 60).ToString("00") + ":" +
                    (seconds % 60).ToString("00") + "  //  +" + OfferRushBonuses[activeOfferIndex] + " CR";
            }
        }

        public bool CanUseTool(UfoWashToolMode mode) => JobAccepted && mode == CurrentStage &&
            (mode == UfoWashToolMode.Foam ? Chemical > 0f : Water > 0f);

        public void SetBayRecoveryTier(int tier)
        {
            int safeTier = Mathf.Clamp(tier, 0, 2);
            supplyUseMultiplier = safeTier == 0 ? 1f : safeTier == 1 ? 0.85f : 0.70f;
        }

        public string ToolBlockReason(UfoWashToolMode mode)
        {
            if (!JobAccepted) return "ACCEPT A CONTRACT AT DISPATCH";
            if (mode != CurrentStage) return "SELECT " + StageLabel + " // KEYS 1 / 2 / 3";
            return mode == UfoWashToolMode.Foam ? "CHEMICAL EMPTY // SUPPLIES 65 CR" : "WATER EMPTY // SUPPLIES 65 CR";
        }

        public bool IsContractSurface(UfoWashDirtSurface surface)
        {
            if (surface == null) return false;
            for (int i = 0; i < contractSurfaces.Length; i++)
                if (contractSurfaces[i] == surface) return true;
            return false;
        }

        public string GetZoneChecklist()
        {
            if (!JobAccepted) return "ACCEPT A CONTRACT TO BEGIN THE HULL INSPECTION";
            if (contractSurfaces.Length == 0) return "NO SERVICE PANELS FOUND // CONTRACT BOARD OFFLINE";

            var lines = new System.Text.StringBuilder();
            for (int i = 0; i < contractSurfaces.Length; i++)
            {
                var surface = contractSurfaces[i];
                if (surface == null) continue;
                string state;
                // Completion refers to the current ordered stage, not a guess
                // from foam amount (which decreases during final rinse).
                bool ready = CurrentStage == UfoWashToolMode.PreRinse ? surface.dirt <= 0.58f :
                    CurrentStage == UfoWashToolMode.Foam ? surface.foam >= 0.62f :
                    surface.dirt <= 0.15f && surface.foam <= 0.10f;
                state = CurrentStage == UfoWashToolMode.PreRinse ?
                    "DUST " + Mathf.RoundToInt(surface.dirt * 100f) + "% / <=58%" :
                    CurrentStage == UfoWashToolMode.Foam ?
                    "FOAM " + Mathf.RoundToInt(surface.foam * 100f) + "% / >=62%" :
                    "DIRT " + Mathf.RoundToInt(surface.dirt * 100f) + "% / FOAM " + Mathf.RoundToInt(surface.foam * 100f) + "%";
                if (lines.Length > 0) lines.Append('\n');
                lines.Append(ready ? "[OK] " : "[  ] ").Append(GetZoneName(surface)).Append('\n')
                    .Append("       ").Append(state);
            }
            return lines.ToString();
        }

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            customerShip = FindAnyObjectByType<UfoWashCustomerShip>();
            if (customerShip != null) contractSurfaces = customerShip.ServiceSurfaces;
            if (customerShip == null)
                contractSurfaces = FindObjectsByType<UfoWashDirtSurface>(FindObjectsSortMode.None);
            if (contractSurfaces == null) contractSurfaces = System.Array.Empty<UfoWashDirtSurface>();
            customerNpc = FindAnyObjectByType<UfoWashCustomerNpc>();
            if (UfoWashHUD.Credits == 0) UfoWashHUD.Credits = 750;
            if (UfoWashProgressSave.Instance != null)
                UfoWashProgressSave.Instance.RestoreCareer(this);
        }

        public void RestoreProgress(int credits, float reputation, float water, float chemical, int completedJobs)
        {
            UfoWashHUD.Credits = Mathf.Max(0, credits);
            Reputation = Mathf.Clamp(reputation, 1f, 5f);
            Water = Mathf.Clamp(water, 0f, 100f);
            Chemical = Mathf.Clamp(chemical, 0f, 100f);
            CompletedJobs = Mathf.Max(0, completedJobs);
            JobAccepted = false;
            activeOfferIndex = -1;
            CurrentStage = UfoWashToolMode.PreRinse;
            UfoWashHUD.JobComplete = false;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void AcceptJob() => AcceptJob(0);

        public void AcceptJob(int offerIndex)
        {
            if (JobAccepted) return;
            if (!CanAcceptJob)
            {
                LastMessage = "CUSTOMER VESSEL INBOUND // WAIT FOR DOCKING";
                return;
            }

            if (!GetOfferUnlocked(offerIndex))
            {
                LastMessage = "CONTRACT LOCKED // REACH " + GetOfferUnlockRequirement(offerIndex);
                return;
            }

            // Fail before accepting an unserviceable contract, never silently fall
            // back to the old global tint or charge supplies for missing visuals.
            foreach (var surface in contractSurfaces)
                if (surface == null || !surface.PrepareCoating())
                {
                    LastMessage = "CONTRACT BLOCKED // HULL COATING SETUP FAILED // CHECK CONSOLE";
                    return;
                }
            activeOfferIndex = Mathf.Clamp(offerIndex, 0, OfferTitles.Length - 1);
            JobAccepted = true;
            activeJobStartedAt = Time.time;
            CurrentStage = UfoWashToolMode.PreRinse;
            UfoWashHUD.JobComplete = false;
            UfoWashDirtSurface.SetJobProfile(contractSurfaces,
                ContaminationColors[activeOfferIndex], OfferStartingDirt[activeOfferIndex]);
            if (customerNpc != null)
            {
                string customerName = OfferClients[activeOfferIndex].Split('/')[0].Trim();
                customerNpc.SetCustomerIdentity(customerName, OfferTraits[activeOfferIndex]);
                customerNpc.OnContractAccepted();
            }
            LastMessage = "CONTRACT ACCEPTED // " + OfferTitles[activeOfferIndex] + " // PRE-RINSE THE HULL";
        }

        public bool ApplyTool(UfoWashDirtSurface surface, UfoWashToolMode mode, float seconds,
            Vector3 hitPoint, Vector3 hitNormal, float sprayRadius, float power = 1f)
        {
            if (!JobAccepted || surface == null || seconds <= 0f || !float.IsFinite(seconds) ||
                !float.IsFinite(power) || power <= 0f) return false;
            if (!IsContractSurface(surface))
            {
                LastMessage = "THIS SURFACE IS OUTSIDE THE ACTIVE SERVICE ORDER";
                return false;
            }
            if (mode != CurrentStage)
            {
                LastMessage = "WRONG SERVICE STEP // " + StageLabel;
                return false;
            }

            if (!surface.CoatingReady) return false;
            float rate = (mode == UfoWashToolMode.Foam ? 1.2f : 0.65f) * supplyUseMultiplier;
            // A nearly empty tank buys only the remaining fraction of a frame.
            // Pauses/stalls cannot grant seconds of free work for the last drop.
            float available = mode == UfoWashToolMode.Foam ? Chemical : Water;
            float actualSeconds = Mathf.Min(seconds, available / rate);
            float supplyCost = actualSeconds * rate;
            if (mode == UfoWashToolMode.Foam && Chemical <= 0f)
            { LastMessage = "CHEMICAL EMPTY // REFILL AT SUPPLY STATION"; return false; }
            if (mode != UfoWashToolMode.Foam && Water <= 0f)
            { LastMessage = "WATER TANK EMPTY // REFILL AT SUPPLY STATION"; return false; }

            if (!surface.Process(mode, actualSeconds * power, hitPoint, hitNormal, sprayRadius))
            {
                LastMessage = "ALREADY TREATED HERE // SWEEP TO AN UNFINISHED HULL ZONE";
                return false;
            }
            if (mode == UfoWashToolMode.Foam) Chemical = Mathf.Max(0f, Chemical - supplyCost);
            else Water = Mathf.Max(0f, Water - supplyCost);
            AdvanceStageIfReady();
            return true;
        }

        public bool ApplyTool(UfoWashDirtSurface surface, UfoWashToolMode mode, float seconds)
        {
            return ApplyTool(surface, mode, seconds, surface != null ? surface.transform.position : Vector3.zero,
                surface != null ? surface.transform.up : Vector3.up, 0.3f);
        }

        void AdvanceStageIfReady()
        {
            if (CurrentStage == UfoWashToolMode.PreRinse && AllZonesAtMost(0.58f, -1f))
            {
                CurrentStage = UfoWashToolMode.Foam;
                LastMessage = "ALL HULL ZONES PRE-RINSED // SWITCH TO FOAM (2)";
            }
            else if (CurrentStage == UfoWashToolMode.Foam && AllZonesAtLeastFoam(0.62f))
            {
                CurrentStage = UfoWashToolMode.FinalRinse;
                LastMessage = "EVERY ZONE COATED // FINAL RINSE (3)";
            }
        }

        public void RefillSupplies()
        {
            const int refillCost = 65;
            if (Water >= 99f && Chemical >= 99f)
            {
                LastMessage = "SUPPLIES ALREADY FULL";
                return;
            }
            if (UfoWashHUD.Credits < refillCost)
            {
                LastMessage = "INSUFFICIENT CREDITS // REFILL COST 65 CR";
                return;
            }
            UfoWashHUD.Credits -= refillCost;
            Water = 100f;
            Chemical = 100f;
            LastMessage = "SUPPLIES REFILLED // -65 CR";
            if (UfoWashProgressSave.Instance != null)
                UfoWashProgressSave.Instance.SaveNow();
        }

        public void CompleteJob()
        {
            if (!CanSubmit) return;

            int basePayout = Reward;
            int qualityBonus = Mathf.RoundToInt(basePayout * Mathf.Clamp01((Quality - 0.85f) / 0.15f) * 0.25f);
            int tip = Quality >= OfferTipQuality[activeOfferIndex]
                ? Mathf.RoundToInt(basePayout * OfferTipRates[activeOfferIndex]) : 0;
            int rushBonus = RushBonus;
            int payout = basePayout + qualityBonus + tip + rushBonus;
            string grade = QualityGrade;
            Reputation = Mathf.Clamp(Reputation + (Quality >= 0.985f ? 0.25f : Quality >= 0.92f ? 0.16f : Quality >= 0.84f ? 0.08f : -0.15f), 1f, 5f);

            JobAccepted = false;
            CompletedJobs++;
            UfoWashHUD.Credits += payout;
            LastPayout = payout;
            LastReceipt = "GRADE " + grade + "  //  BASE " + basePayout + " + QUALITY " + qualityBonus +
                " + TIP " + tip + " + EXPRESS " + rushBonus + " = " + payout + " CR\nREPUTATION " + Reputation.ToString("0.00") +
                " STARS  //  JOBS CERTIFIED " + CompletedJobs;
            if (customerNpc != null) customerNpc.OnContractCompleted(grade, tip);
            if (customerShip != null) customerShip.DepartAndReturn();
            activeOfferIndex = -1;
            UfoWashHUD.JobComplete = true;
            LastMessage = "INSPECTION PASSED // GRADE " + grade + " // BASE " + basePayout +
                " + QUALITY BONUS " + qualityBonus + " + CUSTOMER TIP " + tip +
                " + EXPRESS " + rushBonus + " CR // REPUTATION " + Reputation.ToString("0.0");
            if (UfoWashProgressSave.Instance != null)
                UfoWashProgressSave.Instance.SaveNow();
        }

        public void ResetActiveJob()
        {
            if (!JobAccepted) return;
            CurrentStage = UfoWashToolMode.PreRinse;
            int profile = activeOfferIndex >= 0 ? activeOfferIndex : 0;
            UfoWashDirtSurface.SetJobProfile(contractSurfaces, ContaminationColors[profile], OfferStartingDirt[profile]);
            LastMessage = "CONTRACT RESET // ALL CONTAMINATION RESTORED";
        }

        public void NotifyBossMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message)) LastMessage = message;
        }

        bool AllZonesAtMost(float dirtLimit, float foamLimit)
        {
            if (contractSurfaces == null || contractSurfaces.Length == 0) return false;
            for (int i = 0; i < contractSurfaces.Length; i++)
            {
                var surface = contractSurfaces[i];
                if (surface == null) return false;
                if (surface.dirt > dirtLimit || (foamLimit >= 0f && surface.foam > foamLimit)) return false;
            }
            return true;
        }

        bool AllZonesAtLeastFoam(float foamLimit)
        {
            if (contractSurfaces == null || contractSurfaces.Length == 0) return false;
            for (int i = 0; i < contractSurfaces.Length; i++)
            {
                var surface = contractSurfaces[i];
                if (surface == null || surface.dirt > 0.58f || surface.foam < foamLimit) return false;
            }
            return true;
        }

        static string GetZoneName(UfoWashDirtSurface surface)
        {
            return surface.gameObject.name.Replace(" - contaminated", string.Empty).ToUpperInvariant();
        }
    }
}
