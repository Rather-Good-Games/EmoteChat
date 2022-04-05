using System.Collections;
using System.Collections.Generic;
using LiteNetLibManager;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MultiplayerARPG.GameData.Model.Playables
{
    /// <summary>
    /// Core mod requires adding 'partial' modifier to 'PlayableCharacterModel'
    /// </summary>
    public partial class PlayableCharacterModel_Custom
    {

        public Coroutine PlayActionAnimationDirectly(MultiplayerARPG.ActionAnimation actionAnimation, AvatarMask avatarMask)
        {
            return StartCoroutine(PlayActionAnimationDirectly_Coroutine(actionAnimation, avatarMask));
        }

        private IEnumerator PlayActionAnimationDirectly_Coroutine(MultiplayerARPG.ActionAnimation actionAnimation, AvatarMask avatarMask)
        {
            isDoingAction = true;

            ActionState tempActionState = new ActionState()
            {
                clip = actionAnimation.clip,
                animSpeedRate = (actionAnimation.animSpeedRate > 0) ? actionAnimation.animSpeedRate : 1f,
                avatarMask = avatarMask,
                transitionDuration = 0,
            };

            AudioManager.PlaySfxClipAtAudioSource(actionAnimation.GetRandomAudioClip(), GenericAudioSource);

            bool hasClip = tempActionState.clip != null;

            if (hasClip) Behaviour.PlayAction(tempActionState, 1, actionAnimation.GetExtendDuration());

            float waitTime = (actionAnimation.GetClipLength() / tempActionState.animSpeedRate) + actionAnimation.GetExtendDuration();

            // Waits by clip duration(adjusted by speed rate) + extra duration before ending emote and stopping action
            yield return new WaitForSecondsRealtime(waitTime);

            // Stop doing action animation
            if (hasClip) Behaviour.StopAction();

            isDoingAction = false;

        }

        public void CancelPlayingActionAnimationDirectly()
        {
            StopActionAnimation();
            StopActionCoroutine();

        }

        public bool IsDoingAction()
        {
            return isDoingAction;
        }

        public Coroutine PlayActionAnimationDirectly(ActionAnimation actionAnimation)
        {
            return StartCoroutine(PlayActionAnimationDirectly_Coroutine(actionAnimation));
        }

        private IEnumerator PlayActionAnimationDirectly_Coroutine(ActionAnimation actionAnimation)
        {
            isDoingAction = true;

            AudioManager.PlaySfxClipAtAudioSource(actionAnimation.GetRandomAudioClip(), GenericAudioSource);

            bool hasClip = actionAnimation.state.clip != null;

            if (hasClip) Behaviour.PlayAction(actionAnimation.state, 1, actionAnimation.GetExtendDuration());

            float waitTime = (actionAnimation.GetClipLength() / actionAnimation.state.animSpeedRate) + actionAnimation.GetExtendDuration();

            // Waits by clip duration(adjusted by speed rate) + extra duration before ending emote and stopping action
            yield return new WaitForSecondsRealtime(waitTime);

            // Stop doing action animation
            if (hasClip) Behaviour.StopAction();

            isDoingAction = false;

        }


    }
}