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
        Image bubbleBackground;
        int conversationIndex;
        float messageUntil;
        string currentMessage;
        string idleMessage = "My scout needs a proper wash. Can you help?";

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
                if (nearby && bubbleRoot != null)
                {
                    Vector3 toCamera = playerCamera.position - bubbleRoot.position;
                    if (toCamera.sqrMagnitude > 0.01f)
                        bubbleRoot.rotation = Quaternion.LookRotation(toCamera.normalized, Vector3.up);
                }
            }
        }

        public override string GetPrompt() => "E  TALK TO " + customerName;

        public override void Interact(GameObject player)
        {
            string line = Conversation[conversationIndex % Conversation.Length];
            conversationIndex++;
            SetMessage(line, 9f);
        }

        public void OnContractAccepted()
        {
            SetMessage("Great. I'll wait here while you clean the scout. Please don't miss the cockpit.", 8f);
        }

        public void OnContractCompleted(string grade)
        {
            SetMessage("That's immaculate! Grade " + grade + ". Payment sent — thank you, cleaner!", 10f);
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
            var canvasObject = new GameObject("CUSTOMER SPEECH // WORLD SPACE", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
            canvasObject.transform.SetParent(transform, false);
            bubbleRoot = canvasObject.transform;
            canvasObject.transform.localPosition = Vector3.up * 2.55f;
            canvasObject.transform.localScale = Vector3.one * 0.008f;

            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 30;
            var canvasRect = canvasObject.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(440f, 116f);

            var backgroundObject = new GameObject("Speech Bubble Panel", typeof(RectTransform), typeof(Image));
            backgroundObject.transform.SetParent(canvasObject.transform, false);
            var backgroundRect = backgroundObject.GetComponent<RectTransform>();
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            bubbleBackground = backgroundObject.GetComponent<Image>();
            bubbleBackground.color = new Color(0.015f, 0.045f, 0.075f, 0.94f);

            var textObject = new GameObject("Customer Dialogue", typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(backgroundObject.transform, false);
            var textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(24f, 16f);
            textRect.offsetMax = new Vector2(-24f, -16f);
            bubbleText = textObject.GetComponent<Text>();
            bubbleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            bubbleText.fontSize = 30;
            bubbleText.fontStyle = FontStyle.Bold;
            bubbleText.color = new Color(0.78f, 0.95f, 1f);
            bubbleText.alignment = TextAnchor.MiddleLeft;
            bubbleText.horizontalOverflow = HorizontalWrapMode.Wrap;
            bubbleText.verticalOverflow = VerticalWrapMode.Overflow;
        }
    }
}

