using UnityEngine;

namespace UFOWASH
{
    public sealed class UfoWashCareerLoop : MonoBehaviour
    {
        public static UfoWashCareerLoop Instance { get; private set; }

        static readonly string[] Clients = { "ZORP // ORION TRADERS", "NYX-7 // DEEP SPACE COURIER", "CAPTAIN VEX // OUTER RIM" };
        // The first vertical slice contains one authored hull. New ship names
        // are not advertised until their actual prefabs and surface layouts exist.
        static readonly string[] Ships = { "CLASS-IV SCOUT" };
        static readonly Color[] ContaminationColors = {
            new Color(0.68f, 0.36f, 0.16f),
            new Color(0.30f, 0.25f, 0.48f),
            new Color(0.24f, 0.52f, 0.30f)
        };
        static readonly string[] Contamination = { "ASTEROID DUST + IONIC SLUDGE", "PLASMA SOOT + ENGINE RESIDUE", "BIO-LUMINESCENT SPACE MOLD" };

        public bool JobAccepted { get; private set; }
        public int CompletedJobs { get; private set; }
        public string LastMessage { get; private set; } = "A new contract is waiting";
        public UfoWashToolMode CurrentStage { get; private set; } = UfoWashToolMode.PreRinse;
        public float Water { get; private set; } = 100f;
        public float Chemical { get; private set; } = 100f;
        UfoWashCustomerShip customerShip;
        public bool CanAcceptJob => !JobAccepted && (customerShip == null || customerShip.IsDocked);
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
            customerShip = FindFirstObjectByType<UfoWashCustomerShip>();
            if (UfoWashHUD.Credits == 0) UfoWashHUD.Credits = 750;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void AcceptJob()
        {
            if (JobAccepted) return;
            if (!CanAcceptJob)
            {
                LastMessage = "CUSTOMER VESSEL INBOUND // WAIT FOR DOCKING";
                return;
            }
            JobAccepted = true;
            CurrentStage = UfoWashToolMode.PreRinse;
            UfoWashHUD.JobComplete = false;
            UfoWashDirtSurface.SetJobProfile(
                ContaminationColors[CompletedJobs % ContaminationColors.Length], 1f);
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
            if (customerShip != null) customerShip.DepartAndReturn();
            UfoWashHUD.JobComplete = true;
            LastMessage = "INSPECTION PASSED // GRADE " + grade + " // BASE " +
                basePayout + " + QUALITY BONUS " + qualityBonus + " CR";
        }

        public void ResetActiveJob()
        {
            if (!JobAccepted) return;
            CurrentStage = UfoWashToolMode.PreRinse;
            UfoWashDirtSurface.SetJobProfile(
                ContaminationColors[CompletedJobs % ContaminationColors.Length], 1f);
            LastMessage = "CONTRACT RESET // ALL CONTAMINATION RESTORED";
        }
    }

    public sealed class UfoWashCustomerShip : MonoBehaviour
    {
        enum DockState { Arriving, Docked, Departing, Turnaround }
        [SerializeField] float travelDistance = 10.5f;
        [SerializeField] float travelSpeed = 6f;
        [SerializeField] float turnaroundSeconds = 1.6f;

        Vector3 dockPosition;
        Vector3 approachPosition;
        float turnaroundTimer;
        DockState state;

        public bool IsDocked { get; private set; }

        void Awake()
        {
            dockPosition = transform.position;
            approachPosition = dockPosition + Vector3.forward * travelDistance;
            transform.position = approachPosition;
            IsDocked = false;
            state = DockState.Arriving;
        }

        void Update()
        {
            if (state == DockState.Turnaround)
            {
                turnaroundTimer -= Time.deltaTime;
                if (turnaroundTimer <= 0f) state = DockState.Arriving;
                return;
            }

            Vector3 target = state == DockState.Departing ? approachPosition : dockPosition;
            transform.position = Vector3.MoveTowards(transform.position, target, travelSpeed * Time.deltaTime);

            if ((transform.position - target).sqrMagnitude > 0.0025f) return;

            if (state == DockState.Arriving)
            {
                IsDocked = true;
                state = DockState.Docked;
            }
            else if (state == DockState.Departing)
            {
                IsDocked = false;
                state = DockState.Turnaround;
                turnaroundTimer = turnaroundSeconds;
            }
        }

        public void DepartAndReturn()
        {
            if (!IsDocked || state != DockState.Docked) return;
            IsDocked = false;
            state = DockState.Departing;
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
            if (!loop.CanAcceptJob) return "CUSTOMER VESSEL INBOUND // WAIT FOR DOCKING";
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
