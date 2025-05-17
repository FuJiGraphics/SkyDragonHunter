using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class AilmentOnStun : MonoBehaviour
        , IAilmentLifecycleHandler
    {
        // 필드 (Fields)
        private Queue<float> m_CashingAnimatorSpeeds;
        private Animator[] m_Animators;
        private MonoBehaviour[] m_ComponentsToDisable;
        private AilmentAffectable m_AffactableComp;
        private GameObject m_Receiver;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void OnEnter(GameObject receiver)
        {
            m_Receiver = receiver;

            DrawableMgr.TopText(receiver.transform.position, "Stun!!!!!", Color.cyan);

            m_ComponentsToDisable = receiver.GetComponents<MonoBehaviour>();
            m_AffactableComp = receiver.GetComponent<AilmentAffectable>();

            foreach (var comp in m_ComponentsToDisable)
            {
                if (comp != receiver && comp != m_AffactableComp)
                    comp.enabled = false;
            }

            m_CashingAnimatorSpeeds = new Queue<float>();
            m_Animators = GetComponentsInChildren<Animator>(true);
            foreach (var animator in m_Animators)
            {
                if (animator != null)
                {
                    animator.speed = 0f;
                    m_CashingAnimatorSpeeds.Enqueue(animator.speed);
                }
            }
        }

        public void OnStay()
        {

        }

        public void OnExit()
        {
            foreach (var comp in m_ComponentsToDisable)
            {
                if (comp != m_Receiver && comp != m_AffactableComp)
                    comp.enabled = true;
            }

            foreach (var animator in m_Animators)
            {
                if (animator != null)
                {
                    animator.speed = m_CashingAnimatorSpeeds.Dequeue();
                }
            }
            m_CashingAnimatorSpeeds = null;
        }


        // Private 메서드
        // Others

    } // Scope by class AilmentOnStun
} // namespace SkyDragonHunter