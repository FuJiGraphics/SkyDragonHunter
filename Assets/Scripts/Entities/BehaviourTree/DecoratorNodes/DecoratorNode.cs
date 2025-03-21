using SkyDragonHunter.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public abstract class DecoratorNode<T> : BehaviourNode<T> where T : MonoBehaviour
    {
        // 필드 (Fields)
        protected BehaviourNode<T> m_ChildNode;

        // Public 메서드  
        public void SetChild(BehaviourNode<T> child)
        {
            m_ChildNode = child;
        }

        public override void Reset()
        {
            base.Reset();
            if(m_ChildNode != null)
            {
                m_ChildNode.Reset();
            }
        }       

        // Protected 메서드
        protected DecoratorNode(T context) : base(context)
        {
        }

        protected abstract NodeStatus ProcessChild();

        protected override NodeStatus OnUpdate()
        {
            if (m_ChildNode == null)
            {
                return NodeStatus.Failure;
            }
            return ProcessChild();
        }

    } // Scope by class DecorationNode

} // namespace Root