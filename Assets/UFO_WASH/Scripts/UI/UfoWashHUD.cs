using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UFOWASH
{
    public class UfoWashHUD : MonoBehaviour
    {
        public static int Credits;
        public static bool JobComplete;
        static Sprite roundedPanelSprite;
        static Texture2D roundedPanelTexture;

        Text creditsText, contractLabelText, clientText, shipText, contaminationText, stateText, instructionText, progressText, stageText, supplyText, zonesText, rushText;
        Image progressFill;
        Image zonePanel;
        Image waterFill, foamFill;
        Image[] toolModeCards = System.Array.Empty<Image>();
        Text[] toolModeLabels = System.Array.Empty<Text>();
        UfoWashCareerLoop career;
        UfoWashSprayTool tool;
        Text toolText;
        bool zoneChecklistVisible = true;

        void Start()
        {
            career = UfoWashCareerLoop.Instance;
            tool = FindAnyObjectByType<UfoWashSprayTool>();
            BuildInterface();
        }

        void Update()
        {
            if (zonePanel != null && Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
            {
                zoneChecklistVisible = !zoneChecklistVisible;
                zonePanel.gameObject.SetActive(zoneChecklistVisible);
            }
            if (career == null) career = UfoWashCareerLoop.Instance;
            RefreshInterface();
        }

        void BuildInterface()
        {
            EnsureRoundedPanelSprite();
            var canvasObject = new GameObject("UFO WASH // DIEGETIC HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = .5f;

            var panel = MakePanel(canvasObject.transform, "Mission Panel", new Color(.018f,.035f,.065f,.88f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(28,-28), new Vector2(470,350));
            AddPanelChrome(panel, new Color(.18f,.86f,1f,.95f));
            AddText(panel.transform, "Brand", "U F O   /   W A S H", 23, new Color(.24f,.88f,1f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-18), new Vector2(420,34), TextAnchor.MiddleLeft);
            AddText(panel.transform, "Subbrand", "INTERGALACTIC VEHICLE CARE  //  BAY 01", 11, new Color(.48f,.66f,.78f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-52), new Vector2(420,22), TextAnchor.MiddleLeft);

            creditsText = AddText(panel.transform, "Credits", "CREDITS  //  0000", 17, new Color(1f,.72f,.28f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-86), new Vector2(420,28), TextAnchor.MiddleLeft);
            contractLabelText = AddText(panel.transform, "ContractLabel", "DISPATCH BOARD", 11, new Color(.36f,.83f,.95f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-122), new Vector2(420,20), TextAnchor.MiddleLeft);
            clientText = AddText(panel.transform, "Client", "NO CONTRACT SELECTED", 18, Color.white,
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-145), new Vector2(420,26), TextAnchor.MiddleLeft);
            shipText = AddText(panel.transform, "Ship", "VESSEL // --", 13, new Color(.72f,.82f,.9f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-174), new Vector2(420,22), TextAnchor.MiddleLeft);
            contaminationText = AddText(panel.transform, "Contamination", "CONTAMINATION // --", 12, new Color(.95f,.56f,.35f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-198), new Vector2(420,22), TextAnchor.MiddleLeft);

            stageText = AddText(panel.transform, "Service Stage", "PROCESS // PRE-RINSE", 12, new Color(.25f,.9f,1f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-222), new Vector2(420,20), TextAnchor.MiddleLeft);
            var progressBack = MakeImage(panel.transform, "Progress Track", new Color(.08f,.14f,.2f,1),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-248), new Vector2(420,12));
            progressFill = MakeImage(progressBack.transform, "Progress Fill", new Color(.12f,.82f,1f,1),
                new Vector2(0,0), new Vector2(1,1), Vector2.zero, Vector2.zero);
            progressFill.type = Image.Type.Filled;
            progressFill.fillMethod = Image.FillMethod.Horizontal;
            progressFill.fillAmount = 0;
            progressText = AddText(panel.transform, "Progress Text", "CLEANLINESS  //  0%", 11, new Color(.65f,.82f,.9f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-266), new Vector2(420,20), TextAnchor.MiddleLeft);
            supplyText = AddText(panel.transform, "Supplies", "WATER 100%  //  FOAM 100%", 11, new Color(.95f,.72f,.35f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-292), new Vector2(420,20), TextAnchor.MiddleLeft);
            rushText = AddText(panel.transform, "Express Bonus", "", 11, new Color(1f,.75f,.34f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-317), new Vector2(420,18), TextAnchor.MiddleLeft);
            zonePanel = MakePanel(canvasObject.transform, "Service Checklist", new Color(.018f,.035f,.065f,.86f),
                new Vector2(1,1), new Vector2(1,1), new Vector2(-28,-28), new Vector2(430,280));
            AddPanelChrome(zonePanel, new Color(1f,.57f,.24f,.95f));
            AddText(zonePanel.transform, "Zone Heading", "HULL MAP  //  TAB TO TOGGLE", 13, new Color(.36f,.83f,.95f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(20,-18), new Vector2(390,24), TextAnchor.MiddleLeft);
            zonesText = AddText(zonePanel.transform, "Zone Checklist", "Accept a contract to inspect the hull.", 13,
                new Color(.72f,.84f,.9f), new Vector2(0,1), new Vector2(0,1), new Vector2(20,-54),
                new Vector2(390,145), TextAnchor.UpperLeft);
            AddText(zonePanel.transform, "Water label", "WATER", 10, new Color(.44f,.82f,1f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(20,-214), new Vector2(175,18), TextAnchor.MiddleLeft);
            AddText(zonePanel.transform, "Foam label", "FOAM MIX", 10, new Color(1f,.68f,.34f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(220,-214), new Vector2(175,18), TextAnchor.MiddleLeft);
            var waterBack = MakeImage(zonePanel.transform, "Water supply track", new Color(.045f,.11f,.16f,1f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(20,-238), new Vector2(175,8));
            waterFill = MakeImage(waterBack.transform, "Water supply fill", new Color(.2f,.78f,1f,1f),
                new Vector2(0,0), new Vector2(1,1), Vector2.zero, Vector2.zero);
            var foamBack = MakeImage(zonePanel.transform, "Foam supply track", new Color(.15f,.09f,.055f,1f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(220,-238), new Vector2(175,8));
            foamFill = MakeImage(foamBack.transform, "Foam supply fill", new Color(1f,.58f,.24f,1f),
                new Vector2(0,0), new Vector2(1,1), Vector2.zero, Vector2.zero);

            var statePanel = MakePanel(canvasObject.transform, "Shift Status Banner", new Color(.018f,.035f,.065f,.82f),
                new Vector2(.5f,0), new Vector2(.5f,0), new Vector2(0,50), new Vector2(850,46));
            AddPanelChrome(statePanel, new Color(.18f,.86f,1f,.95f));
            stateText = AddText(canvasObject.transform, "Contract State", "CONTRACT REQUIRED", 15, new Color(.25f,.9f,1f),
                new Vector2(.5f,0), new Vector2(.5f,0), new Vector2(0,50), new Vector2(810,42), TextAnchor.MiddleCenter);
            instructionText = AddText(canvasObject.transform, "Controls", "WASD MOVE  //  E INTERACT  //  LMB WASH  //  1 WATER  2 FOAM  3 RINSE  //  TAB HULL MAP",
                12, new Color(.65f,.77f,.85f), new Vector2(.5f,0), new Vector2(.5f,0),
                new Vector2(0,18), new Vector2(900,22), TextAnchor.MiddleCenter);
            AddText(canvasObject.transform, "Reticle", "·", 28, new Color(.72f,.97f,1f),
                new Vector2(.5f,.5f), new Vector2(.5f,.5f), Vector2.zero, new Vector2(30,30), TextAnchor.MiddleCenter);
            toolText = AddText(canvasObject.transform, "Tool Feedback", "", 13, new Color(.83f,.95f,1f),
                new Vector2(.5f,.5f), new Vector2(.5f,.5f), new Vector2(0,-55), new Vector2(660,48), TextAnchor.MiddleCenter);
            AddReticleBrackets(canvasObject.transform);
            BuildToolHotbar(canvasObject.transform);
        }

        void RefreshInterface()
        {
            if (creditsText == null) return;
            float cleanliness = career != null && career.JobAccepted ? career.StageProgress : JobComplete ? 1f : 0f;
            creditsText.text = Credits.ToString("0000") + " GC" +
                (career != null ? "    //    REP " + career.Reputation.ToString("0.00") + " / 5" : string.Empty);
            // Plain Images have no sprite: resize the track rather than relying on
            // Filled mode, which Unity ignores for a sprite-less Image.
            progressFill.rectTransform.anchorMax = new Vector2(cleanliness, 1f);
            progressText.text = career != null && career.JobAccepted
                ? career.StageLabel + "  //  ALL-ZONE TARGET " + Mathf.RoundToInt(cleanliness * 100f) + "%"
                : JobComplete ? "CONTRACT CERTIFIED" : "ACCEPT A CONTRACT TO BEGIN";

            if (career == null)
            {
                rushText.gameObject.SetActive(false);
                clientText.text = "NO CONTRACT MANAGER";
                shipText.text = "VESSEL // --";
                contaminationText.text = "CONTAMINATION // --";
                stateText.text = "CAREER SYSTEM OFFLINE";
                return;
            }

            clientText.text = career.JobAccepted ? career.Client : "NO ACTIVE CLIENT // CHECK DISPATCH";
            rushText.gameObject.SetActive(career.HasExpressWindow);
            if (career.HasExpressWindow)
            {
                rushText.text = career.RushStatus;
                rushText.color = career.RushBonus > 0
                    ? new Color(1f,.75f,.34f)
                    : new Color(.66f,.77f,.82f);
            }
            contractLabelText.text = career.JobAccepted ? "ACTIVE SERVICE ORDER" : "DISPATCH BOARD // NEXT AVAILABLE JOB";
            shipText.text = "VESSEL // " + career.Ship + (career.JobAccepted ? "     PAYOUT // " + career.Reward + " CR" : string.Empty);
            contaminationText.text = career.JobAccepted ? "HAZARD // " + career.ContaminationType : "HAZARD // INSPECT AT CONTRACT BOARD";
            stageText.text = career.JobAccepted ? "SERVICE PROCESS // " + career.StageLabel : "SERVICE PROCESS // AWAITING DISPATCH";
            supplyText.text = "WATER " + Mathf.RoundToInt(career.Water) + "%  //  FOAM " +
                Mathf.RoundToInt(career.Chemical) + "%" +
                (career.SupplyEfficiencyPercent > 0 ? "  //  LOOP -" + career.SupplyEfficiencyPercent + "%" : string.Empty);
            if (waterFill != null)
                waterFill.rectTransform.anchorMax = new Vector2(Mathf.Clamp01(career.Water / 100f), 1f);
            if (foamFill != null)
                foamFill.rectTransform.anchorMax = new Vector2(Mathf.Clamp01(career.Chemical / 100f), 1f);
            zonesText.text = career.JobAccepted ? career.GetZoneChecklist() :
                JobComplete ? career.LastReceipt : "01  ACCEPT DISPATCH\n02  SWEEP EACH HULL ZONE\n03  INSPECT AND GET PAID\n04  FIT A FAN NOZZLE AT EQUIPMENT";
            if (tool != null)
            {
                toolText.text = tool.Feedback;
                UpdateToolHotbar();
                instructionText.text = "WASD MOVE  //  E INTERACT  //  LMB WASH  //  1 WATER  2 FOAM  3 RINSE  //  TAB HULL MAP" +
                    (tool.FanNozzleInstalled ? "  //  Q " + (tool.FanNozzleActive ? "FAN" : "PRECISION") : "  //  ESC CURSOR");
            }

            if (career.JobAccepted && career.CanSubmit)
            {
                contractLabelText.text = "FINAL INSPECTION READY";
                stateText.text = "INSPECTION READY // GRADE " + career.QualityGrade +
                    " // RETURN TO CONTRACT TERMINAL AND PRESS E";
                stateText.color = new Color(1f,.78f,.32f);
            }
            else if (career.JobAccepted)
            {
                stateText.text = career.LastMessage;
                stateText.color = new Color(.25f,.9f,1f);
            }
            else if (JobComplete)
            {
                contractLabelText.text = "CERTIFIED // PAYMENT RECEIVED";
                stateText.text = "PAYMENT RECEIVED +" + career.LastPayout + " CR // REFILL OR FIT A FAN NOZZLE\n" +
                    (career.CanAcceptJob ? "NEXT CONTRACT READY AT DISPATCH" : "CUSTOMER DEPARTING // NEXT SHIP INBOUND");
                stateText.color = new Color(.35f,1f,.62f);
            }
            else if (!career.CanAcceptJob)
            {
                contractLabelText.text = "CUSTOMER VESSEL IN TRANSIT";
                stateText.text = "CUSTOMER SHIP IN TRANSIT  //  STAND BY FOR DOCKING";
                stateText.color = new Color(.35f,.78f,1f);
            }
            else
            {
                contractLabelText.text = "DISPATCH BOARD // NEXT AVAILABLE JOB";
                stateText.text = "NEW CONTRACT AVAILABLE  //  ACCEPT AT THE CONTRACT TERMINAL";
                stateText.color = new Color(1f,.72f,.28f);
            }
        }

        static Image MakeImage(Transform parent, string objectName, Color color,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
        {
            var go = new GameObject(objectName, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = new Vector2(anchorMin.x, anchorMin.y);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            var image = go.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        static Image MakePanel(Transform parent, string objectName, Color color,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
        {
            var panel = MakeImage(parent, objectName, color, anchorMin, anchorMax, position, size);
            if (roundedPanelSprite != null)
            {
                panel.sprite = roundedPanelSprite;
                panel.type = Image.Type.Sliced;
            }
            return panel;
        }

        static void EnsureRoundedPanelSprite()
        {
            if (roundedPanelSprite != null) return;
            const int size = 64;
            const float radius = 10f;
            roundedPanelTexture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = "UFO WASH // ROUNDED HUD PANEL",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.DontSave
            };
            var pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float nearestX = Mathf.Clamp(x, radius, size - radius - 1);
                float nearestY = Mathf.Clamp(y, radius, size - radius - 1);
                bool inside = (x - nearestX) * (x - nearestX) +
                    (y - nearestY) * (y - nearestY) <= radius * radius;
                pixels[y * size + x] = inside ? Color.white : new Color(1f,1f,1f,0f);
            }
            roundedPanelTexture.SetPixels(pixels);
            roundedPanelTexture.Apply(false, false);
            roundedPanelSprite = Sprite.Create(roundedPanelTexture, new Rect(0f,0f,size,size),
                new Vector2(.5f,.5f), size, 0, SpriteMeshType.FullRect, new Vector4(12f,12f,12f,12f));
            roundedPanelSprite.name = "UFO WASH // ROUNDED HUD PANEL";
            roundedPanelSprite.hideFlags = HideFlags.DontSave;
        }

        void OnDestroy()
        {
            if (roundedPanelSprite != null) Destroy(roundedPanelSprite);
            if (roundedPanelTexture != null) Destroy(roundedPanelTexture);
            roundedPanelSprite = null;
            roundedPanelTexture = null;
        }

        void BuildToolHotbar(Transform parent)
        {
            var hotbar = MakePanel(parent, "WASHER LOADOUT", new Color(.018f,.035f,.065f,.84f),
                new Vector2(.5f,0), new Vector2(.5f,0), new Vector2(0,111), new Vector2(500,56));
            AddPanelChrome(hotbar, new Color(.18f,.86f,1f,.95f));
            toolModeCards = new Image[3];
            toolModeLabels = new Text[3];
            string[] modes = { "1  PRE-RINSE", "2  FOAM", "3  FINAL RINSE" };
            for (int i = 0; i < modes.Length; i++)
            {
                toolModeCards[i] = MakeImage(hotbar.transform, "TOOL MODE // " + modes[i],
                    new Color(.035f,.085f,.12f,.95f), new Vector2(0,1), new Vector2(0,1),
                    new Vector2(10 + i * 162,-9), new Vector2(156,38));
                toolModeLabels[i] = AddText(toolModeCards[i].transform, "Mode label", modes[i], 11,
                    new Color(.65f,.78f,.84f), Vector2.zero, Vector2.one, new Vector2(5,2),
                    new Vector2(-10,-4), TextAnchor.MiddleCenter);
            }
        }

        void UpdateToolHotbar()
        {
            if (tool == null || toolModeCards == null) return;
            for (int i = 0; i < toolModeCards.Length; i++)
            {
                bool selected = i == (int)tool.selectedMode;
                toolModeCards[i].color = selected
                    ? new Color(.08f,.40f,.49f,.98f)
                    : new Color(.035f,.085f,.12f,.95f);
                toolModeLabels[i].color = selected ? Color.white : new Color(.65f,.78f,.84f);
            }
        }

        static void AddReticleBrackets(Transform parent)
        {
            Color tint = new Color(.35f,.92f,1f,.82f);
            Vector2 center = new Vector2(.5f,.5f);
            Vector2[] offsets =
            {
                new Vector2(-15,-15), new Vector2(15,-15),
                new Vector2(-15,15), new Vector2(15,15)
            };
            for (int i = 0; i < offsets.Length; i++)
            {
                var corner = MakeImage(parent, "Reticle bracket " + i, tint,
                    center, center, offsets[i], new Vector2(8,2));
                corner.transform.localRotation = Quaternion.Euler(0f,0f,(i % 2 == 0) ? 0f : 90f);
                var leg = MakeImage(parent, "Reticle bracket leg " + i, tint,
                    center, center, offsets[i], new Vector2(2,8));
                leg.transform.localRotation = Quaternion.Euler(0f,0f,(i % 2 == 0) ? 0f : 90f);
            }
        }

        static void AddPanelChrome(Image panel, Color accent)
        {
            MakeImage(panel.transform, "Top edge", accent, new Vector2(0,1), new Vector2(1,1),
                new Vector2(0,-1), new Vector2(0,2));
            MakeImage(panel.transform, "Left edge", accent, new Vector2(0,0), new Vector2(0,1),
                new Vector2(1,0), new Vector2(2,0));
        }

        static Text AddText(Transform parent, string objectName, string value, int size, Color color,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 dimensions, TextAnchor alignment)
        {
            var go = new GameObject(objectName, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = new Vector2(anchorMin.x, anchorMin.y);
            rect.anchoredPosition = position;
            rect.sizeDelta = dimensions;
            var text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = size;
            text.color = color;
            text.text = value;
            text.alignment = alignment;
            text.fontStyle = FontStyle.Bold;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.raycastTarget = false;
            return text;
        }
    }
}
