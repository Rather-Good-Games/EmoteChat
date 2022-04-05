using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MultiplayerARPG
{
    /// <summary>
    /// This class replaces "UIChatHandler" on the "UIChat_Standalone" 
    /// Add this line to "UIChatHandler" after null check in "private void OnReceiveChat(ChatMessage chatMessage)"    
    ///     chatMessage = CheckAndReplaceChatMsgEmotes(chatMessage);
    /// component of your GameInstance.
    /// </summary>
    public partial class UIChatHandler
    {
        public ChatMessage CheckAndReplaceChatMsgEmotes(ChatMessage chatMessage)
        {

            string tempChatMessage = chatMessage.message.Trim();

            if (tempChatMessage.Length == 0)
                return chatMessage;

            if (tempChatMessage.StartsWith("/")) //now check this first
            {
                string[] splitText = tempChatMessage.Split(' ');
                if (splitText.Length > 0)
                {
                    string cmd = splitText[0].ToLower(); //Grab first item and set all lower case

                    if (GameInstance.Singleton.EmoteData.GetBySlashCmdText(cmd, out EmoteAnimationData emoteAnimationData))
                    {
                        chatMessage.channel = ChatChannel.Local;

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

    }
}
