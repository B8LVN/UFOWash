using UnityEngine;

namespace UFOWASH
{
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
            else if (!loop.JobAccepted && loop.CanAcceptJob) UfoWashContractTerminalUI.Open(loop);
            else if (!loop.JobAccepted) loop.AcceptJob();
        }
    }
}
