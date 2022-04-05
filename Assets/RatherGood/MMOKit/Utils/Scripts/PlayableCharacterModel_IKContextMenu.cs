#if UNITY_EDITOR
using UnityEngine;


//Credit @23rd Silence, MMO Kit discord

namespace MultiplayerARPG.GameData.Model.Playables
{
    public partial class PlayableCharacterModel
    {
        [ContextMenu("Enable 'Apply Playable Ik' On Every Animation")]
        public void EnableIKAll()
        {
            SetIKOn(ref this.defaultAnimations, true);
            SetIKOn(ref this.weaponAnimations, true);
            SetIKOn(ref this.skillAnimations, true);
            Debug.Log("Enabled IK on every animation.");
        }
        
        [ContextMenu("Disable 'Apply Playable Ik' On Every Animation")]
        public void DisableIKAll()
        {
            SetIKOn(ref this.defaultAnimations, false);
            SetIKOn(ref this.weaponAnimations, false);
            SetIKOn(ref this.skillAnimations, false);
            Debug.Log("Disabled IK on every animation.");
        }

        private void SetIKOn(ref SkillAnimations[] anims, bool enabled)
        {
            for (var i = 0; i < anims.Length; i++)
            {
                SetIKOn(ref anims[i], enabled);
            }
        }

        private void SetIKOn(ref SkillAnimations anims, bool enabled)
        {
            SetIKOn(ref anims.castState, enabled);
            SetIKOn(ref anims.activateAnimation, enabled);
        }

        private void SetIKOn(ref WeaponAnimations[] anims, bool enabled)
        {
            for (var i = 0; i < anims.Length; i++)
            {
                SetIKOn(ref anims[i], enabled);
            }
        }

        private void SetIKOn(ref WeaponAnimations anims, bool enabled)
        {
            SetIKOn(ref anims.idleState, enabled);
            SetIKOn(ref anims.moveStates, enabled);
            SetIKOn(ref anims.sprintStates, enabled);
            SetIKOn(ref anims.walkStates, enabled);
            SetIKOn(ref anims.crouchIdleState, enabled);
            SetIKOn(ref anims.crouchMoveStates, enabled);
            SetIKOn(ref anims.crawlIdleState, enabled);
            SetIKOn(ref anims.crawlMoveStates, enabled);
            SetIKOn(ref anims.swimIdleState, enabled);
            SetIKOn(ref anims.swimMoveStates, enabled);
            SetIKOn(ref anims.jumpState, enabled);
            SetIKOn(ref anims.fallState, enabled);
            SetIKOn(ref anims.landedState, enabled);
            SetIKOn(ref anims.hurtState, enabled);
            SetIKOn(ref anims.deadState, enabled);
            SetIKOn(ref anims.pickupState, enabled);
            SetIKOn(ref anims.rightHandChargeState, enabled);
            SetIKOn(ref anims.leftHandChargeState, enabled);
            SetIKOn(ref anims.rightHandAttackAnimations, enabled);
            SetIKOn(ref anims.leftHandAttackAnimations, enabled);
            SetIKOn(ref anims.rightHandReloadAnimation, enabled);
            SetIKOn(ref anims.leftHandReloadAnimation, enabled);
        }

        private void SetIKOn(ref DefaultAnimations anims, bool enabled) {
            SetIKOn(ref anims.idleState, enabled);
            SetIKOn(ref anims.moveStates, enabled);
            SetIKOn(ref anims.sprintStates, enabled);
            SetIKOn(ref anims.walkStates, enabled);
            SetIKOn(ref anims.crouchIdleState, enabled);
            SetIKOn(ref anims.crouchMoveStates, enabled);
            SetIKOn(ref anims.crawlIdleState, enabled);
            SetIKOn(ref anims.crawlMoveStates, enabled);
            SetIKOn(ref anims.swimIdleState, enabled);
            SetIKOn(ref anims.swimMoveStates, enabled);
            SetIKOn(ref anims.jumpState, enabled);
            SetIKOn(ref anims.fallState, enabled);
            SetIKOn(ref anims.landedState, enabled);
            SetIKOn(ref anims.hurtState, enabled);
            SetIKOn(ref anims.deadState, enabled);
            SetIKOn(ref anims.pickupState, enabled);
            SetIKOn(ref anims.rightHandChargeState, enabled);
            SetIKOn(ref anims.leftHandChargeState, enabled);
            SetIKOn(ref anims.rightHandAttackAnimations, enabled);
            SetIKOn(ref anims.leftHandAttackAnimations, enabled);
            SetIKOn(ref anims.rightHandReloadAnimation, enabled);
            SetIKOn(ref anims.leftHandReloadAnimation, enabled);
            SetIKOn(ref anims.skillCastState, enabled);
            SetIKOn(ref anims.skillActivateAnimation, enabled);
        }
        private void SetIKOn(ref AnimState state, bool enabled) => state.applyPlayableIk = enabled;

        private void SetIKOn(ref ActionState state, bool enabled) => state.applyPlayableIk = enabled;

        private void SetIKOn(ref ActionAnimation anim, bool enabled) => SetIKOn(ref anim.state, enabled);

        private void SetIKOn(ref ActionAnimation[] anims, bool enabled)
        {
            for (var i = 0; i < anims.Length; i++)
            {
                SetIKOn(ref anims[i], enabled);
            }
        }
        private void SetIKOn(ref MoveStates states, bool enabled)
        {
            SetIKOn(ref states.forwardState, enabled);
            SetIKOn(ref states.backwardState, enabled);
            SetIKOn(ref states.leftState, enabled);
            SetIKOn(ref states.rightState, enabled);
            SetIKOn(ref states.forwardLeftState, enabled);
            SetIKOn(ref states.forwardRightState, enabled);
            SetIKOn(ref states.backwardLeftState, enabled);
            SetIKOn(ref states.backwardRightState, enabled);
        }
    }
}
#endif
