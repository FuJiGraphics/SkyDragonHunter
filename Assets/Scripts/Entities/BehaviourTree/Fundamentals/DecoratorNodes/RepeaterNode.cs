using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class RepeaterNode<T> : DecoratorNode<T> where T : MonoBehaviour
    {
        protected int m_CurrentCount;
        protected int m_RepeatCount;

        // Public 메서드
        public RepeaterNode(T context) : base(context)
        {
            // NOT YET IMPLEMENTED           
            m_CurrentCount = 0;
            m_RepeatCount = -1;
        }

        public RepeaterNode(T context, int repeatCount) : base(context)
        {
            if(repeatCount < 0)
            {
                Debug.LogError($"Cannot input repeatCount less than 0, input : '{repeatCount}'");
            }
            m_CurrentCount = 0;
            m_RepeatCount = repeatCount;
        }

        // Protected 메서드
        protected override NodeStatus ProcessChild()
        {
            throw new System.NotImplementedException();

            if (m_CurrentCount >= m_RepeatCount)
            {
                return NodeStatus.Success;
            }

            //if(repeatCount)


            return NodeStatus.Running;
        }
    } // Scope by class RepeaterNode
} // namespace Root