using UnityEngine;

namespace UFOWASH
{
    public class UfoWashUpgradeStation : UfoWashInteractable
    {
        [SerializeField] private int baseCost = 200;
        [SerializeField] private float cleaningPowerIncrease = 0.25f;
        [SerializeField] private float sprayCoverageIncrease = 0.06f;
        [SerializeField] private float rangeIncrease = 0.5f;
        [SerializeField] private int maxLevel = 5;

        private int level;
        private UfoWashSprayTool sprayTool;

        private void Start()
        {
            sprayTool = FindAnyObjectByType<UfoWashSprayTool>();
        }

        public override string GetPrompt()
        {
            if (level >= maxLevel) return "E  EQUIPMENT MAXED";
            return $"E  UPGRADE ION WASHER  |  {GetCost()} CR  |  LVL {level}/{maxLevel}  |  +POWER +COVERAGE";
        }

        public override void Interact(GameObject player)
        {
            if (level >= maxLevel || sprayTool == null) return;

            int cost = GetCost();
            if (UfoWashHUD.Credits < cost) return;

            UfoWashHUD.Credits -= cost;
            level++;
            sprayTool.cleanPerSecond += cleaningPowerIncrease;
            sprayTool.sprayRadius += sprayCoverageIncrease;
            sprayTool.range += rangeIncrease;
            Debug.Log($"[UFO WASH] Ion washer level {level}: power {sprayTool.cleanPerSecond:0.00}/s, coverage radius {sprayTool.sprayRadius:0.00}, range {sprayTool.range:0.0}m.");
        }

        private int GetCost() => baseCost * (level + 1);
    }
}
