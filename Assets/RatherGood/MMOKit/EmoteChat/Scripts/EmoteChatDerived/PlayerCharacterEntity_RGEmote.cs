using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static MultiplayerARPG.EmoteData;

namespace MultiplayerARPG
{

    //This part of the class will handle player animations for all visible players
    //Finds the UIChatHandler_RGEmote to retrieve animation data
    public partial class PlayerCharacterEntity
    {

        [Header("Rather Good Emotes")]
        [Tooltip("Enable Emote animation.")]
        public bool enableRatherGoodEmotes = true;

        UIChatHandler_RGEmote _uIChatHandler; //local handler

        public UIChatHandler_RGEmote UIChatHandlerRef
        {
            get
            {
                if (_uIChatHandler == null)
                    _uIChatHandler = FindObjectOfType<UIChatHandler_RGEmote>();
                return _uIChatHandler;
            }
        }

        private AnimatorCharacterModel animatorCharacterModel_ForEmote;  //local reference

        [DevExtMethods("Awake")]
        protected void AwakeRGEmote()
        {
            animatorCharacterModel_ForEmote = GetComponent<AnimatorCharacterModel>();
            ClientGenericActions.onClientReceiveChatMessage += ReceiveChatMessage;
        }

        [DevExtMethods("OnDestroy")]
        protected void OnDestroyRGEmote()
        {
            ClientGenericActions.onClientReceiveChatMessage -= ReceiveChatMessage;
        }

        /// <summary>
        /// Checks chat messages for incoming emotes if they belong to this entity. 
        /// Then plays the appropriate animation.
        /// </summary>
        /// <param name="msg"></param>
        public void ReceiveChatMessage(ChatMessage msg)
        {
            if (enableRatherGoodEmotes)
            {

                if (msg.sender != CharacterName)
                    return;

                if (msg.channel != ChatChannel.Local)
                    return;

                string tempChatMessage = msg.message.Trim();

                if (tempChatMessage.Length == 0)
                    return;

                string[] splitText = tempChatMessage.Split(' ');
                if (splitText.Length > 0)
                {
                    string cmd = splitText[0].ToLower(); //Grab first item and set all lower case
                    if (cmd.StartsWith("/"))
                    {
                        if (UIChatHandlerRef.EmoteData.GetBySlashCmdText(cmd, out EmoteAnimationData emoteAnimationData))
                        {
                            PlayActionAnimationDirectly(emoteAnimationData);
                        }
                    }
                }
            }
        }


        public void PlayActionAnimationDirectly(EmoteAnimationData emoteAnimationData)
        {
            animatorCharacterModel_ForEmote.PlayActionAnimationDirectly(emoteAnimationData.actionAnimation);
        }

    }
}
