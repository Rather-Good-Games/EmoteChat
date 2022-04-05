using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerARPG
{
    public partial class GameInstance
    {

        [Header("Rather Good Emotes")]
        [Tooltip("Enable Emotes. Removes update and chat checks if false.")]
        public bool enableRatherGoodEmotes = true;

        public bool EnableRatherGoodEmotes
        {
            get { return (enableRatherGoodEmotes && (emoteData != null)); }
            set { enableRatherGoodEmotes = value; }
        }

        [Tooltip("Create -> RatherGoodGames -> EmoteData and set up your animations actions as desired.")]
        [SerializeField] private EmoteData emoteData;

        public EmoteData EmoteData
        {
            get { return emoteData; }
            private set { emoteData = value; }
        }

        //Credit @Neomis for color addition
        [Tooltip("Set Emote text color as desired")]
        [SerializeField] Color colorOfEmoteMessageStringForMe = Color.green;
        public string ColorOfEmoteMessageStringForMe => ColorUtility.ToHtmlStringRGBA(colorOfEmoteMessageStringForMe);

        [Tooltip("Set Emote text color as desired")]
        [SerializeField] Color colorOfEmoteMessageStringForOthers = Color.blue;
        public string ColorOfEmoteMessageStringForOthers => ColorUtility.ToHtmlStringRGBA(colorOfEmoteMessageStringForOthers);
        




    }
}