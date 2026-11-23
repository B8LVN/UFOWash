using UnityEngine;

namespace UFOWASH
{
    public class UfoWashUpgradeStation : UfoWashInteractable
    {
        [SerializeField] private int baseCost = 300;
        [SerializeField] private float cleaningPowerIncrease = 0.25f;
        [SerializeField] private int maxLevel = 5;

        private int level;
        private UfoWashSprayTool sprayTool;

        private void Start()
        {
            sprayTool = FindFirstObjectByType<UfoWashSprayTool>();
        }

        public override string GetPrompt()
        {
            if (level >= maxLevel) return "E  EQUIPMENT MAXED";
            return $"E  UPGRADE SPRAYER  |  {GetCost()} CREDITS  |  LVL {level}/{maxLevel}";
        }

        public override void Interact(GameObject player)
        {
            if (level >= maxLevel || sprayTool == null) return;

            int cost = GetCost();
            if (UfoWashHUD.Credits < cost) return;

            UfoWashHUD.Credits -= cost;
            level++;
            sprayTool.cleanPerSecond += cleaningPowerIncrease;
            Debug.Log($"[UFO WASH] Sprayer upgraded to level {level}. Cleaning rate: {sprayTool.cleanPerSecond:0.00}/s");
        }

        private int GetCost() => baseCost * (level + 1);
    }
}
