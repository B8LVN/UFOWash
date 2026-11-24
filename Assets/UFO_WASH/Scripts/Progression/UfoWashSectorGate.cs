using UnityEngine;

namespace UFOWASH
{
    [DisallowMultipleComponent]
    public sealed class UfoWashSectorGate : UfoWashInteractable
    {
        [SerializeField] private string sectorKey = "crater-ridge";
        [SerializeField] private string sectorName = "CRATER RIDGE";
        [SerializeField] private int purchaseCost = 2500;
        [SerializeField] private float requiredReputation = 2.6f;
        [SerializeField] private GameObject barrierObject;
        [SerializeField] private GameObject sectorContent;

        private bool isUnlocked;
        private string PreferenceKey => "UFOWASH_SECTOR_" + sectorKey.ToUpperInvariant();

        private void Awake()
        {
            isUnlocked = PlayerPrefs.GetInt(PreferenceKey, 0) == 1;
            ApplyWorldState();
        }

        public void Configure(string key, string displayName, int cost, float reputation,
            GameObject barrier, GameObject content)
        {
            sectorKey = string.IsNullOrWhiteSpace(key) ? "sector" : key.Trim().ToLowerInvariant();
            sectorName = string.IsNullOrWhiteSpace(displayName) ? "FRONTIER SECTOR" : displayName;
            purchaseCost = Mathf.Max(0, cost);
            requiredReputation = Mathf.Clamp(reputation, 1f, 5f);
            barrierObject = barrier;
            sectorContent = content;
            isUnlocked = PlayerPrefs.GetInt(PreferenceKey, 0) == 1;
            ApplyWorldState();
        }

        public override string GetPrompt()
        {
            if (isUnlocked) return sectorName + " // SECTOR OPEN";
            var career = UfoWashCareerLoop.Instance;
            if (career != null && career.Reputation < requiredReputation)
                return "E  " + sectorName + " // REPUTATION " + FormatReputation(requiredReputation) + " REQUIRED";
            return "E  CLAIM " + sectorName + " // " + purchaseCost.ToString("N0") + " GC";
        }

        public override void Interact(GameObject player)
        {
            if (isUnlocked) return;
            var career = UfoWashCareerLoop.Instance;
            if (career == null) return;

            if (career.Reputation < requiredReputation)
            {
                career.NotifyBossMessage(sectorName + " ACCESS REQUIRES " + FormatReputation(requiredReputation) + " REPUTATION STARS.");
                return;
            }
            if (UfoWashHUD.Credits < purchaseCost)
            {
                career.NotifyBossMessage("LAND CLAIM DENIED // " + purchaseCost.ToString("N0") + " GC REQUIRED; CURRENT BALANCE " + UfoWashHUD.Credits.ToString("N0") + " GC.");
                return;
            }

            UfoWashHUD.Credits -= purchaseCost;
            isUnlocked = true;
            PlayerPrefs.SetInt(PreferenceKey, 1);
            PlayerPrefs.Save();
            ApplyWorldState();
            career.NotifyBossMessage("LAND CLAIM APPROVED // " + sectorName + " IS OPEN. NEW PLOTS AND CONTRACTS ARE NOW AVAILABLE.");
        }

        private void ApplyWorldState()
        {
            if (sectorContent != null) sectorContent.SetActive(isUnlocked);
            if (barrierObject != null) barrierObject.SetActive(!isUnlocked);
        }

        private static string FormatReputation(float value) =>
            value.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
    }
}
