using System.Collections;
using System.Collections.Generic;
using LiteNetLibManager;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MultiplayerARPG
{



    //Rather Good PlayActionAnimationDirectly.
    //Provide hooks to pass an Action Animation to the animator and play it
    public partial class AnimatorCharacterModel
    {

        public Coroutine PlayActionAnimationDirectly(ActionAnimation actionAnimation)
        {
            StopActionAnimation();
            StopSkillCastAnimation();
            StopWeaponChargeAnimation();

            return StartedActionCoroutine(StartCoroutine(PlayActionAnimationDirectly_Coroutine(actionAnimation)));
        }

        protected IEnumerator PlayActionAnimationDirectly_Coroutine(ActionAnimation actionAnimation)
        {

            float playSpeedMultiplier = 1f;
            if (actionAnimation.animSpeedRate > 0)
                playSpeedMultiplier = actionAnimation.animSpeedRate;

            AudioManager.PlaySfxClipAtAudioSource(actionAnimation.GetRandomAudioClip(), GenericAudioSource);

            bool hasClip = actionAnimation.clip != null && animator.isActiveAndEnabled;
            if (hasClip)
            {
                CacheAnimatorController[CLIP_ACTION] = actionAnimation.clip;
                animator.SetFloat(ANIM_ACTION_CLIP_MULTIPLIER, playSpeedMultiplier);
                animator.SetBool(ANIM_DO_ACTION, true);
                animator.SetBool(ANIM_DO_ACTION_ALL_LAYERS, actionAnimation.playClipAllLayers);
                if (actionAnimation.playClipAllLayers)
                {
                    for (int i = 0; i < animator.layerCount; ++i)
                    {
                        animator.Play(actionStateNameHashes[i], i, 0f);
                    }
                }
                else
                {
                    animator.Play(actionStateNameHashes[actionStateLayer], actionStateLayer, 0f);
                }
            }

            float countDowntimer = actionAnimation.GetClipLength() / playSpeedMultiplier; // 3 seconds you can change this 

            //Read action animator flag can cancel action or countown timer
            while (animator.GetBool(ANIM_DO_ACTION) && countDowntimer > 0)
            {
                countDowntimer -= Time.deltaTime;
                yield return null;
            }  

            // Stop doing action animation
            if (hasClip)
            {
                animator.SetBool(ANIM_DO_ACTION, false);
                animator.SetBool(ANIM_DO_ACTION_ALL_LAYERS, false);
            }
            // Waits by current transition + extra duration before end playing animation state
            yield return new WaitForSecondsRealtime(actionAnimation.GetExtraDuration() / playSpeedMultiplier);

        }


    }
}
