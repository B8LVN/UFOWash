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
        public string Client => Clients[CompletedJobs % Clients.Length];
        public string Ship => Ships[CompletedJobs % Ships.Length];
        public string ContaminationType => Contamination[CompletedJobs % Contamination.Length];
        public int Reward => 250 + (CompletedJobs % 3) * 100;

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
            UfoWashHUD.JobComplete = false;
            UfoWashDirtSurface.ResetJob();
            LastMessage = "CONTRACT ACCEPTED // BEGIN DECONTAMINATION";
        }

        public void CompleteJob()
        {
            if (!JobAccepted) return;
            JobAccepted = false;
            CompletedJobs++;
            UfoWashHUD.Credits += Reward;
            UfoWashHUD.JobComplete = true;
            LastMessage = "VESSEL CERTIFIED // PAYMENT TRANSFERRED";
        }

        public void ResetActiveJob()
        {
            if (!JobAccepted) return;
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
            if (loop.JobAccepted) return "CONTRACT ACTIVE // CLEAN THE UFO";
            return "E  ACCEPT CONTRACT  //  " + loop.Reward + " CR";
        }

        public override void Interact(GameObject player)
        {
            var loop = UfoWashCareerLoop.Instance;
            if (loop != null && !loop.JobAccepted) loop.AcceptJob();
        }
    }
}