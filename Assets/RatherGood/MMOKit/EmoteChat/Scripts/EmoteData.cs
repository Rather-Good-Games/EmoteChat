using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerARPG
{

    [System.Serializable]
    public struct EmoteAnimationData
    {
        [Tooltip("Text entered in chat to trigger animation. EX: 'dance'; Slash '/' is not needed here and will be stripped off before checks.")]
        public string slashCmdText;

        [Tooltip("KeyName from InputSettingsManager settings.")]
        public string keyName;

        public ActionAnimation[] actionAnimations;

        [Tooltip("(Optional) Can provide AvatarMask if using 'PlayableCharacterModel' component.")]
        public AvatarMask avatarMask;

        [Tooltip("Display text after message for you. EX: [You 'are dancing.']")]
        public string emoteMessageStringForMe;

        [Tooltip("Display text after message for others. EX: [Player2 'is dancing.']")]
        public string emoteMessageStringForOthers;

        public string GetMessageForEmote(bool itIsMe, string whoIsDoingAction)
        {
            if (itIsMe)
            {
                return "<color=#" + GameInstance.Singleton.ColorOfEmoteMessageStringForMe + ">You " + emoteMessageStringForMe + "</color>";
            }
            else
            {
                return "<color=#" + GameInstance.Singleton.ColorOfEmoteMessageStringForOthers + ">" + whoIsDoingAction + " " + emoteMessageStringForOthers + "</color>";
            }
        }

        [Tooltip("Option to cancel emote animation if movement state changes.")]
        public bool cancelOnMovementState;
    }

    [CreateAssetMenu(fileName = "EmoteData", menuName = "RatherGoodGames/EmoteData", order = 1)]
    public class EmoteData : ScriptableObject
    {

        public EmoteAnimationData[] emoteAnimationData;

        [NonSerialized] public Dictionary<string, EmoteAnimationData> keyNameDict = new Dictionary<string, EmoteAnimationData>();

        [NonSerialized] public Dictionary<string, EmoteAnimationData> slashCmdDictDict = new Dictionary<string, EmoteAnimationData>();

        [NonSerialized] private bool initialized = false;

        /// <summary>
        /// Initialize dictionaries
        /// </summary>
        public void Init()
        {
            keyNameDict.Clear();
            slashCmdDictDict.Clear();
            foreach (EmoteAnimationData item in emoteAnimationData)
            {
                string tempKeyName = item.keyName.TrimStart('/');
                string tempSlashCmdText = item.slashCmdText.TrimStart('/');

                if (!String.IsNullOrEmpty(tempKeyName))
                    keyNameDict.Add(tempKeyName, item);

                if (!String.IsNullOrEmpty(tempSlashCmdText))
                    slashCmdDictDict.Add(tempSlashCmdText.ToLower(), item);  //Always lower case for comparison in chat entry
            }
            initialized = true;
        }

        /// <summary>
        /// Check and retrieve animation data if keyName exists in dictionary.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="emoteAnimationData"></param>
        /// <returns>true if keyName exists.</returns>
        public bool GetByKeyName(string keyName, out EmoteAnimationData emoteAnimationData)
        {

            if (!initialized)
                Init();

            string tempKeyName = keyName.TrimStart('/');
            emoteAnimationData = new EmoteAnimationData();

            if (String.IsNullOrEmpty(keyName))
                return false;

            return (keyNameDict.TryGetValue(tempKeyName, out emoteAnimationData));

        }

        /// <summary>
        /// Check and retrieve animation data if slashCmdText exists in dictionary
        /// </summary>
        /// <param name="slashCmdText"></param>
        /// <param name="emoteAnimationData"></param>
        /// <returns>True if slashCmdText exists.</returns>
        public bool GetBySlashCmdText(string slashCmdText, out EmoteAnimationData emoteAnimationData)
        {

            if (!initialized)
                Init();

            string tempSlashCmdText = slashCmdText.TrimStart('/');
            emoteAnimationData = new EmoteAnimationData();

            if (String.IsNullOrEmpty(slashCmdText))
                return false;

            if (slashCmdDictDict.TryGetValue(tempSlashCmdText, out emoteAnimationData))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Call form Update function.  Will check any existing keyNames for button down activation.
        /// </summary>
        /// <param name="emoteAnimationData"></param>
        /// <returns></returns>
        public bool CheckInputManagerOnUpdate(out EmoteAnimationData emoteAnimationData)
        {
            if (!initialized)
                Init();

            emoteAnimationData = new EmoteAnimationData();

            foreach (var item in keyNameDict)
            {
                if (GetByKeyName(item.Key, out emoteAnimationData))
                {
                    if (InputManager.GetButtonDown(item.Key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }


}