using System;
using System.Collections.Generic;
using UnityEngine;

namespace UFOWASH
{
    /// <summary>
    /// Forensic Inspection Report (GDD Section 4.2, hook 2, Rule A).
    /// Generated after every completed job. Every grade penalty and missed tip
    /// tier is traceable to a listed cause. All numbers originate from the
    /// coating grid or receipt — nothing is invented here (AC-F3).
    /// </summary>
    [Serializable]
    public sealed class ForensicReport
    {
        public string jobId;
        public string clientName;
        public string shipClass;
        public string contaminationType;
        public string grade;
        public float qualityPercent;
        public int basePayout;
        public int qualityBonus;
        public int tip;
        public int rushBonus;
        public int gross;
        public int consumablesCost;
        public int net;
        public float reputationBefore;
        public float reputationAfter;
        public float reputationDelta;

        public bool preRinsePass;
        public bool foamPass;
        public bool finalRinsePass;

        public float avgContamination;
        public float avgFoam;

        /// <summary>Ranked causes (up to 5), each with a one-line suggestion.</summary>
        public List<CauseEntry> causes = new List<CauseEntry>();

        /// <summary>Finds revealed during this job.</summary>
        public List<string> findsRevealed = new List<string>();

        public float waterUsedPercent;
        public float chemicalUsedPercent;
        public float timeSeconds;
    }

    [Serializable]
    public sealed class CauseEntry
    {
        public string zoneName;
        public string description;
        public string suggestion;
        public float severity;
    }
}