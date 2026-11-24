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
        [SerializeField] private string saveKey = "ion-washer";

        private int level;
        private UfoWashSprayTool sprayTool;

        public string SaveKey => saveKey;
        public int Level => level;

        private void Start()
        {
            sprayTool = FindAnyObjectByType<UfoWashSprayTool>();
            if (UfoWashProgressSave.Instance != null)
                SetLevelFromSave(UfoWashProgressSave.Instance.GetUpgradeLevel(saveKey));
        }

        public override string GetPrompt()
        {
            if (level >= maxLevel) return "E  EQUIPMENT MAXED";
            if (level == 0) return $"E  FIT FAN NOZZLE  |  {GetCost()} CR  |  WIDER SWEEPS + Q TO SWITCH";
            return $"E  UPGRADE ION WASHER  |  {GetCost()} CR  |  LVL {level}/{maxLevel}  |  +POWER +COVERAGE";
        }

        public override void Interact(GameObject player)
        {
            if (level >= maxLevel || sprayTool == null) return;

            int cost = GetCost();
            if (UfoWashHUD.Credits < cost)
            {
                UfoWashCareerLoop.Instance?.NotifyBossMessage("INSUFFICIENT CREDITS // NOZZLE COST " + cost + " CR");
                return;
            }

            UfoWashHUD.Credits -= cost;
            level++;
            if (level == 1) sprayTool.InstallFanNozzle();
            sprayTool.cleanPerSecond += cleaningPowerIncrease;
            sprayTool.sprayRadius += sprayCoverageIncrease;
            sprayTool.range += rangeIncrease;
            if (UfoWashProgressSave.Instance != null)
                UfoWashProgressSave.Instance.RecordUpgrade(saveKey, level);
            UfoWashCareerLoop.Instance?.NotifyBossMessage(
                "NOZZLE FITTED // -" + cost + " CR // LEVEL " + level + " // Q TO SWITCH FAN / PRECISION");
            Debug.Log($"[UFO WASH] Ion washer level {level}: power {sprayTool.cleanPerSecond:0.00}/s, coverage radius {sprayTool.sprayRadius:0.00}, range {sprayTool.range:0.0}m.");
        }

        public void SetLevelFromSave(int savedLevel)
        {
            level = Mathf.Clamp(savedLevel, 0, maxLevel);
            if (sprayTool == null) return;
            if (level > 0) sprayTool.InstallFanNozzle();
            sprayTool.cleanPerSecond += cleaningPowerIncrease * level;
            sprayTool.sprayRadius += sprayCoverageIncrease * level;
            sprayTool.range += rangeIncrease * level;
        }

        private int GetCost() => baseCost * (level + 1);
    }
}
