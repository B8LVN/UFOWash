using UnityEngine;

namespace UFOWASH
{
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
