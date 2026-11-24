using UnityEngine;

namespace UFOWASH
{
    /// <summary>
    /// Data-driven economy configuration. Every GC, reputation, tip, and supply value
    /// lives here instead of being hard-coded in gameplay code (GDD Rule 8, AC-E4).
    /// Create one asset via Assets > Create > UFO WASH > Economy Config.
    /// </summary>
    [CreateAssetMenu(menuName = "UFO WASH/Economy Config", fileName = "UfoWashEconomyConfig")]
    public sealed class UfoWashEconomyConfig : ScriptableObject
    {
        [Header("Opening state (GDD 4.3)")]
        [Tooltip("Starting Galactic Credits for a new company.")]
        public int openingCredits = 750;
        [Tooltip("Starting reputation, clamped to 1-5.")]
        public float openingReputation = 2.1f;
        [Tooltip("Opening water percent (0-100).")]
        public float openingWater = 100f;
        [Tooltip("Opening chemical percent (0-100).")]
        public float openingChemical = 100f;

        [Header("Opening quotes (GDD 4.3)")]
        public int quoteLow = 250;
        public int quoteMid = 420;
        public int quoteHigh = 600;

        [Header("Supply refill (GDD 4.3)")]
        [Tooltip("Credits charged to refill water + chemical when depleted.")]
        public int refillCost = 65;

        [Header("Tip thresholds and rates (GDD 4.3)")]
        [Tooltip("Quality needed for the 8% tip tier.")]
        public float tipTier1Quality = 0.84f;
        [Tooltip("8% of base payout.")]
        public float tipTier1Rate = 0.08f;
        [Tooltip("Quality needed for the 16% tip tier.")]
        public float tipTier2Quality = 0.90f;
        [Tooltip("16% of base payout.")]
        public float tipTier2Rate = 0.16f;
        [Tooltip("Quality needed for the 35% VIP tip tier.")]
        public float tipTier3Quality = 0.985f;
        [Tooltip("35% of base payout.")]
        public float tipTier3Rate = 0.35f;

        [Header("Reputation changes per inspection (GDD 4.3)")]
        [Tooltip("Rep change at 84% quality.")]
        public float repAt84 = 0.08f;
        [Tooltip("Rep change at 92% quality.")]
        public float repAt92 = 0.16f;
        [Tooltip("Rep change at 98.5% quality.")]
        public float repAt985 = 0.25f;
        [Tooltip("Rep change below 84% quality.")]
        public float repBelow84 = -0.15f;
        [Tooltip("Minimum reputation (clamped).")]
        public float reputationMin = 1f;
        [Tooltip("Maximum reputation (clamped).")]
        public float reputationMax = 5f;

        [Header("Reputation unlocks (GDD 4.3, 4.4)")]
        [Tooltip("Heavy dispatch unlocks at this reputation.")]
        public float heavyDispatchRep = 2.1f;
        [Tooltip("VIP contracts unlock at this reputation.")]
        public float vipContractsRep = 3.8f;

        [Header("Stage limits (GDD 4.1)")]
        [Tooltip("Pre-rinse: every zone at most this contamination.")]
        public float preRinseLimit = 0.58f;
        [Tooltip("Foam: every zone at least this coverage.")]
        public float foamCoverageMin = 0.62f;
        [Tooltip("Final rinse: every zone at most this contamination.")]
        public float finalRinseDirtMax = 0.15f;
        [Tooltip("Final rinse: every zone at most this foam.")]
        public float finalRinseFoamMax = 0.10f;

        [Header("Quality formula (GDD 4.1)")]
        [Tooltip("Residual foam penalty coefficient in quality = 1 - dirt - coef * foam.")]
        public float foamQualityCoefficient = 0.18f;

        [Header("Automated bay quality cap (GDD G2)")]
        [Tooltip("Fully automated bays cannot exceed this quality (0-1). Owner's Touch exceeds it.")]
        [Range(0f, 1f)]
        public float automatedBayQualityCap = 0.92f;

        [Header("Wash timing guardrails (GDD G3)")]
        [Tooltip("Tier-0 Zorgon Scout hand-wash time, seconds (6-8 min).")]
        public float tier0HandWashMinutesMin = 6f;
        [Tooltip("Tier-0 Zorgon Scout hand-wash time, seconds (6-8 min).")]
        public float tier0HandWashMinutesMax = 8f;
        [Tooltip("Minimum wash time for any tool tier, seconds (2 min).")]
        public float minimumWashMinutes = 2f;

        [Header("Quality grades")]
        public float gradeS = 0.985f;
        public float gradeA = 0.95f;
        public float gradeB = 0.90f;
        public float gradeC = 0.85f;

        /// <summary>Tip rate for a given quality, or 0 if below all thresholds.</summary>
        public float TipRateForQuality(float quality)
        {
            if (quality >= tipTier3Quality) return tipTier3Rate;
            if (quality >= tipTier2Quality) return tipTier2Rate;
            if (quality >= tipTier1Quality) return tipTier1Rate;
            return 0f;
        }

        /// <summary>Reputation delta for a given quality (GDD 4.3 table).</summary>
        public float ReputationDeltaForQuality(float quality)
        {
            if (quality >= tipTier3Quality) return repAt985;
            if (quality >= 0.92f) return repAt92;
            if (quality >= tipTier1Quality) return repAt84;
            return repBelow84;
        }

        /// <summary>Quality grade letter for a given quality (0-1).</summary>
        public string GradeForQuality(float quality)
        {
            if (quality >= gradeS) return "S";
            if (quality >= gradeA) return "A";
            if (quality >= gradeB) return "B";
            if (quality >= gradeC) return "C";
            return "D";
        }

        /// <summary>Opening quote values in order: low, mid, high.</summary>
        public int[] OpeningQuotes => new[] { quoteLow, quoteMid, quoteHigh };
    }
}