using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UFOWASH
{
    public class UfoWashHUD : MonoBehaviour
    {
        public static int Credits;
        public static bool JobComplete;

        Text creditsText, contractLabelText, clientText, shipText, contaminationText, stateText, instructionText, progressText, stageText, supplyText, zonesText;
        Image progressFill;
        UfoWashCareerLoop career;

        void Start()
        {
            career = UfoWashCareerLoop.Instance;
            BuildInterface();
        }

        void Update()
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                if (career != null) career.ResetActiveJob();
                else UfoWashDirtSurface.ResetJob();
            }

            if (career == null) career = UfoWashCareerLoop.Instance;
            RefreshInterface();
        }

        void BuildInterface()
        {
            var canvasObject = new GameObject("UFO WASH // DIEGETIC HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = .5f;

            var panel = MakeImage(canvasObject.transform, "Mission Panel", new Color(.018f,.035f,.065f,.94f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(28,-28), new Vector2(470,448));
            AddText(panel.transform, "Brand", "U F O   /   W A S H", 23, new Color(.24f,.88f,1f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-18), new Vector2(420,34), TextAnchor.MiddleLeft);
            AddText(panel.transform, "Subbrand", "INTERGALACTIC VEHICLE CARE  //  BAY 01", 11, new Color(.48f,.66f,.78f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-52), new Vector2(420,22), TextAnchor.MiddleLeft);

            creditsText = AddText(panel.transform, "Credits", "CREDITS  //  0000", 19, new Color(1f,.72f,.28f),
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
            AddText(panel.transform, "Zone Heading", "HULL SERVICE ZONES", 11, new Color(.36f,.83f,.95f),
                new Vector2(0,1), new Vector2(0,1), new Vector2(24,-320), new Vector2(420,18), TextAnchor.MiddleLeft);
            zonesText = AddText(panel.transform, "Zone Checklist", "Accept a contract to inspect the hull.", 11,
                new Color(.72f,.84f,.9f), new Vector2(0,1), new Vector2(0,1), new Vector2(24,-342),
                new Vector2(420,96), TextAnchor.UpperLeft);

            stateText = AddText(canvasObject.transform, "Contract State", "CONTRACT REQUIRED", 17, new Color(.25f,.9f,1f),
                new Vector2(.5f,0), new Vector2(.5f,0), new Vector2(0,46), new Vector2(760,30), TextAnchor.MiddleCenter);
            instructionText = AddText(canvasObject.transform, "Controls", "WASD MOVE  //  E INTERACT  //  LMB SPRAY  //  1 PRE-RINSE  2 FOAM  3 FINAL RINSE  //  R RESET",
                12, new Color(.65f,.77f,.85f), new Vector2(.5f,0), new Vector2(.5f,0),
                new Vector2(0,18), new Vector2(900,22), TextAnchor.MiddleCenter);
            AddText(canvasObject.transform, "Reticle", "+", 25, new Color(.6f,.95f,1f),
                new Vector2(.5f,.5f), new Vector2(.5f,.5f), Vector2.zero, new Vector2(30,30), TextAnchor.MiddleCenter);
        }

        void RefreshInterface()
        {
            if (creditsText == null) return;
            float cleanliness = career != null && career.JobAccepted ? career.Quality : 1f - UfoWashDirtSurface.TotalDirt();
            creditsText.text = "CREDITS  //  " + Credits.ToString("0000") +
                (career != null ? "     REPUTATION  //  " + career.Reputation.ToString("0.0") + " ★" : string.Empty);
            progressFill.fillAmount = cleanliness;
            progressText.text = "CLEANLINESS  //  " + Mathf.RoundToInt(cleanliness * 100f) + "%";

            if (career == null)
            {
                clientText.text = "NO CONTRACT MANAGER";
                shipText.text = "VESSEL // --";
                contaminationText.text = "CONTAMINATION // --";
                stateText.text = "CAREER SYSTEM OFFLINE";
                return;
            }

            clientText.text = career.JobAccepted ? career.Client : "NO ACTIVE CLIENT // CHECK DISPATCH";
            contractLabelText.text = career.JobAccepted ? "ACTIVE SERVICE ORDER" : "DISPATCH BOARD // NEXT AVAILABLE JOB";
            shipText.text = "VESSEL // " + career.Ship + (career.JobAccepted ? "     PAYOUT // " + career.Reward + " CR" : string.Empty);
            contaminationText.text = career.JobAccepted ? "HAZARD // " + career.ContaminationType : "HAZARD // INSPECT AT CONTRACT BOARD";
            stageText.text = "SERVICE PROCESS // " + career.StageLabel;
            supplyText.text = "WATER " + Mathf.RoundToInt(career.Water) + "%  //  FOAM CHEMICAL " + Mathf.RoundToInt(career.Chemical) + "%";
            zonesText.text = career.GetZoneChecklist();

            if (career.JobAccepted && career.CanSubmit)
            {
                contractLabelText.text = "FINAL INSPECTION READY";
                stateText.text = "INSPECTION READY // GRADE " + career.QualityGrade +
                    " // RETURN TO CONTRACT TERMINAL AND PRESS E";
                stateText.color = new Color(1f,.78f,.32f);
            }
            else if (career.JobAccepted)
            {
                stateText.text = career.LastMessage + "  //  " + career.StageLabel + "  //  HOLD LEFT MOUSE";
                stateText.color = new Color(.25f,.9f,1f);
            }
            else if (JobComplete)
            {
                contractLabelText.text = "CERTIFIED // PAYMENT RECEIVED";
                stateText.text = career.LastMessage + "  //  CONTRACT COMPLETE  //  NEXT JOB AT TERMINAL";
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
            return image;
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
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }
    }
}
