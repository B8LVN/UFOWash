using UnityEngine;
using UnityEngine.UI;

namespace UFOWASH
{
    [DisallowMultipleComponent]
    public sealed class UfoWashCustomerNpc : UfoWashInteractable
    {
        [SerializeField] string customerName = "ZORP";
        [SerializeField] float lookRange = 11f;
        [SerializeField] float idleBobHeight = 0.035f;
        [SerializeField] float idleBobSpeed = 2.1f;

        static readonly string[] Conversation =
        {
            "My scout crossed three asteroid fields. Please get that sludge off the hull.",
            "The cockpit glass is delicate. A gentle pass, please.",
            "Foam first, then rinse it clean. I want my ship looking new.",
            "I have another route to fly once the inspection is done."
        };

        Transform playerCamera;
        Vector3 homePosition;
        Transform bubbleRoot;
        Text bubbleText;
        Text speakerText;
        Image bubbleBackground;
        int conversationIndex;
        float messageUntil;
        string currentMessage;
        string idleMessage = "My scout needs a proper wash. Can you help?";
        string customerTrait = "customer";

        void Awake()
        {
            homePosition = transform.position;
            BuildSpeechBubble();
            var collider = GetComponent<CapsuleCollider>();
            if (collider == null)
            {
                collider = gameObject.AddComponent<CapsuleCollider>();
                collider.center = Vector3.zero;
                collider.height = 2.15f;
                collider.radius = 0.48f;
            }
            SetMessage(idleMessage, 8f);
        }

        void Update()
        {
            if (playerCamera == null && Camera.main != null) playerCamera = Camera.main.transform;

            transform.position = homePosition + Vector3.up * (Mathf.Sin(Time.time * idleBobSpeed) * idleBobHeight);
            if (Time.time >= messageUntil && currentMessage != idleMessage)
                SetMessage(idleMessage, float.MaxValue);

            if (playerCamera != null)
            {
                Vector3 look = playerCamera.position - transform.position;
                look.y = 0f;
                if (look.sqrMagnitude > 0.05f)
                {
                    Quaternion target = Quaternion.LookRotation(look.normalized);
                    transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 1.8f);
                }

                bool nearby = Vector3.Distance(playerCamera.position, transform.position) <= lookRange;
                if (bubbleRoot != null) bubbleRoot.gameObject.SetActive(nearby);
            }
        }

        public override string GetPrompt() => "E  TALK TO " + customerName;

        public override void Interact(GameObject player)
        {
            string line = conversationIndex == 0
                ? "I'm " + customerName + ", your " + customerTrait.ToLowerInvariant() + ". " + Conversation[0]
                : Conversation[conversationIndex % Conversation.Length];
            conversationIndex++;
            SetMessage(line, 9f);
        }

        public void SetCustomerIdentity(string displayName, string trait)
        {
            if (!string.IsNullOrWhiteSpace(displayName)) customerName = displayName;
            if (!string.IsNullOrWhiteSpace(trait)) customerTrait = trait;
            idleMessage = "I'm " + customerName + ". " + customerTrait + " // awaiting hull certification.";
            if (speakerText != null) speakerText.text = customerName.ToUpperInvariant() + " // " + customerTrait;
        }

        public void OnContractAccepted()
        {
            SetMessage("Great. I'll wait here while you clean the scout. Please don't miss the cockpit.", 8f);
        }

        public void OnContractCompleted(string grade) => OnContractCompleted(grade, 0);

        public void OnContractCompleted(string grade, int tip)
        {
            string tipLine = tip > 0 ? " You earned my " + tip + " credit tip." : " I couldn't justify a tip this time.";
            SetMessage("Inspection grade " + grade + ". Payment sent." + tipLine + " See you on my next route.", 10f);
        }

        public void OnShipDeparted()
        {
            SetMessage("Perfect service. I'll be back when the next route gets messy.", 8f);
        }

        public void SetDialogueVisible(bool visible)
        {
            if (bubbleRoot != null) bubbleRoot.gameObject.SetActive(visible);
        }

