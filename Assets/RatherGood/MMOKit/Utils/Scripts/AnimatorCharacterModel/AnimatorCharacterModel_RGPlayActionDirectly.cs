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
        [Header("Play Action Animation Directly Debug.")]
        public bool playActionAnimationDirectlyRunning;
        [SerializeField] private bool currentAnimationHasClip;

        public Coroutine PlayActionAnimationDirectly(ActionAnimation actionAnimation)
        {
            StopActionAnimation();
            StopSkillCastAnimation();
            StopWeaponChargeAnimation();

            return StartedActionCoroutine(StartCoroutine(PlayActionAnimationDirectly_Coroutine(actionAnimation)));
        }

        protected IEnumerator PlayActionAnimationDirectly_Coroutine(ActionAnimation actionAnimation)
        {
            playActionAnimationDirectlyRunning = true;

            float playSpeedMultiplier = (actionAnimation.animSpeedRate > 0) ? actionAnimation.animSpeedRate : 1f;

            AudioManager.PlaySfxClipAtAudioSource(actionAnimation.GetRandomAudioClip(), GenericAudioSource);

            currentAnimationHasClip = actionAnimation.clip != null && animator.isActiveAndEnabled;
            if (currentAnimationHasClip)
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

            yield return new WaitForSecondsRealtime(actionAnimation.GetClipLength() / playSpeedMultiplier);

            // Waits by current transition + extra duration before end playing animation state
            if (actionAnimation.extendDuration > 0)
                yield return new WaitForSecondsRealtime(actionAnimation.extendDuration / playSpeedMultiplier);

            CancelPlayingActionAnimationDirectly(true);

        }

        public void CancelPlayingActionAnimationDirectly(bool stopActionAnimationIfPlaying)
        {
            if (currentAnimationHasClip && stopActionAnimationIfPlaying)
            {
                StopActionAnimation();
            }
            playActionAnimationDirectlyRunning = false;
        }









    }
}
