using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace UFOWASH
{
    public sealed class UfoWashContractTerminalUI : MonoBehaviour
    {
        static UfoWashContractTerminalUI instance;

        Canvas canvas;
        UfoWashCareerLoop career;
        UfoWashFirstPersonController playerController;
        readonly Image[] cardImages = new Image[3];
        readonly Text[] cardSelectionLabels = new Text[3];
        int selectedOffer;

        public static bool IsOpen => instance != null && instance.canvas != null && instance.canvas.enabled;

        public static void Open(UfoWashCareerLoop loop)
        {
            if (loop == null) return;
            if (instance == null)
            {
                var root = new GameObject("UFO WASH // CONTRACT SELECTION UI");
                instance = root.AddComponent<UfoWashContractTerminalUI>();
            }
            instance.Show(loop);
        }

        void Awake()
        {
            BuildInterface();
        }

        void Update()
        {
            if (IsOpen && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                Close();
        }

        void Show(UfoWashCareerLoop loop)
        {
            career = loop;
            selectedOffer = 0;
            playerController = FindFirstObjectByType<UfoWashFirstPersonController>();
            if (playerController != null) playerController.SetInputSuspended(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canvas.enabled = true;
            RefreshCards();
            EventSystem.current?.SetSelectedGameObject(null);
        }

        public void Close()
        {
            if (canvas != null) canvas.enabled = false;
            if (playerController != null) playerController.SetInputSuspended(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void AcceptSelected()
        {
            if (career == null) { Close(); return; }
            career.AcceptJob(selectedOffer);
            if (career.JobAccepted) Close();
            else RefreshCards();
        }

        void SelectOffer(int index)
        {
            selectedOffer = Mathf.Clamp(index, 0, 2);
            RefreshCards();
        }

        void RefreshCards()
        {
            for (int i = 0; i < 3; i++)
            {
                if (cardImages[i] != null)
                    cardImages[i].color = i == selectedOffer
                        ? new Color(0.06f, 0.27f, 0.34f, 0.98f)
                        : new Color(0.025f, 0.065f, 0.095f, 0.97f);
                if (cardSelectionLabels[i] != null)
                    cardSelectionLabels[i].text = i == selectedOffer ? "SELECTED  /  READY" : "SELECT CONTRACT";
            }
        }

        void BuildInterface()
        {
            EnsureEventSystem();

            var root = new GameObject("Contract Board Canvas", typeof(RectTransform), typeof(Canvas),
                typeof(CanvasScaler), typeof(GraphicRaycaster));
            root.transform.SetParent(transform, false);
            canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 120;
            var scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            var shade = MakeImage(root.transform, "Backdrop", new Color(0.005f, 0.012f, 0.024f, 0.82f),
                new Vector2(0f, 0f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);
            var panel = MakeImage(shade.transform, "Main Panel", new Color(0.018f, 0.045f, 0.07f, 0.99f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(1120f, 690f));

            MakeText(panel.transform, "Eyebrow", "U F O   /   W A S H     —     BAY 01 OPERATIONS", 17,
                new Color(0.25f, 0.85f, 0.96f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(48f, -38f), new Vector2(800f, 28f), TextAnchor.MiddleLeft);
            MakeText(panel.transform, "Heading", "AVAILABLE SERVICE CONTRACTS", 36, Color.white,
                new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(48f, -78f),
                new Vector2(900f, 52f), TextAnchor.MiddleLeft);
            MakeText(panel.transform, "Subheading", "Choose a customer job. Higher contamination pays more, but takes longer to finish.", 18,
                new Color(0.61f, 0.74f, 0.82f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(48f, -132f), new Vector2(1000f, 32f), TextAnchor.MiddleLeft);

            for (int i = 0; i < 3; i++) BuildOfferCard(panel.transform, i);

            MakeText(panel.transform, "Footer Hint", "WASD / MOUSE PAUSED WHILE THE CONTRACT BOARD IS OPEN   •   ESC TO CLOSE", 14,
                new Color(0.46f, 0.61f, 0.7f), new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(48f, 30f), new Vector2(720f, 26f), TextAnchor.MiddleLeft);

            MakeButton(panel.transform, "ACCEPT SELECTED CONTRACT", new Vector2(245f, 30f),
                new Vector2(330f, 58f), new Color(0.02f, 0.55f, 0.62f), AcceptSelected);
            MakeButton(panel.transform, "CLOSE", new Vector2(495f, 30f),
                new Vector2(150f, 58f), new Color(0.13f, 0.19f, 0.25f), Close);

            canvas.enabled = false;
        }

        void BuildOfferCard(Transform parent, int index)
        {
            float x = -365f + index * 365f;
            var card = MakeImage(parent, "Contract Card " + (index + 1), new Color(0.025f, 0.065f, 0.095f, 0.97f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(x, 8f), new Vector2(330f, 400f));
            cardImages[index] = card;

            MakeText(card.transform, "Contract Number", "JOB 0" + (index + 1), 15,
                new Color(0.28f, 0.84f, 0.93f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(24f, -22f), new Vector2(280f, 26f), TextAnchor.MiddleLeft);
            MakeText(card.transform, "Contract Name", career != null ? career.GetOfferTitle(index) : "SERVICE CONTRACT", 25,
                Color.white, new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(24f, -62f), new Vector2(280f, 60f), TextAnchor.MiddleLeft);
            MakeText(card.transform, "Customer", career != null ? career.GetOfferClient(index) : "CUSTOMER", 16,
                new Color(0.7f, 0.82f, 0.9f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(24f, -128f), new Vector2(280f, 30f), TextAnchor.MiddleLeft);
            MakeText(card.transform, "Description", career != null ? career.GetOfferBrief(index) : "Inspect and service the arriving vessel.", 17,
                new Color(0.73f, 0.82f, 0.87f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(24f, -174f), new Vector2(280f, 110f), TextAnchor.UpperLeft);
            MakeText(card.transform, "Contamination", career != null ? career.GetOfferContamination(index) : "CONTAMINATION", 14,
                new Color(1f, 0.62f, 0.34f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(24f, -292f), new Vector2(280f, 32f), TextAnchor.MiddleLeft);
            MakeText(card.transform, "Payout", career != null ? career.GetOfferReward(index).ToString("N0") + " CR" : "250 CR", 29,
                new Color(1f, 0.75f, 0.34f), new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(24f, 66f), new Vector2(280f, 42f), TextAnchor.MiddleLeft);

            var select = MakeButton(card.transform, "SELECT CONTRACT", new Vector2(0f, 18f),
                new Vector2(282f, 46f), new Color(0.08f, 0.2f, 0.27f), () => SelectOffer(index));
            cardSelectionLabels[index] = select.GetComponentInChildren<Text>();
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

        static Text MakeText(Transform parent, string objectName, string value, int size, Color color,
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
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        static Button MakeButton(Transform parent, string label, Vector2 position, Vector2 size,
            Color color, Action callback)
        {
            var go = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0f);
            rect.anchorMax = new Vector2(0.5f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;

            var image = go.GetComponent<Image>();
            image.color = color;
            var button = go.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => callback());

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(go.transform, false);
            var labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            var text = labelObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = label;
            text.fontSize = 17;
            text.fontStyle = FontStyle.Bold;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return button;
        }

        static void EnsureEventSystem()
        {
            var eventSystem = FindFirstObjectByType<EventSystem>();
            if (eventSystem == null)
                eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule))
                    .GetComponent<EventSystem>();

            var inputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
            if (inputModule == null) inputModule = eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            if (inputModule.actionsAsset == null) inputModule.AssignDefaultActions();
        }

        void OnDestroy()
        {
            if (playerController != null) playerController.SetInputSuspended(false);
            if (instance == this) instance = null;
        }
    }
}