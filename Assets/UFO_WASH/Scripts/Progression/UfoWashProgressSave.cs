using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UFOWASH
{
    [DefaultExecutionOrder(-1000)]
    [DisallowMultipleComponent]
    public sealed class UfoWashProgressSave : MonoBehaviour
    {
        const int CurrentSchemaVersion = 1;
        const float AutosaveInterval = 2f;
        const float SnapshotInterval = 0.5f;
        static readonly string[] LegacySectorKeys = { "crater-ridge", "ion-flats", "glasswake-reach" };

        [Serializable]
        sealed class UpgradeRecord
        {
            public string key;
            public int level;
        }

        [Serializable]
        sealed class SaveData
        {
            public int schemaVersion;
            public int credits;
            public float reputation;
            public float water;
            public float chemical;
            public int completedJobs;
            public bool attendantHired;
            public UpgradeRecord[] upgrades = Array.Empty<UpgradeRecord>();
            public string[] unlockedSectors = Array.Empty<string>();
        }

        public static UfoWashProgressSave Instance { get; private set; }
        public string Status { get; private set; } = "LEDGER // STARTING";
        public bool SaveBlocked => saveBlocked;

        string savePath;
        SaveData loadedData;
        string lastSnapshot;
        UfoWashUpgradeStation[] upgradeStations = Array.Empty<UfoWashUpgradeStation>();
        UfoWashBayRecoveryStation[] bayRecoveryStations = Array.Empty<UfoWashBayRecoveryStation>();
        UfoWashWashAttendant[] attendants = Array.Empty<UfoWashWashAttendant>();
        UfoWashSectorGate[] sectorGates = Array.Empty<UfoWashSectorGate>();
        bool hasLoadedData;
        bool saveBlocked;
        bool recoveredFromBackup;
        float snapshotTimer;
        float autosaveTimer;

        string BackupPath => savePath + ".bak";

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            savePath = Path.Combine(Application.persistentDataPath, "UfoWash", "company-save.json");
            LoadFromDisk();
        }

        void Start()
        {
            RefreshWorldReferences();
            RestoreSectorState();
            if (hasLoadedData)
                Status = recoveredFromBackup ? "LEDGER // RECOVERED FROM BACKUP" : "LEDGER // SAVED PROGRESS LOADED";
            else if (saveBlocked)
                Status = "LEDGER ERROR // SAVE PRESERVED";
            else
                Status = "LEDGER // NEW COMPANY";
        }

        void Update()
        {
            if (saveBlocked || UfoWashCareerLoop.Instance == null) return;

            snapshotTimer += Time.unscaledDeltaTime;
            if (snapshotTimer < SnapshotInterval) return;
            snapshotTimer = 0f;

            string current = JsonUtility.ToJson(BuildCurrentData());
            if (lastSnapshot == null)
            {
                lastSnapshot = current;
                return;
            }

            if (current == lastSnapshot) return;
            autosaveTimer += SnapshotInterval;
            if (autosaveTimer < AutosaveInterval) return;
            WriteSnapshot(current);
        }

        public void RestoreCareer(UfoWashCareerLoop career)
        {
            if (career == null) return;
            if (hasLoadedData)
            {
                career.RestoreProgress(loadedData.credits, loadedData.reputation,
                    loadedData.water, loadedData.chemical, loadedData.completedJobs);
                career.NotifyBossMessage("COMPANY LEDGER RESTORED // ACTIVE CONTRACTS RESET TO DISPATCH.");
            }
            else if (saveBlocked)
            {
                career.NotifyBossMessage("SAVE FILE DAMAGED // ORIGINAL FILE PRESERVED; SAVING DISABLED.");
            }
        }

        public int GetUpgradeLevel(string key)
        {
            if (loadedData == null || loadedData.upgrades == null || string.IsNullOrEmpty(key)) return 0;
            for (int i = 0; i < loadedData.upgrades.Length; i++)
                if (loadedData.upgrades[i] != null && loadedData.upgrades[i].key == key)
                    return Mathf.Max(0, loadedData.upgrades[i].level);
            return 0;
        }

        public bool GetAttendantHired() => loadedData != null && loadedData.attendantHired;

        public bool IsSectorUnlocked(string key)
        {
            if (loadedData == null || loadedData.unlockedSectors == null || string.IsNullOrEmpty(key)) return false;
            for (int i = 0; i < loadedData.unlockedSectors.Length; i++)
                if (string.Equals(loadedData.unlockedSectors[i], key, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        public void SaveNow()
        {
            if (saveBlocked) return;
            WriteSnapshot(JsonUtility.ToJson(BuildCurrentData()));
        }

        public void MarkAttendantHired()
        {
            if (loadedData == null) loadedData = CreateNewData();
            loadedData.attendantHired = true;
            hasLoadedData = true;
            SaveNow();
        }

        public void MarkSectorUnlocked(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            if (loadedData == null) loadedData = CreateNewData();
            AddSector(loadedData, key);
            hasLoadedData = true;
            SaveNow();
        }

        public void RecordUpgrade(string key, int level)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            if (loadedData == null) loadedData = CreateNewData();
            SetUpgrade(loadedData, key, level);
            hasLoadedData = true;
            SaveNow();
        }

        void LoadFromDisk()
        {
            try
            {
                if (!File.Exists(savePath) && !File.Exists(BackupPath))
                {
                    loadedData = CreateNewData();
                    return;
                }

                if (TryReadSave(savePath, out loadedData))
                {
                    hasLoadedData = true;
                    return;
                }

                if (TryReadSave(BackupPath, out loadedData))
                {
                    hasLoadedData = true;
                    recoveredFromBackup = true;
                    Debug.LogWarning("UFO WASH: the main company save could not be read. The last valid backup was loaded; the damaged file is preserved.");
                    return;
                }

                saveBlocked = true;
                Debug.LogError("UFO WASH: company save and backup are missing or invalid. Saving is disabled to preserve the files; the current session will use a new in-memory company.");
                loadedData = CreateNewData();
            }
            catch (Exception exception)
            {
                saveBlocked = true;
                loadedData = CreateNewData();
                Debug.LogError("UFO WASH: unable to read company progress. Saving is disabled and existing files are preserved. " + exception.Message);
            }
        }

        bool TryReadSave(string path, out SaveData data)
        {
            data = null;
            if (!File.Exists(path)) return false;
            try
            {
                var info = new FileInfo(path);
                if (info.Length <= 0 || info.Length > 1024 * 1024) return false;
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
                return IsValid(data);
            }
            catch (Exception exception)
            {
                Debug.LogWarning("UFO WASH: could not read save candidate " + Path.GetFileName(path) + ". " + exception.Message);
                data = null;
                return false;
            }
        }

        static bool IsValid(SaveData data)
        {
            if (data == null ||
                data.schemaVersion != CurrentSchemaVersion ||
                data.credits < 0 || data.credits > 100000000 ||
                !IsFinite(data.reputation) || data.reputation < 1f || data.reputation > 5f ||
                !IsFinite(data.water) || data.water < 0f || data.water > 100f ||
                !IsFinite(data.chemical) || data.chemical < 0f || data.chemical > 100f ||
                data.completedJobs < 0 ||
                (data.upgrades != null && data.upgrades.Length > 128) ||
                (data.unlockedSectors != null && data.unlockedSectors.Length > 64))
                return false;

            if (data.upgrades != null)
                for (int i = 0; i < data.upgrades.Length; i++)
                    if (data.upgrades[i] == null || string.IsNullOrWhiteSpace(data.upgrades[i].key) ||
                        data.upgrades[i].key.Length > 64 || data.upgrades[i].level < 0 ||
                        data.upgrades[i].level > 20)
                        return false;

            if (data.unlockedSectors != null)
                for (int i = 0; i < data.unlockedSectors.Length; i++)
                    if (string.IsNullOrWhiteSpace(data.unlockedSectors[i]) ||
                        data.unlockedSectors[i].Length > 64)
                        return false;

            return true;
        }

        void RestoreSectorState()
        {
            if (hasLoadedData)
            {
                // Keep land claims made by earlier builds before the company ledger existed.
                for (int i = 0; i < LegacySectorKeys.Length; i++)
                {
                    string preferenceKey = "UFOWASH_SECTOR_" + LegacySectorKeys[i].ToUpperInvariant();
                    if (PlayerPrefs.GetInt(preferenceKey, 0) == 1)
                        AddSector(loadedData, LegacySectorKeys[i]);
                }
            }

            for (int i = 0; i < sectorGates.Length; i++)
            {
                var gate = sectorGates[i];
                if (gate == null) continue;
                bool unlocked = hasLoadedData
                    ? IsSectorUnlocked(gate.SectorKey)
                    : gate.IsUnlocked;
                gate.SetUnlockedFromSave(unlocked);
            }
        }

        SaveData BuildCurrentData()
        {
            SaveData data = Clone(loadedData) ?? CreateNewData();
            data.schemaVersion = CurrentSchemaVersion;

            var career = UfoWashCareerLoop.Instance;
            if (career != null)
            {
                data.credits = Mathf.Max(0, UfoWashHUD.Credits);
                data.reputation = career.Reputation;
                data.water = career.Water;
                data.chemical = career.Chemical;
                data.completedJobs = career.CompletedJobs;
            }

            for (int i = 0; i < upgradeStations.Length; i++)
                if (upgradeStations[i] != null)
                    SetUpgrade(data, upgradeStations[i].SaveKey, upgradeStations[i].Level);

            for (int i = 0; i < bayRecoveryStations.Length; i++)
                if (bayRecoveryStations[i] != null)
                    SetUpgrade(data, bayRecoveryStations[i].SaveKey, bayRecoveryStations[i].Level);

            if (attendants.Length > 0 && attendants[0] != null)
                data.attendantHired = attendants[0].IsHired;

            for (int i = 0; i < sectorGates.Length; i++)
            {
                var gate = sectorGates[i];
                if (gate == null) continue;
                if (gate.IsUnlocked) AddSector(data, gate.SectorKey);
                else RemoveSector(data, gate.SectorKey);
            }

            return data;
        }

        void RefreshWorldReferences()
        {
            upgradeStations = FindObjectsByType<UfoWashUpgradeStation>(FindObjectsSortMode.None);
            bayRecoveryStations = FindObjectsByType<UfoWashBayRecoveryStation>(FindObjectsSortMode.None);
            attendants = FindObjectsByType<UfoWashWashAttendant>(FindObjectsSortMode.None);
            sectorGates = FindObjectsByType<UfoWashSectorGate>(FindObjectsSortMode.None);
        }

        void WriteSnapshot(string json)
        {
            if (saveBlocked) return;
            string temporaryPath = savePath + ".tmp";
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                using (var stream = new FileStream(temporaryPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                    writer.Flush();
                    stream.Flush(true);
                }

                if (File.Exists(savePath))
                {
                    if (recoveredFromBackup)
                    {
                        File.Copy(savePath, savePath + ".corrupt", true);
                        File.Copy(BackupPath, savePath, true);
                        recoveredFromBackup = false;
                    }
                    else
                    {
                        File.Copy(savePath, BackupPath, true);
                    }
                    File.Delete(savePath);
                }

                File.Move(temporaryPath, savePath);
                loadedData = JsonUtility.FromJson<SaveData>(json);
                hasLoadedData = true;
                lastSnapshot = json;
                autosaveTimer = 0f;
                Status = "LEDGER // SAVED";
            }
            catch (Exception exception)
            {
                try { if (File.Exists(temporaryPath)) File.Delete(temporaryPath); } catch { }
                Status = "LEDGER ERROR // SAVE FAILED";
                Debug.LogError("UFO WASH: company progress was not saved. The previous save/backup was retained where possible. " + exception.Message);
            }
        }

        static SaveData CreateNewData()
        {
            return new SaveData
            {
                schemaVersion = CurrentSchemaVersion,
                credits = 750,
                reputation = 2.1f,
                water = 100f,
                chemical = 100f,
                completedJobs = 0,
                attendantHired = false,
                upgrades = Array.Empty<UpgradeRecord>(),
                unlockedSectors = Array.Empty<string>()
            };
        }

        static SaveData Clone(SaveData source)
        {
            if (source == null) return null;
            var copy = new SaveData
            {
                schemaVersion = source.schemaVersion,
                credits = source.credits,
                reputation = source.reputation,
                water = source.water,
                chemical = source.chemical,
                completedJobs = source.completedJobs,
                attendantHired = source.attendantHired,
                unlockedSectors = source.unlockedSectors != null ? (string[])source.unlockedSectors.Clone() : Array.Empty<string>()
            };
            if (source.upgrades != null)
            {
                copy.upgrades = new UpgradeRecord[source.upgrades.Length];
                for (int i = 0; i < source.upgrades.Length; i++)
                    copy.upgrades[i] = source.upgrades[i] == null ? null : new UpgradeRecord
                    {
                        key = source.upgrades[i].key,
                        level = source.upgrades[i].level
                    };
            }
            return copy;
        }

        static void SetUpgrade(SaveData data, string key, int level)
        {
            var records = new List<UpgradeRecord>(data.upgrades ?? Array.Empty<UpgradeRecord>());
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i] == null || records[i].key != key) continue;
                records[i].level = Mathf.Clamp(level, 0, 20);
                data.upgrades = records.ToArray();
                return;
            }
            records.Add(new UpgradeRecord { key = key, level = Mathf.Clamp(level, 0, 20) });
            data.upgrades = records.ToArray();
        }

        static void AddSector(SaveData data, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            var sectors = new List<string>(data.unlockedSectors ?? Array.Empty<string>());
            for (int i = 0; i < sectors.Count; i++)
                if (string.Equals(sectors[i], key, StringComparison.OrdinalIgnoreCase)) return;
            sectors.Add(key);
            data.unlockedSectors = sectors.ToArray();
        }

        static void RemoveSector(SaveData data, string key)
        {
            if (data.unlockedSectors == null) return;
            var sectors = new List<string>(data.unlockedSectors);
            sectors.RemoveAll(value => string.Equals(value, key, StringComparison.OrdinalIgnoreCase));
            data.unlockedSectors = sectors.ToArray();
        }

        static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

        void OnApplicationPause(bool paused)
        {
            if (paused) SaveNow();
        }

        void OnGUI()
        {
            if (Status.IndexOf("ERROR", StringComparison.OrdinalIgnoreCase) < 0 &&
                Status.IndexOf("FAILED", StringComparison.OrdinalIgnoreCase) < 0 &&
                Status.IndexOf("RECOVERED", StringComparison.OrdinalIgnoreCase) < 0)
                return;

            var previousColor = GUI.color;
            GUI.color = Status.IndexOf("ERROR", StringComparison.OrdinalIgnoreCase) >= 0 ||
                Status.IndexOf("FAILED", StringComparison.OrdinalIgnoreCase) >= 0
                ? new Color(1f, 0.55f, 0.3f)
                : new Color(0.55f, 0.95f, 1f);
            GUI.Box(new Rect(Screen.width - 430f, 18f, 410f, 34f), Status);
            GUI.color = previousColor;
        }

        void OnApplicationQuit() => SaveNow();

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
