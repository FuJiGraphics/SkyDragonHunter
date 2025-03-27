using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class SequenceNode<T> : CompositeNode<T> where T : MonoBehaviour
    {
        // Public 메서드
        public SequenceNode(T context) : base(context)
        {
            m_currentChild = 0;
        }

        public override void Reset()
        {
            base.Reset();
            m_currentChild = 0;
        }

        // Protected 메서드
        protected override void OnStart()
        {
            base.OnStart();
            m_currentChild = 0;
        }

        protected override NodeStatus OnUpdate()
        {
            if(m_ChildNodes.Count == 0)
            {
                return NodeStatus.Success;
            }

            while (m_currentChild < m_ChildNodes.Count)
            {
                var childStatus = m_ChildNodes[m_currentChild].Execute();
                if(childStatus != NodeStatus.Success)
                {
                    return childStatus;
                }
                ++m_currentChild;
            }

            return NodeStatus.Success;
        }
    } // Scope by class SequenceNode
} // namespace Root