using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class AilmentOnFreeze : MonoBehaviour
        , IAilmentLifecycleHandler
    {
        // �ʵ� (Fields)
        private Queue<float> m_CashingAnimatorSpeeds;
        private Animator[] m_Animators;
        private MonoBehaviour[] m_ComponentsToDisable;
        private AilmentAffectable m_AffactableComp;
        private GameObject m_Receiver;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        public void OnEnter(GameObject receiver)
        {
            m_Receiver = receiver;

            DrawableMgr.TopText(receiver.transform.position, "Freeze!!!!!", Color.blue);
            receiver.GetComponent<SpriteRenderer>().color = Color.blue;
            
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
            m_Receiver.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Private �޼���
        // Others

    } // Scope by class AilmentOnSlow
} // namespace SkyDragonHunter