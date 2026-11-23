using UnityEngine;

namespace UFOWASH
{
    public sealed class UfoWashCareerLoop : MonoBehaviour
    {
        public static UfoWashCareerLoop Instance { get; private set; }

        static readonly string[] Clients = { "ZORP // ORION TRADERS", "NYX-7 // DEEP SPACE COURIER", "CAPTAIN VEX // OUTER RIM" };
        static readonly string[] Ships = { "CLASS-IV SCOUT", "NEBULA RUNNER", "VOID SKIMMER" };
        static readonly string[] Contamination = { "ASTEROID DUST + IONIC SLUDGE", "PLASMA SOOT + ENGINE RESIDUE", "BIO-LUMINESCENT SPACE MOLD" };

        public bool JobAccepted { get; private set; }
        public int CompletedJobs { get; private set; }
        public string LastMessage { get; private set; } = "A new contract is waiting";
        public UfoWashToolMode CurrentStage { get; private set; } = UfoWashToolMode.PreRinse;
        public float Water { get; private set; } = 100f;
        public float Chemical { get; private set; } = 100f;
        public float Quality => Mathf.Clamp01(1f - UfoWashDirtSurface.TotalDirt() - UfoWashDirtSurface.TotalFoam() * 0.18f);
        public bool CanSubmit => JobAccepted && CurrentStage == UfoWashToolMode.FinalRinse &&
            UfoWashDirtSurface.TotalDirt() <= 0.15f && UfoWashDirtSurface.TotalFoam() <= 0.10f;
        public string QualityGrade => Quality >= 0.985f ? "S" : Quality >= 0.95f ? "A" :
            Quality >= 0.90f ? "B" : Quality >= 0.85f ? "C" : "D";

        public string Client => Clients[CompletedJobs % Clients.Length];
        public string Ship => Ships[CompletedJobs % Ships.Length];
        public string ContaminationType => Contamination[CompletedJobs % Contamination.Length];
        public int Reward => 250 + (CompletedJobs % 3) * 100;
        public string StageLabel => CurrentStage == UfoWashToolMode.PreRinse ? "01 / PRE-RINSE" :
            CurrentStage == UfoWashToolMode.Foam ? "02 / FOAM COVERAGE" : "03 / FINAL RINSE";

        void Awake()
        {
            Instance = this;
            if (UfoWashHUD.Credits == 0) UfoWashHUD.Credits = 750;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void AcceptJob()
        {
            if (JobAccepted) return;
            JobAccepted = true;
            CurrentStage = UfoWashToolMode.PreRinse;
            UfoWashHUD.JobComplete = false;
            UfoWashDirtSurface.ResetJob();
            LastMessage = "CONTRACT ACCEPTED // PRE-RINSE THE HULL";
        }

        public bool ApplyTool(UfoWashDirtSurface surface, UfoWashToolMode mode, float seconds)
        {
            if (!JobAccepted || surface == null || seconds <= 0f) return false;
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

            surface.Process(mode, seconds);
            AdvanceStageIfReady();
            return true;
        }

        void AdvanceStageIfReady()
        {
            if (CurrentStage == UfoWashToolMode.PreRinse && UfoWashDirtSurface.TotalDirt() <= 0.58f)
            {
                CurrentStage = UfoWashToolMode.Foam;
                LastMessage = "PRE-RINSE COMPLETE // SWITCH TO FOAM (2)";
            }
            else if (CurrentStage == UfoWashToolMode.Foam && UfoWashDirtSurface.TotalFoam() >= 0.62f)
            {
                CurrentStage = UfoWashToolMode.FinalRinse;
                LastMessage = "FOAM COVERAGE OK // FINAL RINSE (3)";
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
            int payout = basePayout + qualityBonus;
            string grade = QualityGrade;

            JobAccepted = false;
            CompletedJobs++;
            UfoWashHUD.Credits += payout;
            UfoWashHUD.JobComplete = true;
            LastMessage = "INSPECTION PASSED // GRADE " + grade + " // BASE " +
                basePayout + " + QUALITY BONUS " + qualityBonus + " CR";
        }

        public void ResetActiveJob()
        {
            if (!JobAccepted) return;
            CurrentStage = UfoWashToolMode.PreRinse;
            UfoWashDirtSurface.ResetJob();
            LastMessage = "CONTRACT RESET // ALL CONTAMINATION RESTORED";
        }
    }

    public sealed class UfoWashJobBoard : UfoWashInteractable
    {
        public override string GetPrompt()
        {
            var loop = UfoWashCareerLoop.Instance;
            if (loop == null) return "CONTRACT TERMINAL // OFFLINE";
            if (loop.JobAccepted && loop.CanSubmit)
                return "E  SUBMIT FOR INSPECTION  //  GRADE " + loop.QualityGrade;
            if (loop.JobAccepted) return "CONTRACT ACTIVE // " + loop.StageLabel;
            return "E  ACCEPT CONTRACT  //  " + loop.Reward + " CR";
        }

        public override void Interact(GameObject player)
        {
            var loop = UfoWashCareerLoop.Instance;
            if (loop == null) return;
            if (loop.JobAccepted && loop.CanSubmit) loop.CompleteJob();
            else if (!loop.JobAccepted) loop.AcceptJob();
        }
    }

    public sealed class UfoWashSupplyStation : UfoWashInteractable
    {
        public override string GetPrompt()
        {
            var loop = UfoWashCareerLoop.Instance;
            if (loop == null) return "SUPPLY STATION // OFFLINE";
            return "E  REFILL WATER + FOAM CHEMICAL  //  65 CR";
        }

        public override void Interact(GameObject player)
        {
            var loop = UfoWashCareerLoop.Instance;
            if (loop != null) loop.RefillSupplies();
        }
    }
}
