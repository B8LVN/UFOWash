using UnityEngine;

namespace UFOWASH
{
    [DisallowMultipleComponent]
    public sealed class UfoWashWashAttendant : UfoWashInteractable
    {
        [SerializeField] int hiringCost = 120;
        [SerializeField, Range(0.1f, 1f)] float workRate = 0.6f;
        [SerializeField] Transform nozzle;

        const float PreRinseLimit = 0.58f;

        UfoWashCareerLoop career;
        TextMesh statusText;
        LineRenderer workBeam;
        Material beamMaterial;
        bool hired;
        float idleBobPhase;
        Vector3 startPosition;
        string status = "AVAILABLE // HIRE WASH TECH";

        public bool IsHired => hired;

        void Awake()
        {
            career = UfoWashCareerLoop.Instance;
            startPosition = transform.position;
            idleBobPhase = Random.value * Mathf.PI * 2f;
            CreateStatusDisplay();
            CreateWorkBeam();
        }

        void Start()
        {
            var save = UfoWashProgressSave.Instance;
            if (save != null && save.GetAttendantHired())
                hired = true;
            UpdateStatus();
        }

        void Update()
        {
            if (career == null) career = UfoWashCareerLoop.Instance;
            idleBobPhase += Time.deltaTime * 1.6f;
            transform.position = startPosition + Vector3.up * (Mathf.Sin(idleBobPhase) * 0.025f);

            if (!hired)
            {
                SetStatus("AVAILABLE // HIRE FOR " + hiringCost + " GC", false);
                return;
            }

            if (career == null)
            {
                SetStatus("BLOCKED // CAREER SYSTEM OFFLINE", false);
                return;
            }

            if (!career.JobAccepted)
            {
                SetStatus("IDLE // WAITING FOR A CONTRACT", false);
                return;
            }

            if (career.CurrentStage != UfoWashToolMode.PreRinse)
            {
                SetStatus(career.CurrentStage == UfoWashToolMode.Foam
                    ? "READY // PLAYER FOAM COVERAGE REQUIRED"
                    : "IDLE // PLAYER FINISHING RINSE", false);
                return;
            }

            if (career.Water <= 0.01f)
            {
                SetStatus("BLOCKED // WATER EMPTY // REFILL SUPPLIES", false);
                return;
            }

            UfoWashDirtSurface target = FindNextPreRinseZone(career.ContractSurfaces);
            if (target == null)
            {
                SetStatus("PRE-RINSE COMPLETE // PLAYER NEEDED", false);
                return;
            }

            if (nozzle != null)
            {
                Vector3 aimPoint = target.transform.position;
                Vector3 direction = aimPoint - nozzle.position;
                if (direction.sqrMagnitude > 0.01f)
                    nozzle.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
                ShowBeam(nozzle.position, aimPoint);
            }

            SetStatus("WORKING // PRE-RINSE", true);
            career.ApplyTool(target, UfoWashToolMode.PreRinse,
                Time.deltaTime * workRate, target.transform.position, target.transform.up, 0.35f);
        }

        public override string GetPrompt()
        {
            if (hired) return "WASH TECH // " + status;
            return "E  HIRE VESPER WASH TECH // " + hiringCost + " GC";
        }

        public override void Interact(GameObject player)
        {
            if (hired)
            {
                if (career != null) career.NotifyBossMessage("WASH TECH // " + status);
                return;
            }

            if (UfoWashHUD.Credits < hiringCost)
            {
                if (career != null)
                    career.NotifyBossMessage("HIRE DENIED // " + hiringCost + " GC REQUIRED; CURRENT BALANCE " +
                        UfoWashHUD.Credits + " GC.");
                return;
            }

            UfoWashHUD.Credits -= hiringCost;
            hired = true;
            var save = UfoWashProgressSave.Instance;
            if (save != null) save.MarkAttendantHired();
            if (career != null)
                career.NotifyBossMessage("WASH TECH HIRED // AUTO PRE-RINSE IS ACTIVE ON CONTRACTED HULL ZONES.");
        }

        UfoWashDirtSurface FindNextPreRinseZone(UfoWashDirtSurface[] surfaces)
        {
            if (surfaces == null) return null;
            for (int i = 0; i < surfaces.Length; i++)
            {
                var surface = surfaces[i];
                if (surface != null && surface.dirt > PreRinseLimit &&
                    career.IsContractSurface(surface))
                    return surface;
            }
            return null;
        }

        void CreateStatusDisplay()
        {
            var label = new GameObject("ATTENDANT STATUS");
            label.transform.SetParent(transform, false);
            label.transform.localPosition = new Vector3(0f, 2.25f, 0f);
            statusText = label.AddComponent<TextMesh>();
            statusText.anchor = TextAnchor.MiddleCenter;
            statusText.alignment = TextAlignment.Center;
            statusText.characterSize = 0.0035f;
            statusText.fontSize = 48;
            statusText.color = new Color(0.35f, 0.94f, 1f);
            statusText.text = status;
        }

        void CreateWorkBeam()
        {
            var beamObject = new GameObject("ATTENDANT // WORK STATUS BEAM");
            beamObject.transform.SetParent(transform, false);
            workBeam = beamObject.AddComponent<LineRenderer>();
            workBeam.positionCount = 2;
            workBeam.useWorldSpace = true;
            workBeam.startWidth = 0.035f;
            workBeam.endWidth = 0.008f;
            workBeam.enabled = false;
            workBeam.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            if (shader != null)
            {
                beamMaterial = new Material(shader) { name = "UFO WASH // Attendant Beam" };
                workBeam.material = beamMaterial;
            }
            workBeam.startColor = new Color(0.24f, 0.9f, 1f, 0.9f);
            workBeam.endColor = new Color(0.7f, 0.98f, 1f, 0.35f);
        }

        void ShowBeam(Vector3 from, Vector3 to)
        {
            if (workBeam == null) return;
            workBeam.SetPosition(0, from);
            workBeam.SetPosition(1, to);
            workBeam.enabled = true;
            workBeam.widthMultiplier = 1f + Mathf.Sin(Time.time * 8f) * 0.12f;
        }

        void SetStatus(string value, bool working)
        {
            status = value;
            if (statusText != null)
            {
                statusText.text = value;
                statusText.color = !hired
                    ? new Color(1f, 0.72f, 0.28f)
                    : working ? new Color(0.35f, 1f, 0.68f) : new Color(0.95f, 0.7f, 0.35f);
                Camera camera = Camera.main;
                if (camera != null)
                    statusText.transform.rotation = Quaternion.LookRotation(
                        statusText.transform.position - camera.transform.position);
            }
            if (!working && workBeam != null) workBeam.enabled = false;
        }

        void UpdateStatus()
        {
            SetStatus(hired ? "IDLE // WAITING FOR A CONTRACT" : "AVAILABLE // HIRE WASH TECH", false);
        }

        void OnDestroy()
        {
            if (beamMaterial != null) Destroy(beamMaterial);
        }
    }
}
