using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public enum NodeStatus
    {
        Success,
        Failure,
        Running,
    }

    public abstract class BehaviourNode<T> where T : MonoBehaviour
    {
        // 필드 (Fields)
        protected readonly T m_Context;
        private bool m_IsStarted;
 
        // Public 메서드        
        public virtual void Reset()
        {
            m_IsStarted = false;
        }

        public NodeStatus Execute()
        {
            if (!m_IsStarted)
            {
                m_IsStarted = true;
                OnStart();
            }

            NodeStatus status = OnUpdate();
            if(status != NodeStatus.Running)
            {
                OnEnd();
                m_IsStarted = false;
            }

            return status;
        }

        // Protected 메서드
        protected BehaviourNode(T context)
        {
            this.m_Context = context;
        }

        protected virtual void OnStart()
        {

        }

        protected abstract NodeStatus OnUpdate();

        protected virtual void OnEnd()
        {

        }    
    } // Scope by class BehaviourNode

} // namespace Root