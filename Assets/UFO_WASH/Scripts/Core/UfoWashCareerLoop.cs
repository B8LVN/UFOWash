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

        public bool JobAccepted { get; private set; }
        public int CompletedJobs { get; private set; }
        public string LastMessage { get; private set; } = "A new contract is waiting";
        public UfoWashToolMode CurrentStage { get; private set; } = UfoWashToolMode.PreRinse;
        public float Water { get; private set; } = 100f;
        public float Chemical { get; private set; } = 100f;
        UfoWashCustomerShip customerShip;
        UfoWashCustomerNpc customerNpc;
        UfoWashDirtSurface[] contractSurfaces = System.Array.Empty<UfoWashDirtSurface>();
        int activeOfferIndex = -1;
        public float Reputation { get; private set; } = 2.1f;
        public bool CanAcceptJob => !JobAccepted && contractSurfaces.Length > 0 && (customerShip == null || customerShip.IsDocked);
        public float Quality => Mathf.Clamp01(1f - UfoWashDirtSurface.AverageDirt(contractSurfaces) -
            UfoWashDirtSurface.AverageFoam(contractSurfaces) * 0.18f);
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
                if (surface.dirt <= 0.15f && surface.foam <= 0.10f) state = "CERTIFIED";
                else if (surface.dirt > 0.58f) state = "PRE-RINSE";
                else if (surface.foam < 0.62f) state = "FOAM COVERAGE";
                else state = "FINAL RINSE";
                if (lines.Length > 0) lines.Append('\n');
                lines.Append("[").Append(state == "CERTIFIED" ? "OK" : "  ").Append("] ")
                    .Append(GetZoneName(surface)).Append("  ·  ").Append(state);
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
            if (contractSurfaces == null || contractSurfaces.Length == 0)
                contractSurfaces = FindObjectsByType<UfoWashDirtSurface>();
            customerNpc = FindAnyObjectByType<UfoWashCustomerNpc>();
            if (UfoWashHUD.Credits == 0) UfoWashHUD.Credits = 750;
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

            activeOfferIndex = Mathf.Clamp(offerIndex, 0, OfferTitles.Length - 1);
            JobAccepted = true;
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
            Vector3 hitPoint, Vector3 hitNormal, float sprayRadius)
        {
            if (!JobAccepted || surface == null || seconds <= 0f) return false;
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

            float supplyCost = seconds * (mode == UfoWashToolMode.Foam ? 1.5f : 0.8f);
            if (mode == UfoWashToolMode.Foam)
            {
                if (Chemical <= 0f) { LastMessage = "CHEMICAL EMPTY // REFILL AT SUPPLY STATION"; return false; }
                Chemical = Mathf.Max(0f, Chemical - supplyCost);
            }
            else
            {
                if (Water <= 0f) { LastMessage = "WATER TANK EMPTY // REFILL AT SUPPLY STATION"; return false; }
                Water = Mathf.Max(0f, Water - supplyCost);
            }

            surface.Process(mode, seconds, hitPoint, hitNormal, sprayRadius);
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
        }

        public void CompleteJob()
        {
            if (!CanSubmit) return;

            int basePayout = Reward;
            int qualityBonus = Mathf.RoundToInt(basePayout * Mathf.Clamp01((Quality - 0.85f) / 0.15f) * 0.25f);
            int tip = Quality >= OfferTipQuality[activeOfferIndex]
                ? Mathf.RoundToInt(basePayout * OfferTipRates[activeOfferIndex]) : 0;
            int payout = basePayout + qualityBonus + tip;
            string grade = QualityGrade;
            Reputation = Mathf.Clamp(Reputation + (Quality >= 0.985f ? 0.25f : Quality >= 0.92f ? 0.16f : Quality >= 0.84f ? 0.08f : -0.15f), 1f, 5f);

            JobAccepted = false;
            CompletedJobs++;
            UfoWashHUD.Credits += payout;
            if (customerNpc != null) customerNpc.OnContractCompleted(grade, tip);
            if (customerShip != null) customerShip.DepartAndReturn();
            activeOfferIndex = -1;
            UfoWashHUD.JobComplete = true;
            LastMessage = "INSPECTION PASSED // GRADE " + grade + " // BASE " + basePayout +
                " + QUALITY BONUS " + qualityBonus + " + CUSTOMER TIP " + tip + " CR // REPUTATION " + Reputation.ToString("0.0");
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