        void SetMessage(string message, float seconds)
        {
            currentMessage = message;
            if (bubbleText != null) bubbleText.text = message;
            messageUntil = Time.time + seconds;
            if (bubbleBackground != null) bubbleBackground.enabled = true;
        }

        void BuildSpeechBubble()
        {
            // Customer dialogue is a subtitle card on the HUD, not tiny world-space text
            // floating above the alien. Screen-space also keeps the text readable in both
            // third-person exploration and first-person cleaning views.
            var canvasObject = new GameObject("CUSTOMER SUBTITLES // HUD", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
            bubbleRoot = canvasObject.transform;

            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 80;
            canvas.pixelPerfect = true;
            canvasObject.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920f, 1080f);
            canvasObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
            var canvasRect = canvasObject.GetComponent<RectTransform>();
            canvasRect.anchorMin = Vector2.zero;
            canvasRect.anchorMax = Vector2.one;
            canvasRect.offsetMin = Vector2.zero;
            canvasRect.offsetMax = Vector2.zero;

            var backgroundObject = new GameObject("Subtitle Card", typeof(RectTransform), typeof(Image));
            backgroundObject.transform.SetParent(canvasObject.transform, false);
            var backgroundRect = backgroundObject.GetComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(0.5f, 0f);
            backgroundRect.anchorMax = new Vector2(0.5f, 0f);
            backgroundRect.pivot = new Vector2(0.5f, 0f);
            backgroundRect.anchoredPosition = new Vector2(0f, 38f);
            backgroundRect.sizeDelta = new Vector2(960f, 150f);
            bubbleBackground = backgroundObject.GetComponent<Image>();
            bubbleBackground.color = new Color(0.012f, 0.035f, 0.065f, 0.96f);
            bubbleBackground.raycastTarget = false;

            var accentObject = new GameObject("Cyan Accent", typeof(RectTransform), typeof(Image));
            accentObject.transform.SetParent(backgroundObject.transform, false);
            var accentRect = accentObject.GetComponent<RectTransform>();
            accentRect.anchorMin = new Vector2(0f, 0f);
            accentRect.anchorMax = new Vector2(0f, 1f);
            accentRect.pivot = new Vector2(0f, 0.5f);
            accentRect.sizeDelta = new Vector2(5f, 0f);
            accentRect.anchoredPosition = Vector2.zero;
            var accentImage = accentObject.GetComponent<Image>();
            accentImage.color = new Color(0.1f, 0.86f, 1f, 1f);
            accentImage.raycastTarget = false;

            var speakerObject = new GameObject("Speaker Label", typeof(RectTransform), typeof(Text));
            speakerObject.transform.SetParent(backgroundObject.transform, false);
            var speakerRect = speakerObject.GetComponent<RectTransform>();
            speakerRect.anchorMin = new Vector2(0f, 1f);
            speakerRect.anchorMax = new Vector2(1f, 1f);
            speakerRect.pivot = new Vector2(0f, 1f);
            speakerRect.offsetMin = new Vector2(28f, -39f);
            speakerRect.offsetMax = new Vector2(-24f, -10f);
            speakerText = speakerObject.GetComponent<Text>();
            speakerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            speakerText.fontSize = 16;
            speakerText.fontStyle = FontStyle.Bold;
            speakerText.color = new Color(0.18f, 0.86f, 1f);
            speakerText.text = "CUSTOMER // " + customerName;
            speakerText.alignment = TextAnchor.MiddleLeft;
            speakerText.raycastTarget = false;

            var textObject = new GameObject("Customer Dialogue", typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(backgroundObject.transform, false);
            var textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(28f, 18f);
            textRect.offsetMax = new Vector2(-24f, -48f);
            bubbleText = textObject.GetComponent<Text>();
            bubbleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            bubbleText.fontSize = 24;
            bubbleText.fontStyle = FontStyle.Normal;
            bubbleText.color = new Color(0.88f, 0.96f, 1f);
            bubbleText.alignment = TextAnchor.UpperLeft;
            bubbleText.horizontalOverflow = HorizontalWrapMode.Wrap;
            bubbleText.verticalOverflow = VerticalWrapMode.Overflow;
            bubbleText.raycastTarget = false;

            bubbleRoot.gameObject.SetActive(false);
        }
    }
}


