using UnityEngine;

namespace UFOWASH
{
    [DisallowMultipleComponent]
    public sealed class UfoWashBayRecoveryStation : UfoWashInteractable
    {
        const string UpgradeKey = "bay-recovery-loop";
        const int FirstUpgradeCost = 650;
        const int SecondUpgradeCost = 1400;

        [SerializeField] GameObject firstModule;
        [SerializeField] GameObject secondModule;
        UfoWashCareerLoop career;
        int level;

        public string SaveKey => UpgradeKey;
        public int Level => level;

        void Start()
        {
            career = FindAnyObjectByType<UfoWashCareerLoop>();
            var save = UfoWashProgressSave.Instance;
            SetLevelFromSave(save != null ? save.GetUpgradeLevel(UpgradeKey) : 0);
        }

        public void Configure(GameObject firstUpgrade, GameObject secondUpgrade)
        {
            firstModule = firstUpgrade;
            secondModule = secondUpgrade;
            ApplyLevel();
        }

        public override string GetPrompt()
        {
            if (level >= 2) return "RECOVERY MAX // SUPPLY USE -30%";
            if (career == null) return "BAY RECOVERY SYSTEMS // OFFLINE";
            return level == 0
                ? "E  INSTALL RECOVERY // 650 GC // SUPPLY USE -15%"
                : "E  UPGRADE RECOVERY // 1400 GC // SUPPLY USE -30%";
        }

        public override void Interact(GameObject player)
        {
            if (level >= 2) return;
            if (career == null) career = UfoWashCareerLoop.Instance;
            if (career == null) return;

            int cost = level == 0 ? FirstUpgradeCost : SecondUpgradeCost;
            if (UfoWashHUD.Credits < cost)
            {
                career.NotifyBossMessage("BAY UPGRADE NEEDS " + cost.ToString("N0") +
                    " GC // CURRENT BALANCE " + UfoWashHUD.Credits.ToString("N0") + " GC");
                return;
            }

            UfoWashHUD.Credits -= cost;
            level++;
            ApplyLevel();

            if (UfoWashProgressSave.Instance != null)
                UfoWashProgressSave.Instance.RecordUpgrade(UpgradeKey, level);

            int efficiency = level == 1 ? 15 : 30;
            career.NotifyBossMessage("BAY RECOVERY INSTALLED // -" + cost.ToString("N0") +
                " GC // SUPPLY USE -" + efficiency + "% // ANNEX EXPANDED");
        }

        public void SetLevelFromSave(int savedLevel)
        {
            level = Mathf.Clamp(savedLevel, 0, 2);
            ApplyLevel();
        }

        void ApplyLevel()
        {
            if (firstModule != null) firstModule.SetActive(level == 1);
            if (secondModule != null) secondModule.SetActive(level >= 2);
            if (career == null) career = UfoWashCareerLoop.Instance;
            if (career != null) career.SetBayRecoveryTier(level);
        }
    }
}
