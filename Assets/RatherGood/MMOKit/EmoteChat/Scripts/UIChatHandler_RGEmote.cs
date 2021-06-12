using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MultiplayerARPG
{
    /// <summary>
    /// This class replaces "UIChatHandler" on the "UIChat_Standalone" 
    /// component of your GameInstance.
    /// </summary>
    public partial class UIChatHandler_RGEmote : UIBase
    {

        public static readonly List<ChatMessage> ChatMessages = new List<ChatMessage>();

        public string globalCommand = "/a";
        public string whisperCommand = "/w";
        public string partyCommand = "/p";
        public string guildCommand = "/g";
        public string systemCommand = "/s";
        public KeyCode enterChatKey = KeyCode.Return;
        public int chatEntrySize = 30;
        public GameObject[] enterChatActiveObjects;
        public InputFieldWrapper uiEnterChatField;
        public UIChatMessage_RGEmote uiChatMessagePrefab;
        public Transform uiChatMessageContainer;
        public ScrollRect scrollRect;
        public bool clearPreviousChatMessageOnStart;

        public bool EnterChatFieldVisible { get; private set; }

        [Header("Rather Good Emotes")]
        [Tooltip("Enable Emotes. Removes update and chat checks if false.")]
        public bool enableRatherGoodEmotes = true;

        [Tooltip("Create -> RatherGoodGames -> EmoteData and set up your animations actions as desired.")]
        [SerializeField] private EmoteData emoteData;

        public EmoteData EmoteData
        {
            get { return emoteData; }
            private set { emoteData = value; }
        }

        public string EnterChatMessage
        {
            get { return uiEnterChatField == null ? string.Empty : uiEnterChatField.text; }
            set { if (uiEnterChatField != null) uiEnterChatField.text = value; }
        }

        private UIList cacheList;
        public UIList CacheList
        {
            get
            {
                if (cacheList == null)
                {
                    cacheList = gameObject.AddComponent<UIList>();
                    cacheList.uiPrefab = uiChatMessagePrefab.gameObject;
                    cacheList.uiContainer = uiChatMessageContainer;
                }
                return cacheList;
            }
        }

        private bool movingToEnd;

        private void Start()
        {
            if (clearPreviousChatMessageOnStart)
            {
                ChatMessages.Clear();
            }
            else
            {
                CacheList.Generate(ChatMessages, (index, message, ui) =>
                {
                    UIChatMessage_RGEmote uiChatMessage = ui.GetComponent<UIChatMessage_RGEmote>();
                    uiChatMessage.uiChatHandler = this;
                    uiChatMessage.Data = message;
                    uiChatMessage.Show();
                });
            }
            StartCoroutine(VerticalScroll(0f));

            HideEnterChatField();
            if (uiEnterChatField != null)
            {
                uiEnterChatField.onValueChanged.RemoveListener(OnInputFieldValueChange);
                uiEnterChatField.onValueChanged.AddListener(OnInputFieldValueChange);
            }
        }

        private void OnEnable()
        {
            ClientGenericActions.onClientReceiveChatMessage += OnReceiveChat;
        }

        private void OnDisable()
        {
            ClientGenericActions.onClientReceiveChatMessage -= OnReceiveChat;
        }

        private void Update()
        {
            if (movingToEnd)
            {
                movingToEnd = false;
                uiEnterChatField.MoveTextEnd(false);
            }
            if (Input.GetKeyUp(enterChatKey))
            {
                if (!EnterChatFieldVisible)
                    ShowEnterChatField();
                else
                    SendChatMessage();
            }

            if (enableRatherGoodEmotes)
            {

                if (((PlayerCharacterEntity)GameInstance.PlayingCharacterEntity).CanDoActions())
                {
                    if (EmoteData.CheckInputManagerOnUpdate(out EmoteAnimationData emoteAnimationData))
                    {

                        string tempCmd = emoteAnimationData.slashCmdText;
                        if (!tempCmd.StartsWith("/"))
                            tempCmd = '/' + tempCmd;

                        EnterChatMessage = tempCmd;
                        SendChatMessage();
                    }
                }
            }
        }

        public void ToggleEnterChatField()
        {
            if (EnterChatFieldVisible)
                HideEnterChatField();
            else
                ShowEnterChatField();
        }

        public void ShowEnterChatField()
        {
            foreach (GameObject enterChatActiveObject in enterChatActiveObjects)
            {
                if (enterChatActiveObject != null)
                    enterChatActiveObject.SetActive(true);
            }
            if (uiEnterChatField != null)
            {
                uiEnterChatField.ActivateInputField();
                EventSystem.current.SetSelectedGameObject(uiEnterChatField.gameObject);
                movingToEnd = true;
            }
            EnterChatFieldVisible = true;
        }

        public void HideEnterChatField()
        {
            foreach (GameObject enterChatActiveObject in enterChatActiveObjects)
            {
                if (enterChatActiveObject != null)
                    enterChatActiveObject.SetActive(false);
            }
            if (uiEnterChatField != null)
            {
                uiEnterChatField.DeactivateInputField();
                EventSystem.current.SetSelectedGameObject(null);
            }
            EnterChatFieldVisible = false;
        }

        public void SendChatMessage()
        {
            if (GameInstance.PlayingCharacter == null)
                return;

            string trimText = EnterChatMessage.Trim();
            if (trimText.Length == 0)
                return;

            EnterChatMessage = string.Empty;
            ChatChannel channel = ChatChannel.Local;
            string message = trimText;
            string sender = GameInstance.PlayingCharacter.CharacterName;
            string receiver = string.Empty;
            string[] splitedText = trimText.Split(' ');
            if (splitedText.Length > 0)
            {
                string cmd = splitedText[0];
                if (cmd == whisperCommand && splitedText.Length > 2)
                {
                    channel = ChatChannel.Whisper;
                    receiver = splitedText[1];
                    message = trimText.Substring(cmd.Length + receiver.Length + 2); // +2 for space
                    EnterChatMessage = trimText.Substring(0, cmd.Length + receiver.Length + 2); // +2 for space
                }
                if ((cmd == globalCommand || cmd == partyCommand || cmd == guildCommand || cmd == systemCommand) && splitedText.Length > 1)
                {
                    if (cmd == globalCommand)
                        channel = ChatChannel.Global;
                    if (cmd == partyCommand)
                        channel = ChatChannel.Party;
                    if (cmd == guildCommand)
                        channel = ChatChannel.Guild;
                    if (cmd == systemCommand)
                        channel = ChatChannel.System;
                    message = trimText.Substring(cmd.Length + 1); // +1 for space
                    EnterChatMessage = trimText.Substring(0, cmd.Length + 1); // +1 for space
                }
            }

            GameInstance.ClientChatHandlers.SendChatMessage(new ChatMessage()
            {
                channel = channel,
                message = message,
                sender = sender,
                receiver = receiver,
            });
            HideEnterChatField();
        }

        private void OnReceiveChat(ChatMessage chatMessage)
        {
            chatMessage = CheckAndReplaceChatMsgEmotes(chatMessage);

            ChatMessages.Add(chatMessage);
            if (ChatMessages.Count > chatEntrySize)
                ChatMessages.RemoveAt(0);

            UIChatMessage_RGEmote tempUiChatMessage;
            CacheList.Generate(ChatMessages, (index, message, ui) =>
            {
                tempUiChatMessage = ui.GetComponent<UIChatMessage_RGEmote>();
                tempUiChatMessage.uiChatHandler = this;
                tempUiChatMessage.Data = message;
                tempUiChatMessage.Show();
            });

            StartCoroutine(VerticalScroll(0f));
        }

        public ChatMessage CheckAndReplaceChatMsgEmotes(ChatMessage chatMessage)
        {

            if (chatMessage.channel != ChatChannel.Local)
                return chatMessage;

            string tempChatMessage = chatMessage.message.Trim();

            if (tempChatMessage.Length == 0)
                return chatMessage;

            string[] splitText = tempChatMessage.Split(' ');
            if (splitText.Length > 0)
            {
                string cmd = splitText[0].ToLower(); //Grab first item and set all lower case
                if (cmd.StartsWith("/"))
                {
                    if (EmoteData.GetBySlashCmdText(cmd, out EmoteAnimationData emoteAnimationData))
                    {
                        if (chatMessage.sender == GameInstance.PlayingCharacter.CharacterName)
                        {
                            chatMessage.message = emoteAnimationData.GetMessageForEmote(true, GameInstance.PlayingCharacter.CharacterName);
                        }
                        else
                        {
                            chatMessage.message = emoteAnimationData.GetMessageForEmote(false, chatMessage.sender);
                        }
                    }
                }
            }

            return chatMessage;
        }

        private void OnInputFieldValueChange(string text)
        {
            if (text.Length > 0 && !EnterChatFieldVisible)
                ShowEnterChatField();
        }

        IEnumerator VerticalScroll(float normalize)
        {
            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                yield return null;
                scrollRect.verticalScrollbar.value = normalize;
                Canvas.ForceUpdateCanvases();
            }
        }

    }
}
