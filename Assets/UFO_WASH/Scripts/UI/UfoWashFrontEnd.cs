using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace UFOWASH
{
    /// <summary>
    /// Lightweight in-scene front end for the first playable chapter.
    /// Keeps the existing service bay as the 3D title backdrop, then hands control
    /// to the first-person game only after the player has read the briefing/tutorial.
    /// </summary>
    public sealed class UfoWashFrontEnd : MonoBehaviour
    {
        static readonly Color Ink = new Color(0.025f, 0.045f, 0.075f, 0.96f);
        static readonly Color Panel = new Color(0.045f, 0.085f, 0.12f, 0.94f);
        static readonly Color Cyan = new Color(0.22f, 0.88f, 0.96f, 1f);
        static readonly Color Muted = new Color(0.66f, 0.77f, 0.82f, 1f);
        static readonly Color White = new Color(0.93f, 0.97f, 0.98f, 1f);

        Canvas canvas;
        Transform pageRoot;
        Font font;
        UfoWashFirstPersonController playerController;
        UfoWashPlayerInteraction playerInteraction;
        UfoWashSprayTool sprayTool;
        bool gameplayStarted;

        void Awake()
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerController = FindFirstObjectByType<UfoWashFirstPersonController>();
            playerInteraction = FindFirstObjectByType<UfoWashPlayerInteraction>();
            sprayTool = FindFirstObjectByType<UfoWashSprayTool>();
            if (playerController != null) playerController.enabled = false;
            if (playerInteraction != null) playerInteraction.enabled = false;
            if (sprayTool != null) sprayTool.enabled = false;

            EnsureEventSystem();
            BuildCanvas();
            ShowMainMenu();
        }

        void OnDestroy()
        {
            if (!gameplayStarted) Time.timeScale = 1f;
        }

        void EnsureEventSystem()
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                eventSystem = new GameObject("EVENT SYSTEM // UI INPUT", typeof(EventSystem))
                    .GetComponent<EventSystem>();
            var inputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
            if (inputModule == null)
                inputModule = eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            if (inputModule.actionsAsset == null) inputModule.AssignDefaultActions();
        }

        void BuildCanvas()
        {
            var root = new GameObject("FRONT END CANVAS", typeof(RectTransform), typeof(Canvas),
                typeof(CanvasScaler), typeof(GraphicRaycaster));
            root.transform.SetParent(transform, false);
            canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 250;
            var scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }

        void ShowMainMenu()
        {
            NewPage();
            AddText(pageRoot, "INTERSTELLAR SERVICE NETWORK  //  EST. 2189", 17, Cyan,
                new Vector2(.08f,.86f), new Vector2(.68f,.92f), TextAnchor.MiddleLeft);
            AddText(pageRoot, "UFO", 94, White, new Vector2(.075f,.64f), new Vector2(.48f,.84f), TextAnchor.MiddleLeft, FontStyle.Bold);
            AddText(pageRoot, "WASH", 94, Cyan, new Vector2(.075f,.53f), new Vector2(.52f,.70f), TextAnchor.MiddleLeft, FontStyle.Bold);
            AddText(pageRoot, "THE LAST CLEAN BAY ON THE OUTER RIM", 22, Muted,
                new Vector2(.08f,.48f), new Vector2(.57f,.54f), TextAnchor.MiddleLeft);
            AddText(pageRoot, "Every ship has a story. Every stain has a price.", 18, White,
                new Vector2(.08f,.43f), new Vector2(.58f,.48f), TextAnchor.MiddleLeft);

            ButtonAt("START FIRST SHIFT", new Vector2(.08f,.32f), new Vector2(.29f,.385f), Cyan, ShowBriefing);
            ButtonAt("STORY", new Vector2(.08f,.235f), new Vector2(.22f,.29f), Panel, ShowStory);
            ButtonAt("HOW TO PLAY", new Vector2(.245f,.235f), new Vector2(.39f,.29f), Panel, ShowHowTo);

            PanelAt(new Vector2(.65f,.24f), new Vector2(.91f,.78f), Panel);
            AddText(pageRoot, "YOUR STATION // BAY 01", 16, Cyan,
                new Vector2(.68f,.70f), new Vector2(.89f,.75f), TextAnchor.MiddleLeft, FontStyle.Bold);
            AddText(pageRoot, "A forgotten wash bay.\nA mountain of debt.\nA whole galaxy that leaves a mess.", 25, White,
                new Vector2(.68f,.48f), new Vector2(.89f,.68f), TextAnchor.UpperLeft);
            AddText(pageRoot, "ONE SHIP AT A TIME. BUILD SOMETHING THAT LASTS.", 13, Muted,
                new Vector2(.68f,.29f), new Vector2(.89f,.39f), TextAnchor.MiddleLeft);
            AddText(pageRoot, "VERSION 0.1  //  EARLY ACCESS BUILD", 12, Muted,
                new Vector2(.08f,.06f), new Vector2(.48f,.10f), TextAnchor.MiddleLeft);
        }

        void ShowStory()
        {
            NewPage();
            Header("THE STORY", "THE OUTER RIM DOESN'T FORGIVE A DIRTY SHIP.");
            AddText(pageRoot,
                "YEAR 2189.\n\nThe old interstellar trade lanes are booming, but the independent pilots who keep them alive are being squeezed by fuel taxes, docking fees and corporate repair monopolies.\n\nAt the edge of the Vesper Belt sits Bay 01: a half-forgotten wash station with failing equipment, unpaid bills and one contract left before the doors are sealed.\n\nYou inherited the keys. Not the money. Not the staff. Just the bay, a pressure washer and a chance to turn a dead stop into the busiest service station in the sector.\n\nYour first customer is already on approach.",
                21, White, new Vector2(.12f,.26f), new Vector2(.72f,.72f), TextAnchor.UpperLeft);
            ButtonAt("BACK TO TITLE", new Vector2(.12f,.13f), new Vector2(.29f,.20f), Panel, ShowMainMenu);
            ButtonAt("TAKE THE KEYS", new Vector2(.32f,.13f), new Vector2(.52f,.20f), Cyan, ShowBriefing);
        }

        void ShowBriefing()
        {
            NewPage();
            Header("SHIFT 001  //  BAY 01", "KEEP THE LIGHTS ON.");
            AddText(pageRoot,
                "A scout ship is inbound with asteroid dust and ion residue across its hull. The pilot needs a clean vessel before the next jump.\n\nYour station is not running itself. Read the service request, take the contract, treat the hull, rinse it clean and earn your first credits. A good finish means more than a bigger payout: it is the first step toward rebuilding this place.\n\nDISPATCH: ZORP — ORION TRADERS\nVESSEL: CLASS-IV SCOUT\nJOB: FIRST-CONTACT DECONTAMINATION",
                22, White, new Vector2(.12f,.30f), new Vector2(.72f,.70f), TextAnchor.UpperLeft);
            ButtonAt("BACK", new Vector2(.12f,.13f), new Vector2(.25f,.20f), Panel, ShowMainMenu);
            ButtonAt("CONTINUE TO TRAINING", new Vector2(.29f,.13f), new Vector2(.57f,.20f), Cyan, ShowHowTo);
        }

        void ShowHowTo()
        {
            NewPage();
            Header("FIELD MANUAL  //  BASIC OPERATIONS", "LEARN THE JOB. THEN MAKE IT YOURS.");
            string controls =
                "MOVE  —  W A S D\nLOOK  —  MOUSE\nINTERACT  —  E\nWASHER  —  LEFT MOUSE BUTTON\nTOOL MODE  —  1 PRE-RINSE  /  2 FOAM  /  3 FINAL RINSE\n\n01  ACCEPT THE CONTRACT AT THE JOB TERMINAL.\n02  PRE-RINSE THE HULL TO REMOVE LOOSE DUST.\n03  SWITCH TO FOAM. COAT THE DIRTY SURFACES, NOT THE AIR.\n04  FINAL-RINSE THE TREATMENT UNTIL THE HULL IS CLEAN.\n05  RETURN TO THE TERMINAL. REQUEST INSPECTION AND COLLECT PAYMENT.\n\nTIP: WATCH THE SERVICE STATUS. THE NEXT STEP UNLOCKS WHEN ENOUGH OF THE SHIP HAS BEEN TREATED.";
            AddText(pageRoot, controls, 20, White,
                new Vector2(.12f,.25f), new Vector2(.75f,.75f), TextAnchor.UpperLeft);
            ButtonAt("BACK", new Vector2(.12f,.13f), new Vector2(.25f,.20f), Panel, ShowMainMenu);
            ButtonAt("CLOCK IN  //  START WORK", new Vector2(.29f,.13f), new Vector2(.61f,.20f), Cyan, StartGameplay);
        }

        void StartGameplay()
        {
            if (gameplayStarted) return;
            gameplayStarted = true;
            if (pageRoot != null) Destroy(pageRoot.gameObject);
            Time.timeScale = 1f;
            if (sprayTool != null) sprayTool.enabled = true;
            if (playerInteraction != null) playerInteraction.enabled = true;
            if (playerController != null)
            {
                playerController.enabled = true;
                playerController.SetInputSuspended(false);
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Destroy(gameObject);
        }

        void Header(string eyebrow, string headline)
        {
            PanelAt(new Vector2(.10f,.22f), new Vector2(.82f,.91f), new Color(.025f,.055f,.085f,.94f));
            AddText(pageRoot, eyebrow, 16, Cyan, new Vector2(.12f,.82f), new Vector2(.76f,.88f), TextAnchor.MiddleLeft, FontStyle.Bold);
            AddText(pageRoot, headline, 31, White, new Vector2(.12f,.73f), new Vector2(.80f,.81f), TextAnchor.MiddleLeft, FontStyle.Bold);
        }

        void NewPage()
        {
            if (pageRoot != null) Destroy(pageRoot.gameObject);
            var page = new GameObject("FRONT END PAGE", typeof(RectTransform), typeof(Image));
            page.transform.SetParent(canvas.transform, false);
            var rect = page.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
            var image = page.GetComponent<Image>();
            image.color = new Color(.012f,.025f,.045f,.90f);
            pageRoot = page.transform;
        }

        void PanelAt(Vector2 min, Vector2 max, Color color)
        {
            var panel = new GameObject("PANEL", typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(pageRoot, false);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = min; rect.anchorMax = max; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
            panel.GetComponent<Image>().color = color;
        }

        void ButtonAt(string label, Vector2 min, Vector2 max, Color color, UnityEngine.Events.UnityAction action)
        {
            var buttonObject = new GameObject("BUTTON // " + label, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(pageRoot, false);
            var rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = min; rect.anchorMax = max; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
            var image = buttonObject.GetComponent<Image>();
            image.color = color;
            var button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;
            var colors = button.colors;
            colors.normalColor = color;
            colors.highlightedColor = Color.Lerp(color, Color.white, .18f);
            colors.pressedColor = Color.Lerp(color, Color.black, .2f);
            button.colors = colors;
            button.onClick.AddListener(action);

            var textObject = new GameObject("LABEL", typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(buttonObject.transform, false);
            var textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero; textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(12, 4); textRect.offsetMax = new Vector2(-12, -4);
            var text = textObject.GetComponent<Text>();
            text.font = font; text.text = label; text.fontSize = 17; text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter; text.color = color == Cyan ? Ink : White;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
        }

        void AddText(Transform parent, string value, int size, Color color,
            Vector2 min, Vector2 max, TextAnchor alignment, FontStyle style = FontStyle.Normal)
        {
            var textObject = new GameObject("TEXT // " + value.Substring(0, Mathf.Min(24, value.Length)).Replace("\n", " "),
                typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);
            var rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = min; rect.anchorMax = max; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
            var text = textObject.GetComponent<Text>();
            text.font = font; text.text = value; text.fontSize = size; text.fontStyle = style;
            text.color = color; text.alignment = alignment; text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.lineSpacing = 1.15f;
        }
    }
}
