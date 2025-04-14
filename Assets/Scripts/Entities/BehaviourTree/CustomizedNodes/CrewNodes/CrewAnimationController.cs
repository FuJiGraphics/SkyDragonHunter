using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class CrewAnimationController : MonoBehaviour
    {
        // Static Fields
        private static readonly string s_AnimTagIdle1 = "Idle_1";        

        // Private Fields
        [SerializeField] private string[] m_AttackAnimTriggers;
        [SerializeField] private string[] m_SkillAnimTriggers;
        private SkeletonAnimation m_SkeletonAnim;

        // Unity Methods
        private void Awake()
        {
            m_SkeletonAnim = GetComponentInChildren<SkeletonAnimation>();
        }

        // Public Methods
        public void PlayIdleAnimation(bool loop = true)
        {            
            m_SkeletonAnim.AnimationState.SetAnimation(0, s_AnimTagIdle1, loop);
        }

        public void PlaySkillAnimation(bool loop = false)
        {
            int randomTriggerIndex = Random.Range(0, m_AttackAnimTriggers.Length);
            m_SkeletonAnim.AnimationState.SetAnimation(0, m_SkillAnimTriggers[randomTriggerIndex], loop);
        }

        public void PlayAttackAnimation(bool loop = false)
        {
            int randomTriggerIndex = Random.Range(0, m_AttackAnimTriggers.Length);
            m_SkeletonAnim.AnimationState.SetAnimation(0, m_AttackAnimTriggers[randomTriggerIndex], loop);
        }


    } // Scope by class AnimationController

} // namespace Root