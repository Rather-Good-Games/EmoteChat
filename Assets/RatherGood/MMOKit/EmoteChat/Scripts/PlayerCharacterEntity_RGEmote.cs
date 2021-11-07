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

        //Old(legacy?)
        private AnimatorCharacterModel animatorCharacterModel_ForEmote;  //local reference

        //alt(new) if using PlayableCharacterModel 
        private GameData.Model.Playables.PlayableCharacterModel playableCharacterModel_ForEmote;

        //Ref if we own this routine
        Coroutine thisActionRoutineRef = null;

        [DevExtMethods("Awake")]
        protected void AwakeRGEmote()
        {
            thisActionRoutineRef = null;

            animatorCharacterModel_ForEmote = GetComponent<AnimatorCharacterModel>();

            //Check if using newer animator version
            playableCharacterModel_ForEmote = GetComponent<GameData.Model.Playables.PlayableCharacterModel>();

            ClientGenericActions.onClientReceiveChatMessage += ReceiveChatMessage;
            onUpdate += RGEmotePlayerUpdate;
        }

        [DevExtMethods("OnDestroy")]
        protected void OnDestroyRGEmote()
        {
            ClientGenericActions.onClientReceiveChatMessage -= ReceiveChatMessage;
            onUpdate -= RGEmotePlayerUpdate;
        }

        /// <summary>
        /// Checks chat messages for incoming emotes if they belong to this entity. 
        /// Then plays the appropriate animation.
        /// </summary>
        /// <param name="msg"></param>
        public void ReceiveChatMessage(ChatMessage msg)
        {

            if (CurrentGameInstance.EnableRatherGoodEmotes)
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
                        if (CurrentGameInstance.EmoteData.GetBySlashCmdText(cmd, out EmoteAnimationData emoteAnimationData))
                        {

                            if (playableCharacterModel_ForEmote != null)
                                StartCoroutine(PlayEmoteAnimationPCM(emoteAnimationData));
                            else
                                StartCoroutine(PlayEmoteAnimation(emoteAnimationData));

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Start emote process and check for cancel or movement state change
        /// </summary>
        /// <param name="emoteAnimationData"></param>
        /// <returns></returns>
        public IEnumerator PlayEmoteAnimation(EmoteAnimationData emoteAnimationData)
        {

            foreach (var actionAnimation in emoteAnimationData.actionAnimations)
            {
                thisActionRoutineRef = animatorCharacterModel_ForEmote.PlayActionAnimationDirectly(actionAnimation);

                while (true)
                {
                    if (!CanDoActions()) //another action probably, cancel this.
                    {
                        CancelEmoteAnimations(false); //no need to overwrite animation if another action already did
                        yield break; //done
                    }
                    else if (!animatorCharacterModel_ForEmote.playActionAnimationDirectlyRunning)//break from while, check for another animation or exit if done.
                    {
                        break; //next?
                    }
                    else if (emoteAnimationData.cancelOnMovementState) //This is still running so check for movement state.
                    {
                        if (EntityIsMoving())
                        {
                            CancelEmoteAnimations(true);
                            yield break; //done
                        }
                    }

                    yield return null;
                }
            }
        }

        public IEnumerator PlayEmoteAnimationPCM(EmoteAnimationData emoteAnimationData)
        {

            foreach (var actionAnimation in emoteAnimationData.actionAnimations)
            {

                thisActionRoutineRef = playableCharacterModel_ForEmote.PlayActionAnimationDirectly(actionAnimation, emoteAnimationData.avatarMask);

                while (true)
                {
                    if (!CanDoActions()) //another action probably, cancel this.
                    {
                        CancelEmoteAnimations(false); //no need to overwrite animation if another action already did
                        yield break; //done
                    }
                    else if (!playableCharacterModel_ForEmote.IsDoingAction())//break from while, check for another animation or exit if done.
                    {
                        break; //Next?
                    }
                    else if (emoteAnimationData.cancelOnMovementState) //This is still running so check for movement state.
                    {
                        if (EntityIsMoving())
                        {
                            CancelEmoteAnimations(true);
                            yield break; //done
                        }
                    }

                    yield return null;
                }
            }

            thisActionRoutineRef = null;

        }

        private void CancelEmoteAnimations(bool stopActionAnimationIfPlaying)
        {
            animatorCharacterModel_ForEmote?.CancelPlayingActionAnimationDirectly(stopActionAnimationIfPlaying);
            playableCharacterModel_ForEmote?.CancelPlayingActionAnimationDirectly();
            thisActionRoutineRef = null;
        }

        private bool EntityIsMoving()
        {
            return (!MovementState.HasFlag(MovementState.IsGrounded) ||
                    Entity.MovementState.HasFlag(MovementState.Forward) ||
                    Entity.MovementState.HasFlag(MovementState.Backward) ||
                    Entity.MovementState.HasFlag(MovementState.Left) ||
                    Entity.MovementState.HasFlag(MovementState.Right) ||
                    Entity.MovementState.HasFlag(MovementState.IsJump));
        }



        void RGEmotePlayerUpdate()
        {

            if (CurrentGameInstance.EnableRatherGoodEmotes)
            {
                if (CanDoActions())
                {
                    if (CurrentGameInstance.EmoteData.CheckInputManagerOnUpdate(out EmoteAnimationData emoteAnimationData))
                    {
                        string tempCmd = emoteAnimationData.slashCmdText;
                        if (!tempCmd.StartsWith("/"))
                            tempCmd = '/' + tempCmd;

                        GameInstance.ClientChatHandlers.SendChatMessage(new ChatMessage()
                        {
                            channel = ChatChannel.Local,
                            message = tempCmd,
                            sender = CharacterName,
                            receiver = string.Empty, //TODO: Could also get player target for point
                        });
                    }
                }
            }

        }

    }
}
